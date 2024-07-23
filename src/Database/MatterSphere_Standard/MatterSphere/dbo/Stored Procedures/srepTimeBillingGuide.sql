

CREATE PROCEDURE [dbo].[srepTimeBillingGuide]
(
	@UI uUICultureInfo='{default}',
	@FILEID bigint = null,
	@TIMEACT nvarchar(15) = null,
	@STARTDATE datetime = null,
	@ENDDATE datetime = null,
	@VALUES bit = 0,
	@ITEMS bit = 0,
	@COST bit = 0,
	@TIMEDESC bit = 0,
	@UNBILLED bit = 0,
	@CHARGEABLE bit = 0
)

AS 

DECLARE @SELECT nvarchar(2500)
DECLARE @WHERE nvarchar(1250)
DECLARE @ORDERBY nvarchar(250)

--- SET THE SELECT CLAUSE
SET @SELECT = '
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT     
	CL.clNo
	, CL.clName
	, F.fileNo
	, REPLACE(F.fileDesc, char(13) + char(10), '', '') as [File Description]
	, T.timeUnits
	, T.timeDesc
    , T.timeCost
	, T.timeCharge
	, T.timeMins
	, T.timeRecorded
	, X.cdDesc AS [Fund Type Description]
	, T.timeActivityCode 
	, Y.cdDesc AS [Time Activity Description]
    , U.usrInits
	, U.usrFullName
	, @VALUES AS [Value]
	, @ITEMS AS [Items]
	, @COST AS [Cost]
	, @TIMEDESC AS [Show Time Description]
	, A.actChargeable
	, T.timeBilled
FROM    
	dbo.dbClient CL
INNER JOIN
    dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
    dbo.dbTimeLedger T ON F.fileID = T.fileID 
INNER JOIN
    dbo.dbUser U ON F.filePrincipleID = U.usrID
LEFT OUTER JOIN
	dbo.dbActivities A ON A.actCode = T.timeActivityCode
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''FUNDTYPE'', @UI) AS X ON X.cdCode = F.fileFundCode
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''TIMEACTCODE'', @UI) AS Y ON Y.cdCode = T.timeActivityCode '

--- SET THE WHERE CLAUSE
SET @WHERE = ' WHERE F.fileID = @FILEID '

--- TIME ACTIVITY CODE CLAUSE
IF(@TIMEACT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND T.timeActivityCode = @TIMEACT '
END

--- TIME RECORDED START DATE AND END DATE
IF(@STARTDATE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND (T.timeRecorded >= @STARTDATE AND T.timeRecorded < @ENDDATE) '
END

--- TIME BILLED CLAUSE
IF(@UNBILLED IS NOT NULL AND @UNBILLED = 1)
BEGIN
	SET @WHERE = @WHERE + ' AND T.timeBilled = 0 ' 
END

--- TIME CHARGEABLE CLAUSE
IF(@CHARGEABLE IS NOT NULL AND @CHARGEABLE = 1)
BEGIN
	SET @WHERE = @WHERE + ' AND A.actChargeable = 1 '
END

--- Set the Order By Statement
SET @ORDERBY = ' ORDER BY T.timeRecorded '

DECLARE @SQL nvarchar(4000)
--- ADD CLAUSES TOGETHER
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

-- DEBUG PRINT
-- PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @FILEID bigint
	, @TIMEACT nvarchar(15)
	, @STARTDATE datetime
	, @ENDDATE datetime
	, @VALUES bit
	, @ITEMS bit
	, @COST bit
	, @TIMEDESC bit
	, @UNBILLED bit
	, @CHARGEABLE bit'
	, @UI
	, @FILEID
	, @TIMEACT
	, @STARTDATE
	, @ENDDATE
	, @VALUES
	, @ITEMS
	, @COST
	, @TIMEDESC
	, @UNBILLED
	, @CHARGEABLE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepTimeBillingGuide] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepTimeBillingGuide] TO [OMSAdminRole]
    AS [dbo];

