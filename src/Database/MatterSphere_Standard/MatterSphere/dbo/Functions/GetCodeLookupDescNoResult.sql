

CREATE FUNCTION [dbo].[GetCodeLookupDescNoResult] (@Type uCodeLookup, @Code uCodeLookup, @UI uUICultureInfo)  
RETURNS nvarchar(1000) AS  
BEGIN 
	declare @cddesc nvarchar (1000)
	select @cddesc = cddesc from dbcodelookup where cdtype = @Type and cdcode = @Code and @UI Like cdUICultureInfo + '%'
	return @cddesc
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCodeLookupDescNoResult] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCodeLookupDescNoResult] TO [OMSAdminRole]
    AS [dbo];

