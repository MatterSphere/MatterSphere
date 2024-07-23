Print 'Start AllInternalGroup'

SET NOCOUNT ON

/************ CREATE New Group ************/
DECLARE @groupname nvarchar(200)
SET @GroupName = 'AllInternal'
if not exists (SELECT * FROM [item].[Group] WHERE [Description] = @groupname)
begin
                INSERT [item].[Group] ([ID], [Name] , [Description] , [Active] , [PolicyID],[ADDistinguishedName] )
                VALUES ('DF08633D-9262-489F-B7F1-9A4FC56B41BC', @groupname , @groupname , 1 , newid(),'' );


/******* ADD USERS TO GROUP *****/
insert into [relationship].[Group_User] (GroupID,UserID)
select distinct b.ID,u.id
from [dbo].[dbuser] a 
join item.[User] u
                on a.usrADID = u.NTLogin
cross join [item].[Group] b
where a.AccessType = 'INTERNAL'
and b.Description = @groupname
and not exists (Select 1 from [relationship].[Group_User] x where x.GroupID = b.ID and x.UserID = u.ID)
end

GO
