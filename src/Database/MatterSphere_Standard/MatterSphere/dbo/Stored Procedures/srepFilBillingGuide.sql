

CREATE PROCEDURE [dbo].[srepFilBillingGuide]
(
	@UI uUICultureInfo='{default}'
	, @CLNO nvarchar(10) 
	, @FILENO nvarchar(20)
	, @ACTIVITY nvarchar(50) = NULL
	, @FEEEARNER nvarchar(50) = NULL
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
)

AS 

DECLARE @SELECT nVarChar(3000)
DECLARE @WHERE nvarchar(1500)
DECLARE @SQL nVarChar(4000)
DECLARE @ORDERBY nvarchar(100)

-- Start of the select, checking if the user has asked for units or values
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT     
	CL.clNo, 
	CL.clName, 
	F.fileNo, 
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	T.timeUnits, 
	T.timeDesc, 
    T.timeCost, 
	T.timeCharge, 
	T.timeMins, 
	T.timeRecorded, 
	F.fileFundCode,
	X.cdDesc AS FundTypeDesc,
	T.timeActivityCode, 
	Y.cdDesc AS TimeActivityDesc,
	F.fileLACategory, 
    U.usrInits, 
	U.usrFullName
FROM    
	dbo.dbClient CL
INNER JOIN
    dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
    dbo.dbTimeLedger T ON F.fileID = T.fileID 
INNER JOIN
    dbo.dbUser U ON F.filePrincipleID = U.usrID
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''FUNDTYPE'', @UI) AS X ON X.cdCode = F.fileFundCode
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''TIMEACTCODE'', @UI) AS Y ON Y.cdCode = T.timeActivityCode '

-- where clause , note clno and fileno are mandatory filters
SET @WHERE = N' WHERE CL.clNo = @CLNO AND F.fileNo = @FILENO '

-- time activity filter
IF (@ACTIVITY IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND T.timeActivityCode = @ACTIVITY '
END
 
-- Fee Earner filter
IF (@FEEEARNER IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND U.usrID = @FEEEARNER '
END

-- Date range
IF (@STARTDATE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND (T.timeRecorded >= @STARTDATE AND T.timeRecorded < @ENDDATE) '
END

-- Order by clause
SET @ORDERBY = N' ORDER BY T.timeRecorded DESC '

-- build the query
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE)+ RTrim(@ORDERBY)

exec sp_executesql @sql,
 N'
	@UI nvarchar(10),
	@CLNO nvarchar(10),
	@FILENO nvarchar(20),
	@ACTIVITY nvarchar(50),
	@FEEEARNER nvarchar(50),
	@STARTDATE datetime,
	@ENDDATE datetime',
	@UI,
	@CLNO,
	@FILENO,
	@ACTIVITY,
	@FEEEARNER,
	@STARTDATE,
	@ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilBillingGuide] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilBillingGuide] TO [OMSAdminRole]
    AS [dbo];

