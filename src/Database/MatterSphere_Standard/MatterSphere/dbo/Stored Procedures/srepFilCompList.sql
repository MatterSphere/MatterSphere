

CREATE PROCEDURE [dbo].[srepFilCompList]
(
	@UI uUICultureInfo='{default}'
	, @FILETYPE nvarchar(15) = NULL
	, @FEEEARNER int = NULL
	, @DEPARTMENT nvarchar(15) = NULL
	, @FUNDTYPE nvarchar(15) = NULL
	, @BRANCH int = NULL
	, @STATUS nvarchar(15) = NULL
	, @LACAT nvarchar(15) = NULL
	, @INTERACTIVE bit = NULL
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
	, @SUMMARY bit = 0
	, @CLOSEDDATE bit = 0
	, @GRPCHECK nvarchar(15) = NULL
)

AS 

DECLARE @SELECT nvarchar(2250)
DECLARE @WHERE nvarchar(1500)
DECLARE @ORDERBY nvarchar(250)

--- SET THE SELECT CLAUSE
SET @SELECT = ' 
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT
	CASE
		WHEN @SUMMARY = 0 THEN 0
		ELSE 1
	END AS Summary
	, CASE
		WHEN F.fileAllowExternal = 1 THEN 1
		ELSE NULL
	  END AS Interactive
	, CASE
		WHEN F.fileAllowExternal = 0 THEN 1
		ELSE NULL
	  END AS notInteractive
	, F.fileAllowExternal
	, F.fileLACategory
	, U.usrInits AS FeeInits
	, U.usrFullName AS FeeName
	, CL.clNo + ''/'' + F.fileNo AS OurRef
	, CL.clName
	, Replace(F.fileDesc, char(13) + char(10), '', '') AS fileDesc
	, X.cdDesc AS fileTypeDesc
	, Y.cdDesc AS DeptDesc
	, F.Created
	, CASE
		WHEN @GRPCHECK = 1 THEN ''Group By Fee Earner : '' + CONVERT(nvarchar(30), U.usrFullName)
		WHEN @GRPCHECK = 2 THEN ''Group By Department : '' + Y.cdDesc
		WHEN @GRPCHECK = 3 THEN ''Group By File Type : '' + X.cdDesc
		WHEN @GRPCHECK = 4 THEN ''Group By Legal Aid Category : '' + Z.cdDesc
	  END AS GroupBy
	, @GRPCHECK AS grpCheck
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''FILETYPE'', @UI) AS X ON X.cdCode = F.fileType
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''LACAT'', @UI) AS Z ON Z.cdCode = F.fileLACategory
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''DEPT'', @UI) AS Y ON Y.cdCode = F.fileDepartment '

--- SET THE WHERE CLAUSE
SET @WHERE = ''

--- FILE TYPE CLAUSE
IF(@FILETYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' F.fileType = @FILETYPE '
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

--- INTERACTIVE CLAUSE
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

--- STARTDATE AND ENDDATE CLAUSE
IF(@CLOSEDDATE = 0)
BEGIN
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
END
ELSE
BEGIN
	IF(@STARTDATE IS NOT NULL)
	BEGIN
		IF(@WHERE <> '')
		BEGIN
			SET @WHERE = @WHERE + ' AND (F.fileClosed >= @STARTDATE AND F.fileClosed < @ENDDATE) '
		END
		ELSE
		BEGIN
			SET @WHERE = @WHERE + ' (F.fileClosed >= @STARTDATE AND F.fileClosed < @ENDDATE) '
		END
	END
END

--- BUILD THE WHERE CLAUSE
IF @WHERE <> ''
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

--- SET THE ORDER BY CLAUSE
SET @ORDERBY = ' 
ORDER BY
	F.Created DESC '

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

--- DEBUG PRINT
-- PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI uUICultureInfo
	, @FILETYPE nvarchar(15)
	, @FEEEARNER int
	, @DEPARTMENT nvarchar(15)
	, @FUNDTYPE nvarchar(15)
	, @BRANCH int
	, @STATUS nvarchar(15)
	, @LACAT nvarchar(15)
	, @INTERACTIVE bit
	, @STARTDATE datetime
	, @ENDDATE datetime
	, @SUMMARY bit
	, @CLOSEDDATE bit
	, @GRPCHECK nvarchar(15)'
	, @UI
	, @FILETYPE
	, @FEEEARNER
	, @DEPARTMENT
	, @FUNDTYPE
	, @BRANCH
	, @STATUS
	, @LACAT
	, @INTERACTIVE
	, @STARTDATE
	, @ENDDATE
	, @SUMMARY
	, @CLOSEDDATE
	, @GRPCHECK

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilCompList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilCompList] TO [OMSAdminRole]
    AS [dbo];

