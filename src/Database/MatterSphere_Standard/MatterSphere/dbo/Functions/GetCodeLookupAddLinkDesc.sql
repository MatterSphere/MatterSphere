

CREATE  FUNCTION [dbo].[GetCodeLookupAddLinkDesc]  (@Type uCodeLookup, @Code uCodeLookup, @UI uUICultureInfo, @AddLink uCodeLookup)  
RETURNS nvarchar(1000) AS  
BEGIN 
declare @cddesc nvarchar (1000)
	select @cddesc = cddesc from dbcodelookup where cdtype = @Type and cdcode = @Code and coalesce(cdaddlink,'') = coalesce(@AddLink,'') and @UI Like cdUICultureInfo + '%'
	if (@cddesc is null)
		select @cddesc = cddesc from dbcodelookup where cdtype = @Type and cdcode = @Code and coalesce(cdaddlink,'') = coalesce(@AddLink,'') and cdUICultureInfo = '{default}'
	if (@cddesc is null) and not (@code = '' or @code is null) --- Still null return the query name
		select @cddesc = '~' + @code + '~'
	return @cddesc
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCodeLookupAddLinkDesc] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCodeLookupAddLinkDesc] TO [OMSAdminRole]
    AS [dbo];

