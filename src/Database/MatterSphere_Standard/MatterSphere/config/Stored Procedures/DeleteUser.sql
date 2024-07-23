

CREATE PROCEDURE [config].[DeleteUser]
	@userName nvarchar(50),
	@adminUserID int

AS

SET NOCOUNT ON
DECLARE @userID uniqueidentifier

SET @userID = ( SELECT [ID] FROM [item].[User] WHERE [Name] = @userName )
BEGIN TRY
BEGIN TRANSACTION
	-- Delete the User associations
	DELETE [relationship].[Group_User] WHERE [UserID] = @userID
	DELETE [relationship].[UserGroup_Client] WHERE [UserGroupID] = @userID
	DELETE [relationship].[UserGroup_Contact] WHERE [UserGroupID] = @userID
	DELETE [relationship].[UserGroup_File] WHERE [UserGroupID] = @userID
	DELETE [relationship].[UserGroup_Document] WHERE [UserGroupID] = @userID
	-- Now audit the event
	INSERT [audit].[User] ( [Updated] , [UpdatedBy] , [Event] ,  [UserID] , [Name] , [NTLogin] , [Active] , [PolicyID] )
	SELECT GetUTCDate() , @adminUserID , 'DELUSER' , @userID , [Name] , [NTLogin] , [Active] , [PolicyID] FROM [item].[User] WHERE [ID] = @userID
	-- Finally delete the User
	DELETE [item].[User] WHERE [Name] = @userName
COMMIT TRANSACTION
END TRY
BEGIN CATCH
	IF @@TranCount <> 0
		ROLLBACK TRANSACTION
	DECLARE @err nvarchar(max)
	SET @err = ERROR_MESSAGE()
	RAISERROR ( @err , 16 , 1 )
END CATCH



GO
GRANT EXECUTE
    ON OBJECT::[config].[DeleteUser] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[DeleteUser] TO [OMSAdminRole]
    AS [dbo];

