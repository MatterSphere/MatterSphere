

CREATE PROCEDURE [dbo].[srepFileBillingGuide]
(@UI uUICultureInfo='{default}',
@ClNo nvarchar(50) = null,
@FileNo nvarchar(50) = null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'
SELECT     
	dbo.dbClient.clNo, 
	dbo.dbClient.clName, 
	dbo.dbFile.fileNo, 
	replace(dbo.dbFile.fileDesc, char(13) + char(10), '', '') AS fileDesc, 
	dbo.dbTimeLedger.timeUnits, 
	dbo.dbTimeLedger.timeDesc, 
	dbo.dbTimeLedger.timeCost, 
	dbo.dbTimeLedger.timeCharge, 
	dbo.dbTimeLedger.timeMins, 
	dbo.dbTimeLedger.timeBilled, 
    dbo.dbTimeLedger.timeRecorded, 
	dbo.dbFile.fileFundCode, 
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(dbFile.fileFundCode, '''') + ''~'') AS FundTypeDesc, 
    dbo.dbTimeLedger.timeActivityCode, 
	COALESCE(CL2.cdDesc, ''~'' + NULLIF(dbTimeLedger.timeActivityCode, '''') + ''~'') AS TimeActivityDesc, 
	dbo.dbFile.fileLACategory, 
	dbo.dbUser.usrInits, 
	dbo.dbUser.usrFullName, 
	dbUser_1.usrInits AS TimeFEInits
FROM    
	dbo.dbClient 
INNER JOIN
        dbo.dbFile ON dbo.dbClient.clID = dbo.dbFile.clID 
INNER JOIN
        dbo.dbTimeLedger ON dbo.dbFile.fileID = dbo.dbTimeLedger.fileID 
INNER JOIN
        dbo.dbUser ON dbo.dbFile.filePrincipleID = dbo.dbUser.usrID 
INNER JOIN
        dbo.dbUser dbUser_1 ON dbo.dbTimeLedger.feeusrID = dbUser_1.usrID
LEFT JOIN dbo.GetCodeLookupDescription ( ''FUNDTYPE'', @UI ) CL1 ON CL1.[cdCode] = dbFile.fileFundCode
LEFT JOIN dbo.GetCodeLookupDescription ( ''TIMEACTCODE'', @UI ) CL2 ON CL2.[cdCode] = dbTimeLedger.timeActivityCode'

---- Build Where Clause
SET @WHERE = ''

if coalesce(@clno, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbclient.clno = ''' + @clno + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbclient.clno = ''' + @clno + ''''
	END
END

if coalesce(@fileno, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfile.Fileno = ''' + @fileno + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfile.Fileno = ''' + @fileno + ''''
	END
END

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

---- Build OrderBy Clause
set @orderby = N' order by dbo.dbtimeledger.timerecorded'

declare @sql nvarchar(4000)
--- Add Statements together
set @sql = @select + @where + @orderby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileBillingGuide] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileBillingGuide] TO [OMSAdminRole]
    AS [dbo];

