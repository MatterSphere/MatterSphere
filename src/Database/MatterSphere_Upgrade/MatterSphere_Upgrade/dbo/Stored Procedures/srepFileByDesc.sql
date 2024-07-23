CREATE PROCEDURE [dbo].[srepFileByDesc]
(
	@UI uUICultureInfo = '{default}'
	, @FILEDESC nvarchar(100) = NULL
	, @FILETYPE nvarchar(15) = NULL
	, @DEPARTMENT nvarchar(15) = NULL
	, @BRANCH int = NULL
	, @FUNDTYPE nvarchar(15) = NULL
	, @STATUS nvarchar(15) = NULL
	, @LACAT nvarchar(15) = NULL
	, @EXTERNAL bit = NULL
	, @FEEEARNER int = NULL
)

AS 

DECLARE @SELECT nvarchar(1900)
DECLARE @WHERE nvarchar(2000)
DECLARE @ORDERBY nvarchar(100)

--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

SELECT     
	CL.clName
	, replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') AS FileTypeDesc
	, F.fileNo
	, CL.clNo
    , F.fileLACategory
    , U.usrFullName
	, COALESCE(CL2.cdDesc, ''~'' + NULLIF(F.fileDepartment, '''') + ''~'') AS DepDesc
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
LEFT JOIN dbo.GetCodeLookupDescription ( ''FILETYPE'', @UI ) CL1 ON CL1.[cdCode] = F.fileType
LEFT JOIN dbo.GetCodeLookupDescription ( ''DEPT'', @UI ) CL2 ON CL2.[cdCode] = F.fileDepartment '

---- SET THE WHERE STATEMENT
SET @WHERE = ''

--- FILE DESCRIPTION CLAUSE
IF(@FILEDESC IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' F.fileDesc LIKE ''%'' + @FILEDESC + ''%'' '
END

--- FILE TYPE CLAUSE
IF(@FILETYPE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileType = @FILETYPE '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.fileType = @FILETYPE '
	END
END

--- DEPARTMENT CLAUSE
IF(@DEPARTMENT IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileDepartment = @DEPARTMENT '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.fileDepartment = @DEPARTMENT '
	END
END

--- BRANCH CLAUSE
IF(@BRANCH IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.brID = @BRANCH '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.brID = @BRANCH '
	END
END

--- FUND TYPE CLAUSE
IF(@FUNDTYPE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileFundCode = @FUNDTYPE '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.fileFundCode = @FUNDTYPE '
	END
END 

--- FILE STATUS CLAUSE
IF(@STATUS IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileStatus = @STATUS '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.fileStatus = @STATUS '
	END
END


--- LEGAL AID CATEGORY CLAUSE
IF(@LACAT IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileLACategory = @LACAT '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.fileLACategory = @LACAT '
	END
END

--- EXTERNAL CLAUSE
IF(@EXTERNAL IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileAllowExternal = @EXTERNAL '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.fileAllowExternal = @EXTERNAL '
	END
END   

--- FEE EARNER CLAUSE
IF(@FEEEARNER IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.filePrincipleID = @FEEEARNER '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.filePrincipleID = @FEEEARNER '
	END
END 

--- BUILD THE WHERE CLAUSE
IF @WHERE <> ''
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

--- SET THE ORDER BY STATEMENT
SET @ORDERBY = ' 
ORDER BY
	F.Created
	, F.fileDesc '

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = @SELECT + @WHERE + @ORDERBY

--- DEBUG PRINT
PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @FILEDESC nvarchar(100)
	, @FILETYPE nvarchar(15)
	, @DEPARTMENT nvarchar(15)
	, @BRANCH int
	, @FUNDTYPE nvarchar(15)
	, @STATUS nvarchar(15)
	, @LACAT nvarchar(15)
	, @EXTERNAL bit
	, @FEEEARNER int'
	, @UI
	, @FILEDESC
	, @FILETYPE
	, @DEPARTMENT
	, @BRANCH
	, @FUNDTYPE
	, @STATUS
	, @LACAT
	, @EXTERNAL
	, @FEEEARNER

