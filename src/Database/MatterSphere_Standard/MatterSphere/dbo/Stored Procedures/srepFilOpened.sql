

CREATE PROCEDURE [dbo].[srepFilOpened]
(
	@UI uUICultureInfo = '{default}'
	, @FILETYPE nvarchar(15) = NULL
	, @DEPARTMENT nvarchar(15) = NULL
	, @BRANCH int = NULL
	, @FUNDTYPE nvarchar(15) = NULL
	, @LACAT tinyint = NULL
	, @INTERACTIVE bit = NULL
	, @FEEEARNER int = NULL
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
	, @DATESORT nvarchar(15) = NULL
	, @STATUS nvarchar(15) = NULL
)

AS 

DECLARE @SELECT nvarchar(2500)
DECLARE @WHERE nvarchar(1400)
DECLARE @ORDERBY nvarchar(100)


--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT
	dbo.GetFileRef(CL.clNo, F.fileNo) AS Ref  
	, CL.clName
	, REPLACE(F.fileDesc, char(13) + char(10), '', '') as fileDesc
	, A.cdDesc as Department
	, B.cdDesc as FileTypeDesc
	, U.usrInits AS FeeInits
	, F.fileLACategory
	, F.Created
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
LEFT JOIN
	dbo.GetCodeLookupDescription ( ''DEPT'' , ''{default}'' ) A  ON A.cdCode = F.fileDepartment
LEFT JOIN
	dbo.GetCodeLookupDescription ( ''FILETYPE'' , ''{default}'' ) B ON B.cdCode = F.FileType'

--- SET THE WHERE CLAUSE
SET @WHERE = ''

--- FILE TYPE CLAUSE
IF(@FILETYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' F.fileType = @FILETYPE '
END

--- FILE DEPARTMENT CLAUSE
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

--- FUNDING TYPE CLAUSE
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

--- ALLOW EXTERNAL CLAUSE
IF(@INTERACTIVE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileAllowExternal = @INTERACTIVE '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.fileAllowExternal = @INTERACTIVE '
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

--- STATUS CLAUSE
IF(@STATUS IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.FileStatus = @STATUS '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.FileStatus = @STATUS '
	END
END

--- CREATED START DATE AND END DATE
IF(@STARTDATE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND (F.Created >= @STARTDATE AND F.Created < @ENDDATE) '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' (F.Created >= @STARTDATE AND F.Created < @ENDDATE) '
	END
END

--- BUILD THE WHERE CLAUSE
IF @WHERE <> ''
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

--- BUILD THE ORDER BY CLAUSE
SET @ORDERBY = ' '

IF(@DATESORT = 'ASC')
BEGIN
	SET @ORDERBY = @ORDERBY + ' ORDER BY F.Created ASC '
END
ELSE
BEGIN
	SET @ORDERBY = @ORDERBY + ' ORDER BY F.Created DESC '
END

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

--- DEBUG PRINT
 PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @FILETYPE nvarchar(15)
	, @DEPARTMENT nvarchar(15)
	, @BRANCH int
	, @FUNDTYPE nvarchar(15)
	, @LACAT tinyint
	, @INTERACTIVE bit
	, @FEEEARNER int
	, @STARTDATE datetime
	, @ENDDATE datetime
	, @DATESORT nvarchar(15)
	, @STATUS nvarchar(15)'
	, @UI
	, @FILETYPE
	, @DEPARTMENT
	, @BRANCH
	, @FUNDTYPE
	, @LACAT
	, @INTERACTIVE
	, @FEEEARNER
	, @STARTDATE
	, @ENDDATE
	, @DATESORT
	, @STATUS

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilOpened] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilOpened] TO [OMSAdminRole]
    AS [dbo];

