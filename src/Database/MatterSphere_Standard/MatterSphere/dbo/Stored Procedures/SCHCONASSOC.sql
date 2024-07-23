CREATE PROCEDURE dbo.SCHCONASSOC
(
	@UI uUICultureInfo = '{default}'
	, @CONTID BIGINT
	, @ORDERBY NVARCHAR(MAX) = NULL
)  
AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @Select NVARCHAR(MAX)

SET @Select = N'
WITH Res AS (
SELECT
	F.fileID
	, dbo.GetFileRef(CL.clNo, F.fileNo) AS fileRef
	, F.fileDesc
	, CO.contID
	, CL.clName
	, F.Created
	, F.CreatedBy
	, U.usrFullName
	, U1.usrFullName AS UpdatedBy
	, U2.usrFullName AS FeeEarner
	, dbo.GetCodeLookupDesc(''SUBASSOC'', A.assocType, @UI) AS assocTypeDesc
	, F.fileStatus
	, dbo.GetCodeLookupDesc(''FILESTATUS'', F.fileStatus,@UI) AS fileStatusDesc
	, F.fileallowexternal
	, A.assocActive
FROM dbo.dbContact CO
	INNER JOIN dbo.dbAssociates A ON A.contID = CO.contID
	INNER JOIN dbo.dbFile F ON F.fileID = A.fileID
	INNER JOIN dbo.dbClient CL ON CL.clID = F.clID 
	INNER JOIN dbo.dbUser U ON U.usrID = F.CreatedBy
	LEFT OUTER JOIN	dbo.dbUser U1 ON U1.usrID  = F.Updatedby
	INNER JOIN dbo.dbUser U2 ON U2.usrID = F.filePrincipleID 
WHERE CO.Contid = @CONTID 
)
SELECT 
	fileID
	, fileRef
	, fileDesc
	, contID
	, clName
	, Created
	, CreatedBy
	, usrFullName
	, UpdatedBy
	, FeeEarner
	, assocTypeDesc
	, fileStatus
	, fileStatusDesc
	, fileallowexternal
	, assocActive
FROM Res
'
IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY fileID'
ELSE 
	IF @ORDERBY NOT LIKE '%fileID%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', fileID'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY
--PRINT @Select
EXEC sp_executesql @Select,  N'@UI uUICultureInfo, @CONTID BIGINT', @UI, @CONTID 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCONASSOC] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCONASSOC] TO [OMSAdminRole]
    AS [dbo];