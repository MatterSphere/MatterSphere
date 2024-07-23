CREATE FUNCTION [dbo].[GetPrecedent]  (@Title nvarchar(100), @Type uCodeLookup, @Library uCodeLookup, @Category uCodeLookup, @SubCategory uCodeLookup, @MinorCategory uCodeLookup, @Language uUICultureInfo)  
RETURNS bigint AS  
BEGIN 
	
declare @precid bigint

if @language is null 
	set @language = ''

if @Library is null
	set @Library = ''

if @Category is null
	set @Category = ''

if @SubCategory is null
	set @SubCategory = ''

if @MinorCategory is null
	set @MinorCategory = ''

if @category = ''
	select @precid = precid from dbprecedents 
		where prectitle = @Title 
		and prectype = @Type 
		and coalesce(preclibrary, '') = @library 
		and preccategory is null 
		and precsubcategory is null 
		and precminorcategory is null 
		and @language like coalesce(preclanguage, '') + '%'
else
	select @precid = precid from dbprecedents 
		where prectitle = @Title 
		and prectype = @Type 
		and coalesce(preclibrary, '') = @library 
		and coalesce(preccategory, '') = @category 
		and coalesce(precsubcategory, '') = @subcategory 
		and coalesce(precminorcategory, '') = @minorcategory 
		and @language like coalesce(preclanguage, '') + '%'


return @precid

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPrecedent] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPrecedent] TO [OMSAdminRole]
    AS [dbo];

