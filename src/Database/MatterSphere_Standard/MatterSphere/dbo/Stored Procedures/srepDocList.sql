

CREATE PROCEDURE [dbo].[srepDocList]
(@UI nvarchar(50)='{default}',
@DocType nvarchar(50) = '')
AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT * from dbdocument'

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''

if @doctype <> ''
BEGIN
     set @where = @where + ' doctype = ''' + @doctype + ''''
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


exec sp_executesql @sql,  N'@UI uUICultureInfo, @doctype uCodeLookup', @UI ,@doctype

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepDocList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepDocList] TO [OMSAdminRole]
    AS [dbo];

