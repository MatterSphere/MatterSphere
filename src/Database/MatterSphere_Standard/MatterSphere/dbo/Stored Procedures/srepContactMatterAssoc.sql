

CREATE PROCEDURE [dbo].[srepContactMatterAssoc]
	@contID bigint ,
	@uI uUICultureInfo = '{default}'
AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

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
	X.cdDesc as assocTypeDesc ,
	Y.cdDesc as fileStatusDesc
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
LEFT JOIN
	dbo.GetCodeLookupDescription ( 'SUBASSOC' , @uI ) X ON X.cdCode = A.assocType
LEFT JOIN
	dbo.GetCodeLookupDescription ( 'FILESTATUS' , @uI ) Y ON Y.cdCode = F.fileStatus
WHERE 
	assocActive = 1 AND
	C.contID = @contID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepContactMatterAssoc] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepContactMatterAssoc] TO [OMSAdminRole]
    AS [dbo];

