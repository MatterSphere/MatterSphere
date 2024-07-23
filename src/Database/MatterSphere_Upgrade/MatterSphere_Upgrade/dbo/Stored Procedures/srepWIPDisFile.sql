CREATE PROCEDURE [dbo].[srepWIPDisFile]
(@UI uUICultureInfo='{default}',
@FileType nvarchar(50) = null,
@FileDepartment nvarchar(50) = null,
@brID nvarchar(50)= null,
@FundingType nvarchar(50) = null,
@LACategory nvarchar(50) = null,
@feePrincipleID int = null,
@MatterStatus nvarchar (50) = null)
AS 

declare @Select nvarchar(max)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbClient.clNo, dbo.dbClient.clName, dbo.dbFile.fileNo, replace(dbo.dbFile.fileDesc, char(13) + char(10), '', '') as fileDesc, dbo.dbFile.fileFundCode, dbo.dbFile.fileCreditLimit, 
                      dbo.dbFile.fileRatePerUnit, dbo.dbTimeLedger.timeUnits, dbo.dbTimeLedger.timeMins, dbo.dbFile.fileWIP, dbo.dbUser.usrInits, 
                      dbo.dbFile.fileDepartment, dbo.dbFile.fileType, COALESCE(CL1.cdDesc, ''~'' + NULLIF(dbo.dbFile.fileFundCode, '''') + ''~'') AS FundingType, 
                      dbo.dbFile.fileLACategory, dbo.dbFile.brID, dbo.dbFile.fileStatus, dbo.dbFile.filePrincipleID, 
                      dbo.dbTimeLedger.timeCost, dbo.dbTimeLedger.timeRecorded
FROM         dbo.dbFile INNER JOIN
                      dbo.dbClient ON dbo.dbFile.clID = dbo.dbClient.clID INNER JOIN
                      dbo.dbUser ON dbo.dbClient.clSourceUser = dbo.dbUser.usrID INNER JOIN
                      dbo.dbTimeLedger ON dbo.dbFile.fileID = dbo.dbTimeLedger.fileID
			LEFT JOIN dbo.GetCodeLookupDescription ( ''FUNDTYPE'' , @UI ) CL1 ON CL1.cdCode = dbo.dbFile.fileFundCode'

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''

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

if coalesce(@brID, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbfile.brID = ' + @brID 
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbfile.brID = ' + @brID 
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

if coalesce(@LAcategory, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbfile.fileLACategory = ''' + @lacategory + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbfile.fileLACategory = ''' + @lacategory + ''''
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

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

---- Build OrderBy Clause
set @orderby = N''

declare @sql nvarchar(4000)
--- Add Statements together
set @sql = @select + @where + @orderby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

