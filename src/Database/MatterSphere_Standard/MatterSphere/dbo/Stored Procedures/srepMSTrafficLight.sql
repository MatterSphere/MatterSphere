

CREATE PROCEDURE [dbo].[srepMSTrafficLight]
(@UI uUICultureInfo='{default}',
@FileType nvarchar(50) = null,
@FileDepartment nvarchar(50) = null,
@Source nvarchar(50)= null,
@FileStat nvarchar(50) = null,
@MSplan nvarchar(50) = null,
@FundingType nvarchar(50) = null,
@feePrincipleID int = null,
@begindate datetime=null,
@enddate datetime=null)
AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbUser.usrInits, dbo.dbUser.usrFullName, dbo.dbClient.clNo, dbo.dbClient.clName, dbo.dbFile.fileNo, replace(dbo.dbFile.fileDesc, char(13) + char(10), '', '') as fileDesc, dbo.dbClient.clSource, 
                      dbo.dbMSData_OMS2K.MSNextDueDate, dbo.dbMSData_OMS2K.MSNextDueStage, dbo.dbMSData_OMS2K.MSCode, dbo.dbFile.Created, 
                      dbo.dbFile.filePrincipleID, dbo.dbFile.fileDepartment, dbo.dbFile.fileType, dbo.dbFile.fileFundCode, dbo.dbFile.fileStatus, dbo.dbfile.filenotes
FROM         dbo.dbClient INNER JOIN
                      dbo.dbFile ON dbo.dbClient.clID = dbo.dbFile.clID INNER JOIN
                      dbo.dbMSData_OMS2K ON dbo.dbFile.fileID = dbo.dbMSData_OMS2K.fileID INNER JOIN
                      dbo.dbUser ON dbo.dbFile.filePrincipleID = dbo.dbUser.usrID'

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

if coalesce(@source, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbclient.clsource = ''' + @source + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbclient.clsource = ''' + @source + '''' 
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

if coalesce(@filestat, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbfile.filestatus = ''' + @filestat + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbfile.filestatus = ''' + @filestat + ''''
	END
END 

if coalesce(@MSplan, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbmsdata_oms2k.mscode = ''' + @msplan + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbmsdata_oms2k.mscode = ''' + @msplan + ''''
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

print @BEGINDATE
Print @ENDDATE

if coalesce(@BEGINDATE, '') <> ''
BEGIN
    IF @where = '' or @where is null
	BEGIN
	    if @enddate is null
		BEGIN
		    set @where = @where + ' dbo.dbfile.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' dbo.dbfile.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
   ELSE
	BEGIN
	    if @ENDDATE is null
		BEGIN
		    set @where = @where + ' AND dbo.dbfile.Created BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' AND dbo.dbfile.Created BETWEEN ''' + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
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

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepMSTrafficLight] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepMSTrafficLight] TO [OMSAdminRole]
    AS [dbo];

