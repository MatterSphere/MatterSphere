

CREATE   FUNCTION [dbo].[GetCodeLookupDesc]  (@Type uCodeLookup, @Code uCodeLookup, @UI uUICultureInfo)  
RETURNS nvarchar(1000) AS  
BEGIN 
	declare @cddesc nvarchar (1000)
	select @cddesc = cddesc from dbcodelookup where cdtype = @Type and cdcode = @Code and @UI Like cdUICultureInfo + '%'
	if (@cddesc is null)
		select @cddesc = cddesc from dbcodelookup where cdtype = @Type and cdcode = @Code and cdUICultureInfo = '{default}'
	if (@cddesc is null) and not (@code = '' or @code is null) --- Still null return the query name
		select @cddesc = '~' + @code + '~'
	return @cddesc
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCodeLookupDesc] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCodeLookupDesc] TO [OMSAdminRole]
    AS [dbo];

