CREATE PROCEDURE [dbo].[srepTimeSheetFeeEarner]  

(
	@UI uUICultureInfo = '{default}'
	, @FEEEARNER int = NULL
	, @STARTDATE datetime 
	, @ENDDATE datetime 
	, @BYFEEEARNER bit = 1
--	, @DEBUG bit = 1
)

AS

DECLARE @SELECT nvarchar(1900)
DECLARE @WHERE nvarchar(2000)
DECLARE @ORDERBY nvarchar(100) 

-- No longer required when using UTC date format.
--- initialise Date Parameters as per RH recommendations on date performance issues
-- Date filters are mandatory
--IF (@STARTDATE IS NOT NULL)
--BEGIN
-- SET @STARTDATE = dbo.GetStartDate(@STARTDATE)
--END
--IF (@ENDDATE IS NOT NULL)
--BEGIN
-- SET @ENDDATE = dbo.GetEndDate(@ENDDATE)
--END

--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT     
	U.usrFullName, 
	T.timeRecorded, 
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(T.timeActivityCode, '''') + ''~'') AS ActivityDesc, 
	CL.clNo, 
	CL.clName, 
	T.timeDesc, 
	T.timeUnits, 
	T.timeMins, 
	T.timeCharge, 
	F.fileNo
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbTimeLedger T ON F.fileID = T.fileID
INNER JOIN
	dbo.dbUser U ON U.usrID = T.feeusrid
LEFT JOIN 
	dbo.GetCodeLookupDescription ( ''TIMEACTCODE'' , @UI ) CL1 ON CL1.cdCode = T.timeActivityCode '

---- SET THE WHERE STATEMENT
SET @WHERE = N' WHERE T.timeRecorded >= @STARTDATE AND T.timeRecorded < @ENDDATE '

--- FEE EARNER CLAUSE
--- FEE EARNER / FEE EARNER MATTERS
---  @BYFEEEARNER = 1 : User ticked By Fee Earner Checkbox
---  @BYFEEEADNER = 0 : User ticked On Fee Earners Matters
IF(@FEEEARNER IS NOT NULL)
BEGIN
	IF (@BYFEEEARNER = 1)
	BEGIN
		SET @WHERE = @WHERE + ' AND T.feeUsrId = @FEEEARNER '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' AND F.filePrincipleID = @FEEEARNER '
	END
END


---- SET THE ORDER BY STATEMENT
set @orderby = N' 
ORDER BY 
	T.timeRecorded
	, U.usrFullName '

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

--- DEBUG PRINT
-- PRINT @SQL

--if @debug = 1  print @sql

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @FEEEARNER int
	, @STARTDATE datetime
	, @ENDDATE datetime
	, @BYFEEEARNER bit '
	, @UI
	, @FEEEARNER
	, @STARTDATE
	, @ENDDATE
	, @BYFEEEARNER
