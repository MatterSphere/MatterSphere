

CREATE PROCEDURE [dbo].[sprPermissions] (@UserID int, @Type uCodeLookup, @UI uUICultureInfo = '{default}')
AS 

declare @Role uCodeLookup

select @Role = rolecode from dbsecurityrole where rolecode = (select usrrole from dbuser where usrid = @UserID)


--PERMISSIONS
select * from dbsecuritypermission P
where 
	permtype = @Type 
	and coalesce(permRole, '') = (case usrid when null then @Role else coalesce(permRole, '') end)
	and coalesce(usrID, '') = 
		coalesce(
			(select top 1 usrid from dbsecuritypermission 
				where 
					permType = @Type 
					and usrid = @UserID 
					and coalesce(permObjectS, '') = coalesce(P.permObjectS, '') 
					and coalesce(permObjectN, -1) = coalesce(permObjectN, -1) 
					and permCode = P.permCode), '')

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprPermissions] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprPermissions] TO [OMSAdminRole]
    AS [dbo];

