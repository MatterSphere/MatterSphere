

CREATE PROCEDURE [dbo].[srepOpenedCrimeUFNs] 
(
	@STARTDATE datetime = null
	, @ENDDATE datetime = null
)

AS

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

DECLARE @SQL nvarchar(3000)
DECLARE @SELECT nvarchar(2500)
DECLARE @WHERE nvarchar(1400)
DECLARE @ORDERBY nvarchar(100)

-- Build the select clause
SET @SELECT = N'
SELECT
	U.ufnHeadCode as UFNID
	, CL.clName
	, CL.clNo + ''/'' + f.FileNO as Ref
	, F.fileDesc
	, X.cdDesc AS fileDepartment
	, F.Created
	, FT.fileDefFundCode
	, B.brName 
FROM
	dbFile F 
INNER JOIN
	dbFileType FT on FT.typeCode = F.fileType
INNER JOIN
	dbClient CL on CL.clID = F.clID
INNER JOIN
	dbBranch B on B.brID = F.brID
INNER JOIN
	dbFileLegal FL on FL.fileId = F.fileID
INNER JOIN
	dbUFN U on U.ufnCode = FL.MatLAUFN
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''DEPT'', ''{default}'') AS X ON X.cdCode = f.fileDepartment '

-- Build the where clause
SET @WHERE = N' WHERE F.fileStatus = ''LIVE'' '

-- date filer
IF (@STARTDATE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND (F.Created >= @STARTDATE AND F.Created < @ENDDATE) '
END 

-- ORder by clause
SET @ORDERBY = N' ORDER BY U.UFNID '

-- merge the lot together
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)


EXEC sp_executesql @SQL, 
N'
	@STARTDATE datetime
	, @ENDDATE datetime '
	, @STARTDATE
	, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepOpenedCrimeUFNs] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepOpenedCrimeUFNs] TO [OMSAdminRole]
    AS [dbo];

