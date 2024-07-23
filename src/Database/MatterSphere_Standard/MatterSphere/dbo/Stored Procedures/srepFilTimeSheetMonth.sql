

CREATE PROCEDURE [dbo].[srepFilTimeSheetMonth]
(
	@UI uUICultureInfo = '{default}'
	, @FEEEARNER int = NULL
	, @FEEDEPT uCodeLookup = NULL
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
	, @VALUNITS bit = 0
	, @CHARGEABLE bit = 0
)

AS 

DECLARE @SQL1 nVarChar(4000)
DECLARE @SQL2 nVarChar(4000)
DECLARE @ORDERBY nVarChar(200)

-- Start of the select, checking if the user has asked for units or values
SET @SQL1 = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED '
IF (@VALUNITS = 0)
BEGIN
	SET @SQL1 = Rtrim(@SQL1) + '
		SELECT 
			U.usrFullName,
			T.timeRecorded, 
			T.timeCharge AS numVar '
END
ELSE
BEGIN
	SET @SQL1 = Rtrim(@SQL1) + '
		SELECT 
			U.usrFullName,
			T.timeRecorded, 
			T.timeUnits AS numVar '
END
	 
-- now continue with the select
SET @SQL1 = Rtrim(@SQL1) + '
FROM 
	dbTimeLedger T
INNER JOIN
	dbActivities A ON A.actCode = T.timeActivityCode
INNER JOIN
	dbFeeEarner FE ON FE.feeUsrID = T.feeUsrID
INNER JOIN
	dbUSer U ON FE.feeUsrID = U.usrID
WHERE 
	(T.timerecorded >= @STARTDATE AND T.timeRecorded < @ENDDATE) '

-- FEEDEPT filter
IF (@FEEDEPT IS NOT NULL)
BEGIN
	SET @SQL1 = @SQL1 + ' AND FE.feeDepartment = @FEEDEPT '
END

--  FeeEarner filter
IF (@FEEEARNER IS NOT NULL)
BEGIN
	SET @SQL1 = @SQL1 + ' AND FE.feeUsrId = @FEEEARNER '
END

--  Chargeable filter
IF (@CHARGEABLE IS NOT NULL)
BEGIN
	SET @SQL1 = @SQL1 + ' AND A.actChargeable = @CHARGEABLE '
END

-- order by clause
SET @ORDERBY = N'
ORDER BY 
	U.usrFullName,
	T.timeRecorded '

-- combine into 1 query
SET @SQL2 = Rtrim(@SQL1) + Rtrim(@ORDERBY)

--print @sql2

EXEC sp_executesql @SQL2, 
N'
	@UI nvarchar(10)
	, @FEEEARNER int
	, @FEEDEPT nvarchar(50)
	, @STARTDATE datetime
	, @ENDDATE datetime
	, @VALUNITS bit
	, @CHARGEABLE bit'
	, @UI
	, @FEEEARNER
	, @FEEDEPT
	, @STARTDATE
	, @ENDDATE
	, @VALUNITS
	, @CHARGEABLE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilTimeSheetMonth] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilTimeSheetMonth] TO [OMSAdminRole]
    AS [dbo];

