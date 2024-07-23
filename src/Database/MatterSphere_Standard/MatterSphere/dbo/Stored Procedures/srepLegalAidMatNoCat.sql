

CREATE PROCEDURE [dbo].[srepLegalAidMatNoCat]
(@UI uUICultureInfo='{default}')

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbClient.clNo, dbo.dbClient.clName, dbo.dbFile.fileNo, replace(dbo.dbFile.fileDesc, char(13) + char(10), '', '') as fileDesc, dbo.dbFile.fileFundCode, dbo.dbFile.fileStatus, dbo.dbUser.usrID, 
                      dbo.dbUser.usrFullName, dbo.dbLegalAidCategory.LegAidCategory, dbo.dbFile.fileLACategory
FROM         dbo.dbClient INNER JOIN
                      dbo.dbFile ON dbo.dbClient.clID = dbo.dbFile.clID INNER JOIN
                      dbo.dbUser ON dbo.dbClient.clSourceUser = dbo.dbUser.usrID INNER JOIN
                      dbo.dbLegalAidCategory ON dbo.dbFile.fileSource = dbo.dbLegalAidCategory.LegAidCategory
WHERE     (dbo.dbLegalAidCategory.LegAidCategory IS NULL) AND (dbo.dbFile.fileLACategory <> 0) AND (dbo.dbFile.fileFundCode = N''LEGALAID'')'

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
    ON OBJECT::[dbo].[srepLegalAidMatNoCat] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepLegalAidMatNoCat] TO [OMSAdminRole]
    AS [dbo];

