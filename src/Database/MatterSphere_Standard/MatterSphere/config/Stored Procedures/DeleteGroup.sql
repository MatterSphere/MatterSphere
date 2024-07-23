

CREATE PROCEDURE [config].[DeleteGroup]
	@groupName uCodeLookup,
	@adminUserID int

AS

SET NOCOUNT ON
DECLARE @groupID uniqueidentifier
IF @groupName = 'EVERYONE'
BEGIN
	RAISERROR ( 'Cannot delete the group Everyone ' , 25 , 1)
	RETURN
END

SET @groupID = ( SELECT [ID] FROM [item].[Group] WHERE [Name] = @groupName )
BEGIN TRY
BEGIN TRANSACTION
	-- Delete the Group associations
	DELETE [relationship].[Group_User] WHERE [GroupID] = @groupID
	DELETE [relationship].[UserGroup_Client] WHERE [UserGroupID] = @groupID
	DELETE [relationship].[UserGroup_Contact] WHERE [UserGroupID] = @groupID
	DELETE [relationship].[UserGroup_File] WHERE [UserGroupID] = @groupID
	DELETE [relationship].[UserGroup_Document] WHERE [UserGroupID] = @groupID
	-- Now audit the event
	INSERT [audit].[Group] ( [Updated] , [UpdatedBy] , [Event] ,  [GroupID] , [Name] , [Description] , [Active] , [PolicyID] )
	SELECT GetUTCDate() , @adminUserID , 'DELGROUP' , @groupID , [Name] , [Description] , [Active] , [PolicyID] FROM [item].[Group] WHERE [ID] = @groupID
	-- Finally delete the group
	DELETE [item].[Group] WHERE [Name] = @groupName
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
    ON OBJECT::[config].[DeleteGroup] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[DeleteGroup] TO [OMSAdminRole]
    AS [dbo];

