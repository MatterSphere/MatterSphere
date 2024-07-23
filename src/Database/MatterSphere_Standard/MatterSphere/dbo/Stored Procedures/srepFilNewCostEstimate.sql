

CREATE PROCEDURE [dbo].[srepFilNewCostEstimate]
(
	@UI uUICultureInfo='{default}'
	, @FILETYPE uCodeLookup = NULL
	, @FEEEARNER bigint = NULL
	, @DEPARTMENT uCodeLookup = NULL
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
)

AS 

DECLARE @SELECT nvarchar(2000)
DECLARE @WHERE nvarchar(1800)
DECLARE @ORDERBY nvarchar(200)

--- BUILD THE SELECT CLAUSE
SET @SELECT = ' 
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	dbo.GetFileRef(CL.clNo, F.fileNo) AS [Ref]
	, CL.clName
	, F.fileDesc
	, F.fileEstimate
	, F.Created
FROM
	dbFile F
INNER JOIN
	dbClient CL ON CL.clID = F.clID '

--- BUILD THE WHERE CLAUSE
SET @WHERE = ' WHERE F.fileEstimate > 0 '

-- filetype filter
IF (@FILETYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.fileType = @FILETYPE '
END

-- Fee Earner filter
IF (@FEEEARNER IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.filePrincipleID = @FEEEARNER '
END

-- Department filter
IF (@DEPARTMENT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.fileDepartment = @DEPARTMENT '
END

-- Startdate filter
IF(@STARTDATE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND (F.Created >= @STARTDATE AND F.Created < @ENDDATE) '
END

--- BUILD THE ORDER BY CLAUSE
SET @ORDERBY = ' ORDER BY F.Created DESC '

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

-- DEBUG PRINT
-- PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI uUICultureInfo
	, @FILETYPE uCodeLookup
	, @FEEEARNER bigint
	, @DEPARTMENT uCodeLookup
	, @STARTDATE datetime
	, @ENDDATE datetime'
	, @UI
	, @FILETYPE
	, @FEEEARNER
	, @DEPARTMENT
	, @STARTDATE
	, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilNewCostEstimate] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilNewCostEstimate] TO [OMSAdminRole]
    AS [dbo];

