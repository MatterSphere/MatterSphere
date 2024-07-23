

CREATE PROCEDURE [dbo].[srepTimeBillingGuideFiles]
(@UI uUICultureInfo='{default}',
@ClNo nvarchar(12) = null,
@FileNo nvarchar(20) = null,
@Activity nvarchar(15) = null,
@FeeEarner bigint = null,
@begindate datetime=null,
@enddate datetime=null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'
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
    COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileFundCode, '''') + ''~'') AS FundTypeDesc, 
	T.timeActivityCode, 
	COALESCE(CL2.cdDesc, ''~'' + NULLIF(T.timeActivityCode, '''') + ''~'') AS TimeActivityDesc, 
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
LEFT JOIN dbo.GetCodeLookupDescription ( ''FUNDTYPE'' , @UI ) CL1 
	ON CL1.cdCode = F.fileFundCode
LEFT JOIN dbo.GetCodeLookupDescription ( ''TIMEACTCODE'' , @UI ) CL2 
	ON CL2.cdCode = T.timeActivityCode'

--- Build Where Clause
SET @WHERE = ''

if coalesce(@clno, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND CL.clno = @CLNO '
	END
   ELSE
	BEGIN
	     set @where = @where + ' CL.clno = @CLNO '
	END
END

if coalesce(@fileno, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND F.Fileno = @FILENO '
	END
   ELSE
	BEGIN
	     set @where = @where + ' F.Fileno = @FILENO '
	END
END

if coalesce(@Activity, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND T.timeactivitycode = @ACTIVITY '
	END
   ELSE
	BEGIN
	     set @where = @where + ' T.timeactivitycode = @ACTIVITY '
	END
END

if coalesce(@feeearner, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND U.usrid = @FEEEARNER '
	END
   ELSE
	BEGIN
	     set @where = @where + ' U.usrid = @FEEEARNER '
	END
END

if coalesce(@BEGINDATE, '') <> ''
BEGIN
    IF @where = '' or @where is null
	BEGIN
	    if @enddate is null
		BEGIN
		    set @where = @where + ' T.timerecorded BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' T.timerecorded BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
   ELSE
	BEGIN
	    if @ENDDATE is null
		BEGIN
		    set @where = @where + ' AND T.timerecorded BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' AND T.timerecorded BETWEEN ''' + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
END  


--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

--- Build OrderBy Clause
set @orderby = N' ORDER BY T.timerecorded'

declare @sql nvarchar(4000)

--- Add Statements together
set @sql = @select + @where + @orderby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10), @CLNO nvarchar(12), @FILENO nvarchar(20), @ACTIVITY nvarchar(15), @FEEEARNER bigint', @UI, @CLNO, @FILENO, @ACTIVITY, @FEEEARNER

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepTimeBillingGuideFiles] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepTimeBillingGuideFiles] TO [OMSAdminRole]
    AS [dbo];

