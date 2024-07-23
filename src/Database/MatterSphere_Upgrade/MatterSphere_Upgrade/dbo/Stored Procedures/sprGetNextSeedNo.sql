

CREATE PROCEDURE [dbo].[sprGetNextSeedNo] (@Branch int, @Code uCodeLookup, @lastused nvarchar(20) = null, @number nvarchar(100) output, @newentityid bigint = null)
as
	declare @auto bit
	declare @prefix nvarchar(20)
	declare @suffix nvarchar(20)
	declare @sql nvarchar(500)
	declare @last nvarchar(20)
select 
	@auto = seedauto,
	@prefix = seedprefix,
	@suffix = seedsuffix,
	@last = seedlastused,
	@sql = seedsql
from
	dbseed where seedtype = @Code and brid = @branch

if @@rowcount = 0
	return 1 -- No seed counter exists.
else if @auto = 0
	return 2 -- The seed counter exists but the number is not used.
begin
	declare @updateseed bit
set @updateseed = 1
 
set @number = Isnull(@prefix, '')
 
if not @sql is null and Ltrim(@sql) <> ''
begin
	set @sql = 'set @last = (' + @sql + N')'
	exec sp_executesql @sql, N'@last nvarchar(20) output, @id bigint', @last output, @newentityid
	set @lastused = @last
	IF @lastused IS NULL
		RAISERROR ( 'Error generating seed number for code %s and Branch %d' , 16 , 1 , @code , @branch )
end

if @prefix = 'SPECIAL' 
begin 
	set @number = @last
	return 0
end 
 
if not @lastused is null
begin
	if @prefix = substring(@lastused, 1, len(@prefix))
		set @lastused = substring(@lastused, len(@prefix) + 1, len(@lastused))
	if reverse(@suffix) = substring(reverse(@lastused), 1, len(@suffix))
		set @lastused = reverse(substring(reverse(@lastused), len(@suffix) + 1, len(@lastused)))
	if isnumeric(@lastused) = 1
		set @last = cast(@lastused as bigint)
	else
		set @last = 0
	set @updateseed = 0
end

set @last = IsNull(@last, 0) + 1
set @number = @number + convert(nvarchar, @last)
set @number = @number + IsNull(@suffix, '')
 
 
if @updateseed = 1 
	update dbseed set seedlastused = @last where seedtype = @code and brid = @Branch

return 0 -- Everything hunky dory.
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprGetNextSeedNo] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprGetNextSeedNo] TO [OMSAdminRole]
    AS [dbo];

