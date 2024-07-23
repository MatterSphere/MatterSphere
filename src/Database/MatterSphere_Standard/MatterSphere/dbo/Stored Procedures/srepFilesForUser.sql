

CREATE PROCEDURE [dbo].[srepFilesForUser]
(
	@UI uUICultureInfo = '{default}'
	, @USER int = NULL
)

AS 

DECLARE @SELECT nvarchar(MAX)
DECLARE @WHERE nvarchar(2000)
DECLARE @ORDERBY nvarchar(100)

--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

SELECT     
	dbo.GetFileRef(CL.clNo, F.fileNo) as [fileRef]
	, CL.clName
	, F.filedesc
	, F.created
	, F.fileacccode
	, U.usrFullName
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') as FileTypeDesc
	, COALESCE(CL2.cdDesc, ''~'' + NULLIF(F.fileStatus, '''') + ''~'') as FileStatusDesc
FROM  
	dbo.dbClient CL
INNER JOIN 
	dbo.dbFile F ON CL.clID = F.clID
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
LEFT JOIN dbo.GetCodeLookupDescription ( ''FILETYPE'', @UI ) CL1 ON CL1.[cdCode] = F.fileType
LEFT JOIN dbo.GetCodeLookupDescription ( ''FILESTATUS'', @UI ) CL2 ON CL2.[cdCode] = F.fileStatus '

--- SET THE WHERE STATEMENT
SET @WHERE = ' F.fileStatus LIKE ''LIVE%'' '

--- USER CLAUSE
IF(@USER IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.filePrincipleID = @USER '
END

--- BUILD THE WHERE CLAUSE
IF @WHERE <> ''
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

--- SET ORDER BY STATEMENT
SET @ORDERBY = N' 
ORDER BY 
	CL.clNo  
	, F.fileNo 
	, F.created DESC '

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = @SELECT + @WHERE + @ORDERBY

--- DEBUG PRINT
PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @USER int'
	, @UI
	, @USER

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilesForUser] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilesForUser] TO [OMSAdminRole]
    AS [dbo];

