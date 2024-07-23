

CREATE PROCEDURE [dbo].[srepWIPWarning] 
(
	@STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
)

AS

DECLARE @SELECT nvarchar(2000)
DECLARE @WHERE nvarchar(1000)
DECLARE @SQL nvarchar(3000)

-- build the select statement
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT 
	CL.clNo + ''/'' + F.fileNo AS Ref
	, CL.clName
	, F.fileDesc
	, X.Wip AS fileWIP
	, F.fileCreditLimit
	, F.fileWarningPerc
	, F.Created 
FROM
	dbFile F 
INNER JOIN 
	dbClient CL on CL.clID = F.clID
LEFT OUTER JOIN
	(SELECT
		T.fileID,
		Sum(T.timeCharge) AS Wip
	 FROM
		dbTimeLedger T
	 WHERE
		T.timeBilled = 0
	 GROUP BY
		T.fileId
	 ) AS X ON X.fileID = F.fileID '

-- Build the where clause
SET @WHERE = N'
WHERE
	X.Wip > F.fileCreditLimit/100 * F.fileWarningPerc AND
	F.fileStatus = ''LIVE'' '

-- date filters
IF (@STARTDATE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND (F.Created >= @STARTDATE AND F.Created < @ENDDATE) '
END

-- build the whole query
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE)


exec sp_executesql @sql, N'
	@STARTDATE datetime
	, @ENDDATE datetime '
	, @STARTDATE
	, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepWIPWarning] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepWIPWarning] TO [OMSAdminRole]
    AS [dbo];

