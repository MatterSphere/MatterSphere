CREATE PROCEDURE [dbo].[sprPrecedentMatch]  (@Title nvarchar(50), @Type uCodeLookup, @Library uCodeLookup = null, @Category uCodeLookup = null, @SubCategory uCodeLookup = null,  @MinorCategory uCodeLookup = null, @Language uUICultureInfo = null, @UI uUICultureInfo = '{default}') AS

declare @precid bigint
select @precid = precid from dbprecedents where precid = dbo.GetPrecedent(@title, @type, @library, @category, @subcategory, @minorcategory, @language)
execute sprPrecedentRecord @precid, @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprPrecedentMatch] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprPrecedentMatch] TO [OMSAdminRole]
    AS [dbo];

