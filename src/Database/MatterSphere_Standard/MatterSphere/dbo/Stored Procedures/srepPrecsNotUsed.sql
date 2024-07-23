

CREATE PROCEDURE [dbo].[srepPrecsNotUsed]
(@UI uUICultureInfo='{default}')

AS 

declare @Select nvarchar(1800)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)
declare @having nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbPrecedents.PrecTitle, dbo.dbPrecedents.PrecType, dbo.dbPrecedents.PrecDesc, dbo.dbPrecedents.PrecCategory, 
                      dbo.dbPrecedents.PrecSubCategory, COUNT(dbo.dbDocument.docprecID) AS CountOfDocs
FROM         dbo.dbDocument RIGHT OUTER JOIN
                      dbo.dbPrecedents ON dbo.dbDocument.docbaseprecID = dbo.dbPrecedents.PrecID
GROUP BY dbo.dbPrecedents.PrecTitle, dbo.dbPrecedents.PrecDesc, dbo.dbPrecedents.PrecCategory, dbo.dbPrecedents.PrecSubCategory, 
                      dbo.dbPrecedents.PrecType'

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''
SET @HAVING =''

if @having <> ''
	BEGIN
	     set @having = @having + ' AND (COUNT(dbo.dbDocument.docprecID) = 0)'
	END
    ELSE
	BEGIN
	     set @having = @having + ' (COUNT(dbo.dbDocument.docprecID) = 0)'
	END

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

if @having <> ''
BEGIN
	set @having = N' HAVING ' + @having
END

---- Build OrderBy Clause
set @orderby = N''

declare @sql nvarchar(4000)
--- Add Statements together
set @sql = @select + @where + @orderby + @having

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepPrecsNotUsed] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepPrecsNotUsed] TO [OMSAdminRole]
    AS [dbo];

