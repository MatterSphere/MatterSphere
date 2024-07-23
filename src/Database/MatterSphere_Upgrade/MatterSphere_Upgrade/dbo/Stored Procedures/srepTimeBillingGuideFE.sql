CREATE PROCEDURE [dbo].[srepTimeBillingGuideFE]
(@UI uUICultureInfo='{default}',
@FileType nvarchar(50) = null,
@FileDepartment nvarchar(50) = null,
@FundingType nvarchar(50) = null,
@feePrincipleID int = null,
@fileStatus nvarchar(50) = null,
@WIP int = null)

AS 

declare @Select nvarchar(max)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)
declare @groupby nvarchar(2000)
declare @having nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbClient.clNo, dbo.dbFile.fileNo, dbo.dbClient.clName, replace(dbo.dbFile.fileDesc, char(13) + char(10), '', '') as fileDesc, dbo.dbFile.fileFundCode, COALESCE(CL1.cdDesc, ''~'' + NULLIF(dbo.dbFile.fileFundCode, '''') + ''~'') AS FundingType
					, dbo.dbFile.fileType, dbo.dbFile.fileWIP, dbo.dbUser.usrInits, dbo.dbFile.filePrincipleID, dbo.dbFile.fileDepartment, dbo.dbFile.fileStatus, 
                      SUM(dbo.dbTimeLedger.timeCharge) AS WIP
FROM         dbo.dbClient INNER JOIN
                      dbo.dbFile ON dbo.dbClient.clID = dbo.dbFile.clID INNER JOIN
                      dbo.dbUser ON dbo.dbFile.filePrincipleID = dbo.dbUser.usrID INNER JOIN
                      dbo.dbTimeLedger ON dbo.dbFile.fileID = dbo.dbTimeLedger.fileID AND dbo.dbtimeledger.timebilled = 0
			LEFT JOIN dbo.GetCodeLookupDescription ( ''FUNDTYPE'' , @UI ) CL1 ON CL1.cdCode =  dbo.dbFile.fileFundCode'

set @groupby = N' GROUP BY dbo.dbClient.clNo, dbo.dbFile.fileNo, dbo.dbClient.clName, dbo.dbFile.fileDesc, dbo.dbFile.fileFundCode, COALESCE(CL1.cdDesc, ''~'' + NULLIF(dbo.dbFile.fileFundCode, '''') + ''~'')
					, dbo.dbFile.fileType, dbo.dbFile.fileWIP, 
                      dbo.dbUser.usrInits, dbo.dbFile.filePrincipleID, dbo.dbFile.fileDepartment, dbo.dbFile.fileStatus'

---- Debug Print
PRINT @SELECT
print @groupby

---- Build Where Clause
SET @WHERE = ''
set @HAVING=''
if coalesce(@filetype, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfile.Filetype = ''' + @filetype + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfile.Filetype = ''' + @filetype + ''''
	END
END

if coalesce(@filedepartment, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfile.Filedepartment = ''' + @filedepartment + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfile.FileDepartment = ''' + @filedepartment + ''''
	END
END

if coalesce(@FundingType, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbfile.fileFundCode = ''' + @FundingType + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbfile.fileFundCode = ''' + @FundingType + ''''
	END
END  

if coalesce(@filestatus, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbfile.filestatus = ''' + @filestatus + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbfile.filestatus = ''' + @filestatus + ''''
	END
END  

if not @FeePrincipleID is null
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbfile.filePrincipleID = ' + convert(nvarchar, @FeePrincipleID)
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbfile.filePrincipleID = ' + convert(nvarchar, @FeePrincipleID)
	END
END

if not @WIP is null
BEGIN
   IF @having <> ''
	BEGIN
	    set @having = @having + ' AND sum(dbo.dbtimeledger.timecharge) >= ' + convert(nvarchar, @WIP)
	END
   ELSE
	BEGIN
	    set @having = @having + ' sum(dbo.dbtimeledger.timecharge) >= ' + convert(nvarchar, @WIP)
	END
END

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

If @having <> ''
BEGIN
	set @having = N' HAVING ' + @having
END



---- Build OrderBy Clause
set @orderby = N''

declare @sql nvarchar(4000)
--- Add Statements together
set @sql = @select + @where + @groupby + @having

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

