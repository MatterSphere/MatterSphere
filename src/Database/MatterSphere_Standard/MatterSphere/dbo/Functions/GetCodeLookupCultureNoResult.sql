

CREATE FUNCTION [dbo].[GetCodeLookupCultureNoResult](@Type uCodeLookup, @Code uCodeLookup, @UI uUICultureInfo)
RETURNS nvarchar(15) AS  
BEGIN 
	declare @cdcult nvarchar (15)
	select @cdcult = cduicultureinfo from dbcodelookup where cdtype = @Type and cdcode = @Code and @UI Like cdUICultureInfo + '%'
	return @cdcult
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCodeLookupCultureNoResult] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCodeLookupCultureNoResult] TO [OMSAdminRole]
    AS [dbo];

