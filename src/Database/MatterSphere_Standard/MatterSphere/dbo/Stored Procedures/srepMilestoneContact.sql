

CREATE PROCEDURE [dbo].[srepMilestoneContact]
(@UI uUICultureInfo='{default}',
@msplan nvarchar(50) = null,
@FileType nvarchar(50) = null,
@FileDepartment nvarchar(50) = null,
@FileStat nvarchar(50) = null,
@FundingType nvarchar(50) = null,
@feePrincipleID int = null)
AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbUser.usrInits AS FeeInits, dbo.dbUser.usrFullName, dbo.dbFile.fileNo, dbo.dbClient.clNo, dbo.dbClient.clName, dbo.dbFile.fileNotes, 
                      dbo.dbMSData_OMS2K.MSNextDueDate, dbo.dbMSData_OMS2K.MSNextDueStage, dbo.dbFile.fileDepartment, dbo.dbFile.fileType, 
                      dbo.dbFile.fileStatus, dbo.dbMSData_OMS2K.MSCode, dbo.dbFile.fileFundCode, replace(dbo.dbFile.fileDesc, char(13) + char(10), '', '') as fileDesc
FROM         dbo.dbClient INNER JOIN
                      dbo.dbFile ON dbo.dbClient.clID = dbo.dbFile.clID INNER JOIN
                      dbo.dbMSData_OMS2K ON dbo.dbFile.fileID = dbo.dbMSData_OMS2K.fileID INNER JOIN
                      dbo.dbUser ON dbo.dbFile.filePrincipleID = dbo.dbUser.usrID'

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''

if coalesce(@msplan, '') <> ''
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

if coalesce(@filestat, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbfile.Filestatus = ''' + @filestat + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfile.Filestatus = ''' + @filestat + ''''
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

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepMilestoneContact] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepMilestoneContact] TO [OMSAdminRole]
    AS [dbo];

