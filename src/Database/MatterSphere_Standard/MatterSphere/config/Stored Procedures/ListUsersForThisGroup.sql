
CREATE PROCEDURE [config].[ListUsersForThisGroup]
	@ID nvarchar(max)
	
AS
SET NOCOUNT ON

SELECT 
	dbo.dbUser.usrID,dbo.dbUser.usrFullName 
FROM 
	relationship.Group_User 
	INNER JOIN item.[User] ON relationship.Group_User.UserID = item.[User].ID 
	INNER JOIN dbo.dbUser ON item.[User].NTLogin = dbo.dbUser.usrADID 
WHERE 
	GroupID = @ID


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListUsersForThisGroup] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListUsersForThisGroup] TO [OMSAdminRole]
    AS [dbo];

