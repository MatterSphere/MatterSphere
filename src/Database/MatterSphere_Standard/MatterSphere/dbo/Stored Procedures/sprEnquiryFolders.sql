

CREATE PROCEDURE [dbo].[sprEnquiryFolders](@UI uUICultureInfo = '{default}', @CurrentFolderID int = 0)
AS

DECLARE @Type NVARCHAR(1000) =  dbo.GetCodeLookupDesc(N'FOLDERS', 'FORMS', @UI)

SELECT 
	flParentID AS ParentID, 
	flID as ID,
	flCode,
	COALESCE(CL1.cdDesc, '~' + NULLIF(flCode, '') + '~') AS FolderName, 
	COALESCE(CL2.cdDesc, '~' + NULLIF(flType, '') + '~')  AS Type,
	fldIcon
FROM
	dbo.dbFolderStructure
	LEFT JOIN dbo.GetCodeLookupDescription ( 'FOLDERS', @UI ) CL1 ON CL1.[cdCode] =  flCode
	LEFT JOIN dbo.GetCodeLookupDescription ( 'FOLDERS', @UI ) CL2 ON CL2.[cdCode] =  flType
WHERE (ftIntType = 'FORMS') and flForward = @CurrentFolderID
UNION
SELECT
	flParent,
	dbEnquiry.enqID, 
	enqCode,
	COALESCE(CL1.cdDesc, '~' + NULLIF(enqCode, '') + '~') AS FileNamed, 
	(case when dbFolderLinks.fldType is null then @Type  else COALESCE(CL2.cdDesc, '~' + NULLIF(dbFolderLinks.fldType, '') + '~') end) AS Type,	
	convert(smallint,(case when dbFolderLinks.fldIcon is null then 18 else dbFolderLinks.fldIcon end)) AS Icon
FROM   
	dbEnquiry LEFT JOIN dbFolderLinks ON dbEnquiry.enqID = dbFolderLinks.id
	LEFT JOIN dbo.GetCodeLookupDescription ( 'ENQHEADER', @UI ) CL1 ON CL1.[cdCode] =  enqCode
	LEFT JOIN dbo.GetCodeLookupDescription ( 'FOLDERS', @UI ) CL2 ON CL2.[cdCode] =  dbFolderLinks.fldType
WHERE 
	convert(int,(case when dbFolderLinks.flid is null then 0 else dbFolderLinks.flid end)) = @CurrentFolderID
ORDER BY [Type]

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprEnquiryFolders] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprEnquiryFolders] TO [OMSAdminRole]
    AS [dbo];

