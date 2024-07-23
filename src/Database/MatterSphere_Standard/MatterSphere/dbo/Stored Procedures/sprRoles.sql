

CREATE PROCEDURE [dbo].[sprRoles] (@UserID int, @Role uCodeLookup, @UI uUICultureInfo = '{default}')
AS 

--ROLE
if @Role is null
	select @Role = rolecode from dbsecurityrole where rolecode = (select usrrole from dbuser where usrid = @UserID)

if (select count(rolecode) from dbsecurityrole where rolecode = @Role) = 0
	select * from dbsecurityrole where rolecode = 'USER'
else
	select * from dbsecurityrole where rolecode = @Role

--PERMISSION_TYPE
select * from dbsecuritypermissiontype

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprRoles] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprRoles] TO [OMSAdminRole]
    AS [dbo];

