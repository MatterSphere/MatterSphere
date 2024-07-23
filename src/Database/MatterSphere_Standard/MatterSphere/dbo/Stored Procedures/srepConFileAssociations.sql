

CREATE PROCEDURE [dbo].[srepConFileAssociations]
(
	@UI uUICultureInfo='{default}',
	@CONTID nvarchar(50) = null
)

AS 

DECLARE @SQL nvarchar(4000)

--- Select Statement for the base Query
SET @SQL = N'
SELECT
	C.contName,
	F.fileID, 
	dbo.GetFileRef(CL.clNo, F.fileNo) as fileRef, 
	CL.clName,
	F.fileDesc,
	F.Created,
	F.fileAllowExternal, 
	U.usrFullName AS createdBy, 
	U1.usrFullName AS updatedBy, 
	U2.usrFullName AS FeeEarner, 
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(A.assocType, '''') + ''~'') as assocTypeDesc,
	COALESCE(CL2.cdDesc, ''~'' + NULLIF(F.fileStatus, '''') + ''~'') as fileStatusDesc
FROM         
	dbo.dbContact C
INNER JOIN
	dbo.dbAssociates A ON C.contID = A.contID 
INNER JOIN
	dbo.dbFile F ON A.fileID = F.fileID 
INNER JOIN
	dbo.dbClient CL ON F.clID = CL.clID 
INNER JOIN
	dbo.dbUser U ON F.CreatedBy = U.usrID 
INNER JOIN
	dbo.dbUser U1 ON F.Updatedby = U1.usrID 
INNER JOIN
	dbo.dbUser U2 ON F.filePrincipleID = U2.usrID 
LEFT JOIN dbo.GetCodeLookupDescription ( ''SUBASSOC'', @UI ) CL1 ON CL1.[cdCode] = A.assocType
LEFT JOIN dbo.GetCodeLookupDescription ( ''FILESTATUS'', @UI ) CL2 ON CL2.[cdCode] = F.fileStatus
WHERE 
	assocActive = 1 AND
	C.contID = COALESCE(@CONTID, C.contID)'

--- Debug Print
print @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @CONTID nvarchar(50)', @UI, @CONTID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepConFileAssociations] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepConFileAssociations] TO [OMSAdminRole]
    AS [dbo];

