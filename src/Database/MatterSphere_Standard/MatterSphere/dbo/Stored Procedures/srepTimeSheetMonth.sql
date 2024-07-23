

CREATE PROCEDURE [dbo].[srepTimeSheetMonth]
(
	@UI uUICultureInfo='{default}',
	@MONTH int = null,
	@DAY int = null,
	@FEEEARNER nvarchar(50) = null,
	@BILLED nvarchar(50) = null
)

AS 

DECLARE @SQL nvarchar(4000)

--- Select Statement for the base Query
SET @SQL = N'
SELECT     
	U.usrInits AS FeeInits, 
	T.timeActivityCode, 
	dbo.GetCodeLookupDesc(N''TIMEACTCODE'', T.timeActivityCode, @UI) AS ActivityDesc, 
	DATEPART(yyyy, T.timerecorded) AS Year, 
	DATEPART(m, T.timeRecorded) AS Month, 
	DATEPART(d, T.timeRecorded) AS Day, 
        T.timeBilled, 
	T.timeUnits
FROM         
	dbo.dbTimeLedger T
INNER JOIN
	dbo.dbUser U ON T.feeusrID = U.usrID
WHERE
	COALESCE(T.timeBilled, '''') = COALESCE(@BILLED, T.timeBilled, '''') AND
	COALESCE(U.usrID, '''') = COALESCE(@FEEEARNER, U.usrID, '''') AND
	MONTH(T.timeRecorded) = COALESCE(@MONTH, MONTH(T.timeRecorded, '''') AND
	DAY(T.timeRecorded) = COALESCE(@DAY, DAY(T.timeRecorded), '''')'
/*
---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''
SET @HAVING= ''

if coalesce(@chargeable, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbtimeledger.timebilled = ''' + @chargeable + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbtimeledger.timebilled = ''' + @chargeable + ''''
	END
END  

if coalesce(@feeearner, '') <> ''
BEGIN
IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbuser.usrinits = ''' + @feeearner + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbuser.usrinits = ''' + @feeearner + ''''
	END
END

if coalesce(@Month, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND DATEPART(m, dbo.dbTimeLedger.timeRecorded) = ''' + @Month + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' DATEPART(m, dbo.dbTimeLedger.timeRecorded) = ''' + @Month + ''''
	END
END

if coalesce(@Day, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND DATEPART(d, dbo.dbTimeLedger.timeRecorded) = ''' + @Day + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' DATEPART(d, dbo.dbTimeLedger.timeRecorded) = ''' + @Day + ''''
	END
END 

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

---- Build OrderBy Clause
set @orderby = N''

declare @sql nvarchar(4000)
--- Add Statements together
set @sql = @select + @where
*/

--- Debug Print
PRINT @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @MONTH int, @DAY int, @FEEEARNER nvarchar, @BILLED nvarchar', @UI, @MONTH, @DAY, @FEEEARNER, @BILLED

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepTimeSheetMonth] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepTimeSheetMonth] TO [OMSAdminRole]
    AS [dbo];

