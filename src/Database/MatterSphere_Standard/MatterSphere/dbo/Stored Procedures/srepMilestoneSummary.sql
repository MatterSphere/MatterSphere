

CREATE PROCEDURE [dbo].[srepMilestoneSummary]
(@UI uUICultureInfo='{default}',
@MSPlan nvarchar(50) = null,
@Source nvarchar(50) = null,
@feePrincipleID int = null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbUser.usrInits, dbo.dbUser.usrFullName, dbo.dbClient.clNo, dbo.dbClient.clName, dbo.dbFile.fileNo, replace(dbo.dbFile.fileDesc, char(13) + char(10), '', '') as fileDesc, dbo.dbClient.clSource, 
                      dbo.dbMSData_OMS2K.MSNextDueDate, dbo.dbMSData_OMS2K.MSNextDueStage, dbo.dbMSData_OMS2K.MSCode
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

if coalesce(@Source, '') <> ''
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
    ON OBJECT::[dbo].[srepMilestoneSummary] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepMilestoneSummary] TO [OMSAdminRole]
    AS [dbo];

