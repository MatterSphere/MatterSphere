

CREATE FUNCTION [dbo].[GetCodeLookupHelpNoResult]  (@Type uCodeLookup, @Code uCodeLookup, @UI uUICultureInfo)  
RETURNS nvarchar(1000) AS  
BEGIN 
	declare @cdhelp nvarchar (1000)
	select @cdhelp = cdhelp from dbcodelookup where cdtype = @Type and cdcode = @Code and @UI Like cdUICultureInfo + '%'
	return @cdhelp
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCodeLookupHelpNoResult] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCodeLookupHelpNoResult] TO [OMSAdminRole]
    AS [dbo];

