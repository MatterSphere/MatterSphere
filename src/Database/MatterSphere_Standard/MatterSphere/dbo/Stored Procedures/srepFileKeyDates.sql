

CREATE PROCEDURE [dbo].[srepFileKeyDates]
(@UI uUICultureInfo='{default}',
@clno nvarchar(50) = null,
@fileno nvarchar(50) = null,
@kdactive nvarchar(50) = null)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbClient.clNo, dbo.dbFile.fileNo, dbo.dbClient.clName, replace(dbo.dbFile.fileDesc, char(13) + char(10), '', '') as fileDesc, dbo.dbKeyDates.kdDesc, dbo.dbKeyDates.kdDate, dbo.dbUser.usrInits, 
                      dbo.dbFile.fileStatus, dbo.dbUser.usrFullName
FROM         dbo.dbFile INNER JOIN
                      dbo.dbClient ON dbo.dbFile.clID = dbo.dbClient.clID INNER JOIN
                      dbo.dbKeyDates ON dbo.dbFile.fileID = dbo.dbKeyDates.fileID AND dbo.dbkeydates.kdactive = ''1'' INNER JOIN
                      dbo.dbUser ON dbo.dbFile.filePrincipleID = dbo.dbUser.usrID'

---- Debug Print
PRINT @SELECT

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
	     set @where = @where + ' AND dbo.dbfile.fileno = ''' + @fileno + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbfile.fileno = ''' + @fileno + ''''
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
    ON OBJECT::[dbo].[srepFileKeyDates] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileKeyDates] TO [OMSAdminRole]
    AS [dbo];

