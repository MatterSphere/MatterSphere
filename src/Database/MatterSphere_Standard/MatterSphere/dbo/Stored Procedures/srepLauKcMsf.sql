

CREATE PROCEDURE [dbo].[srepLauKcMsf]
(
	@CONTRACT nvarchar(50) 
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
)

AS 

DECLARE @SQL1 nVarChar(2000)
DECLARE @SQL2 nVarChar(4000)
DECLARE @ORDERBY nVarChar(200)

-- Start of the select, checking if the user has asked for units or values
SET @SQL1 = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	C.cdDesc,
	(SELECT
		COUNT(F.fileID)
	 FROM
		dbFile F
	 INNER JOIN
		dbFileLegal FL on F.fileID = FL.fileID
	 WHERE
		FL.matLAContract = @CONTRACT AND
		FL.matLAMatType = C.cdCode '
	-- Date filters
	if (@STARTDATE IS NOT NULL)
	BEGIN
		SET @SQL1 = @SQL1 + ' AND (F.Created >= @STARTDATE AND F.Created < @ENDDATE) '
	END
	-- continue
	SET @SQL1 = @SQL1 +
	' ) AS NewMatStart,
	(SELECT
		COUNT(F.fileID)
	 FROM
		dbFile F
	 INNER JOIN
		dbFileLegal FL on F.fileID = FL.fileID
	 WHERE
		matLAMatType = cdCode AND
		matLAContract = @CONTRACT '
	-- Date filters
	if (@STARTDATE IS NOT NULL)
	BEGIN
		SET @SQL1 = @SQL1 + ' AND (F.fileClosed >= @STARTDATE AND F.fileClosed < @ENDDATE) '
	END
	-- continue
	SET @SQL1 = @SQL1 +
	' ) AS Closed,
	RI.regCompanyName,
	LAC.LAContractRef
FROM 
	dbCodeLookup C
CROSS JOIN
	dbRegInfo RI
CROSS JOIN
	dbLegalAidContract LAC
WHERE
	C.cdType = ''GFGROUP'' AND
	LAC.LAContractCode = @CONTRACT '

-- order by clause
SET @ORDERBY = N' ORDER BY C.cdaddlink '

-- combine into 1 query
SET @SQL2 = Rtrim(@SQL1) + Rtrim(@ORDERBY)

-- print @sql2

EXEC sp_executesql @SQL2, 
N'
	@CONTRACT nvarchar(50)
	, @STARTDATE datetime
	, @ENDDATE datetime'
	, @CONTRACT
	, @STARTDATE
	, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepLauKcMsf] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepLauKcMsf] TO [OMSAdminRole]
    AS [dbo];

