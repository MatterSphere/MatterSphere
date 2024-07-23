

CREATE PROCEDURE [dbo].[srepFilAnalysis]
(
	@UI uUICultureInfo='{default}',
	@FILETYPE nvarchar(50) = null,
	@DEPARTMENT nvarchar(50) = null,
	@BRANCH nvarchar(50)= null,
	@FUNDTYPE nvarchar(50) = null,
	@LACAT nvarchar(50) = null,
	@FEEEARNER int = null,
	@FILESTATUS nvarchar(50) = null,
	@STARTDATE datetime=null,
	@ENDDATE datetime=null
)

AS 

DECLARE @Sql nvarchar(4000)
DECLARE @Select nvarchar(2000)
DECLARE @Where nvarchar(2000)
DECLARE @GroupBy nvarchar(100)

--- Select Statement for the base Query
SET @Select = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT DISTINCT    
	U.usrFullName
	, COUNT(F.fileNo) As NumFiles
FROM         
	dbo.dbFile F 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID'


---- Build Where Clause
SET @Where = ''

--- STARTDATE/ENDDATE Clause
IF(@STARTDATE IS NOT NULL)
BEGIN
	SET @Where = @Where + ' (F.Created >= @STARTDATE AND F.Created < @ENDDATE) '
END

--- FILETYPE Clause
IF(@FILETYPE IS NOT NULL)
BEGIN
	IF @Where <> '' 
	BEGIN
		SET @Where = @Where + ' AND F.Filetype = @FILETYPE '
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' F.Filetype = @FILETYPE '
	END
END

--- DEPARTMENT Clause
IF(@DEPARTMENT IS NOT NULL)
BEGIN
	IF @Where <> '' 
	BEGIN
	     set @Where = @Where + ' AND F.FileDepartment = @DEPARTMENT '
	END
	ELSE
	BEGIN
	     set @Where = @Where + ' F.FileDepartment = @DEPARTMENT '
	END
END

--- BRANCH Clause
IF(@BRANCH IS NOT NULL)
BEGIN
	IF @Where <> '' 
	BEGIN
	     set @Where = @Where + ' AND F.brID = @BRANCH '
	END
	ELSE
	BEGIN
	     set @Where = @Where + ' F.brID = @BRANCH '
	END
END

--- FUNDTYPE Clause
IF(@FUNDTYPE IS NOT NULL)
BEGIN
	IF @Where <> '' 
	BEGIN
	     set @Where = @Where + ' AND F.fileFundCode = @FUNDTYPE '
	END
	ELSE
	BEGIN
	     set @Where = @Where + ' F.fileFundCode = @FUNDTYPE '
	END
END

--- FILESTATUS Clause
IF(@FILESTATUS IS NOT NULL)
BEGIN
	IF @Where <> '' 
	BEGIN
	     set @Where = @Where + ' AND F.fileStatus = @FILESTATUS '
	END
	ELSE
	BEGIN
	     set @Where = @Where + ' F.fileStatus = @FILESTATUS '
	END
END

--- LACAT  (Legal Aid Category) Clause
IF(@LACAT IS NOT NULL)
BEGIN
	IF @Where <> '' 
	BEGIN
	     set @Where = @Where + ' AND F.fileLACategory = @LACAT '
	END
	ELSE
	BEGIN
	     set @Where = @Where + ' F.fileLACategory = @LACAT '
	END
END

--- FEEEARNER Clause
IF(@FEEEARNER IS NOT NULL)
BEGIN
	IF @Where <> '' 
	BEGIN
	     set @Where = @Where + ' AND F.filePrincipleID = @FEEEARNER '
	END
	ELSE
	BEGIN
	     set @Where = @Where + ' F.filePrincipleID = @FEEEARNER '
	END
END

--- Complete Where clause
IF(@WHERE <> '')
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

--- Build the Group By clause
SET @GroupBy = N' GROUP BY U.usrFullName '

--- Build the command
SET @Sql = Rtrim(@Select) + Rtrim(@Where) + Rtrim(@GroupBy)

--- Debug Print
-- PRINT @Sql

exec sp_executesql @Sql,
N'
	@UI nvarchar(10)
	, @FILETYPE nvarchar(50)
	, @DEPARTMENT nvarchar(50)
	, @BRANCH nvarchar(50)
	, @FUNDTYPE nvarchar(50)
	, @LACAT nvarchar(50)
	, @FEEEARNER int
	, @FILESTATUS nvarchar(50)
	, @STARTDATE datetime
	, @ENDDATE datetime'
	, @UI
	, @FILETYPE
	, @DEPARTMENT
	, @BRANCH
	, @FUNDTYPE
	, @LACAT
	, @FEEEARNER
	, @FILESTATUS
	, @STARTDATE
	, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilAnalysis] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilAnalysis] TO [OMSAdminRole]
    AS [dbo];

