

CREATE FUNCTION [dbo].[GetCodeLookupHelp]  (@Type uCodeLookup, @Code uCodeLookup, @UI uUICultureInfo)  
RETURNS nvarchar(1000) AS  
BEGIN 
	declare @cdhelp nvarchar (1000)
	select @cdhelp = cdhelp from dbcodelookup where cdtype = @Type and cdcode = @Code and @UI Like cdUICultureInfo + '%'
	if (@cdhelp is null)
		select @cdhelp = cdhelp from dbcodelookup where cdtype = @Type and cdcode = @Code and cdUICultureInfo = '{default}'
	return @cdhelp
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCodeLookupHelp] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCodeLookupHelp] TO [OMSAdminRole]
    AS [dbo];

