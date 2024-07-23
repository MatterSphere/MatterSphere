
CREATE PROCEDURE [dbo].[sprAudit]
	@license nvarchar(50)


AS
SET NOCOUNT ON

declare @serialno bigint
declare @database nvarchar(50)
declare @server nvarchar(50)
declare @currentcount bigint
declare @maxcount bigint
declare @month tinyint
declare @year int
declare @newchecksum varbinary(50)
declare @checksum varbinary(50)
declare @defaultsyslic nvarchar(50)
declare @brid int


begin transaction

select top 1 @brid = brid, @serialno = regserialno, @defaultsyslic = coalesce(nullif(dbo.Decr(regDefaultSystemLicense), ''), 'SYSTEM') from dbRegInfo


set @database =   coalesce(@database, DB_NAME())
set @server = coalesce(@server, @@SERVERNAME)
set @month = MONTH(GETUTCDATE())
set @year = YEAR(GETUTCDATE())

-- Get the current logged in count
select @currentcount = COUNT(TermName) 
from dbTerminal T
inner join dbUser U
on T.termLastUser = U.usrID
where termLoggedIn = 1 and coalesce(nullif(dbo.Decr(usrSystemLicense), ''), @defaultsyslic) = @license and T.brID = @brid

set @newchecksum = BINARY_CHECKSUM(@serialno,@license,@database,@server,@currentcount,@month,@year)

-- Grab the current max log in
select 
	@maxcount = [MaxCount],
	@checksum = BINARY_CHECKSUM([SerialNo],[License],[Database],[Server],[MaxCount],[Month],[Year])
	from dbAuditLog
	where 
	[SerialNo] = @serialno and
	[License] = @license and 
	[Database] = @database and 
	[Server] = @server and
	[Month] = @month and
	[Year] = @year
	

if @maxcount is null
begin
	insert into dbAuditLog
	(
		[SerialNo],
		[License],
		[Database], 
		[Server],  
		[MaxCount],
		[Month],
		[Year],
		[Checksum]
	) 
	values 
	(
		@serialno,
		@license, 
		@database, 
		@server, 
		@currentcount,
		@month,
		@year,
		@newchecksum
	)
	set @maxcount = @currentcount
	set @checksum = @newchecksum
end
else
begin
	if @currentcount > @maxcount
	begin
		update dbAuditLog 
			set [MaxCount] = @currentcount,
			[Checksum] = @newchecksum
		where
		[Checksum] = @checksum
	end
	
end
	

commit transaction



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAudit] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAudit] TO [OMSAdminRole]
    AS [dbo];

