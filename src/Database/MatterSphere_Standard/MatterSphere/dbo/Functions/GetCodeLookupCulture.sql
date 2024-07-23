

CREATE FUNCTION [dbo].[GetCodeLookupCulture](@Type uCodeLookup, @Code uCodeLookup, @UI uUICultureInfo)
RETURNS nvarchar(15) AS  
BEGIN 
	declare @cdcult nvarchar (15)
	select @cdcult = cduicultureinfo from dbcodelookup where cdtype = @Type and cdcode = @Code and @UI Like cdUICultureInfo + '%'
	if (@cdcult is null)
		select @cdcult = cduicultureinfo from dbcodelookup where cdtype = @Type and cdcode = @Code and cdUICultureInfo = '{default}'
	return @cdcult
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCodeLookupCulture] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCodeLookupCulture] TO [OMSAdminRole]
    AS [dbo];

