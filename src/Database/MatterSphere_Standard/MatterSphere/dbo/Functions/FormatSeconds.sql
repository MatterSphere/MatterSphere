

CREATE FUNCTION [dbo].[FormatSeconds](@seconds bigint)
RETURNS nvarchar(50) AS  
BEGIN 

declare @value nvarchar(50)
declare @remainder bigint
--declare @days bigint
declare @hours bigint
declare @mins bigint
declare @secs bigint

set @remainder = @seconds
set @value = ''

/*
set @days = @seconds / 86400
if @days > 0
begin
	if (len(@value) > 0) 
		set @value = @value + ' '
	set @value = @value + cast(@days as nvarchar) + 'd'
end
else
begin
	if (len(@value) > 0) 
		set @value = @value + ' ' +  cast(@days as nvarchar) + 'd'
end
set @remainder = (@remainder - (@days * 86400))
*/

set @hours = @remainder / 3600
set @value = replicate(' ', 4-len(cast(@hours as nvarchar))) + cast(@hours as nvarchar) + 'h'
set @remainder = (@remainder - (@hours * 3600))


set @mins = @remainder / 60
set @value = @value + ' ' + replicate(' ', 2-len(cast(@mins as nvarchar))) + cast(@mins as nvarchar) + 'm'
set @remainder = (@remainder - (@mins * 60))

set @secs = @remainder
set @value = @value + ' ' + replicate(' ', 2-len(cast(@secs as nvarchar))) + cast(@secs as nvarchar) + 's'

return @value

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FormatSeconds] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FormatSeconds] TO [OMSAdminRole]
    AS [dbo];

