

CREATE PROCEDURE [dbo].[srepFileCompSecurity]
(@UI uUICultureInfo='{default}')

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbClient.clNo, dbo.dbClient.clName, dbo.dbFile.fileNo, dbo.dbFile.fileDesc, dbo.dbUser.usrFullName, dbo.dbfileevents.evdesc, dbo.dbFileEvents.evWhen
FROM         dbo.dbFile INNER JOIN
                      dbo.dbFileEvents ON dbo.dbFile.fileID = dbo.dbFileEvents.fileID AND dbo.dbfileevents.evtype = ''SECAUDIT'' AND dbo.dbfileevents.evdesc LIKE ''Audit check%'' INNER JOIN
                      dbo.dbUser ON dbo.dbFileEvents.evusrID = dbo.dbUser.usrID INNER JOIN
                      dbo.dbClient ON dbo.dbFile.clID = dbo.dbClient.clID'

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''

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
    ON OBJECT::[dbo].[srepFileCompSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileCompSecurity] TO [OMSAdminRole]
    AS [dbo];

