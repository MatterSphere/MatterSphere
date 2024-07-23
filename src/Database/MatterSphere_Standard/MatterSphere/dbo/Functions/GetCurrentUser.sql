

CREATE FUNCTION [dbo].[GetCurrentUser] ()
RETURNS int
AS
BEGIN
	declare @ntuser nvarchar(100)
declare @usrid int
select @ntuser = nt_username from master.dbo.sysprocesses sp where sp.spID = @@spid 

if not @ntuser is null
	select @usrid = usrid from dbuser where usradid =  CURRENT_USER
else
	select @usrid = usrid from dbuser where usrsqlid =  CURRENT_USER

if @usrid is null
	select @usrid = usrid from dbuser where usradid =  CURRENT_USER

return @usrid

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentUser] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentUser] TO [OMSAdminRole]
    AS [dbo];

