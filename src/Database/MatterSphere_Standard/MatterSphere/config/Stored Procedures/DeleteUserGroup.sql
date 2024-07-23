

CREATE PROCEDURE [config].[DeleteUserGroup]
	@userGroupID uniqueidentifier,
	@securableType uCodelookup,
	@securableID bigint,
	@parentSecurableID bigint = null,
	@adminUserID bigint

AS
SET NOCOUNT ON
SET ANSI_WARNINGS OFF
	DECLARE @err nvarchar(2000) , @policyID uniqueidentifier


IF @securableType IN ( 'clientTYpe' , 'FileType' , 'DocumentType' , 'ConactType' )
BEGIN
	DELETE [config].[ConfigurableTypePolicy_UserGroup]
	WHERE UserGroupID = @userGroupID AND SecurableType = @securableType AND SecurableTypeCode = @securableID
	RETURN
END

IF @securableType = 'CLIENT'
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			DELETE [relationship].[UserGroup_Client] WHERE UserGroupID = @userGroupID
			DELETE [relationship].[UserGroup_Contact] WHERE UserGroupID = @userGroupID
			DELETE [relationship].[UserGroup_File] WHERE UserGroupID = @userGroupID
			DELETE [relationship].[UserGroup_Document] WHERE UserGroupID = @userGroupID
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TranCount <> 0
			ROLLBACK TRANSACTION
		SET @err = ( SELECT ERROR_MESSAGE() )
		RAISERROR (@err , 16 , 1)
	END CATCH
RAISERROR ( 'PERMRESTORE' , 16 , 1)
RETURN
END

IF @securableType = 'CONTACT'
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			DELETE [relationship].[UserGroup_Contact] WHERE UserGroupID = @userGroupID
		--	DELETE [relationship].[UserGroup_File] WHERE UserGroupID = @userGroupID
		--	DELETE [relationship].[UserGroup_Document] WHERE UserGroupID = @userGroupID
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TranCount <> 0
			ROLLBACK TRANSACTION
		SET @err = ( SELECT ERROR_MESSAGE() )
		RAISERROR (@err , 16 , 1)
	END CATCH
RETURN
END


IF @securableType = 'FILE'
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			DELETE [relationship].[UserGroup_File] WHERE UserGroupID = @userGroupID
			DELETE [relationship].[UserGroup_Document] WHERE UserGroupID = @userGroupID
			IF EXISTS ( SELECT RelationshipID FROM [relationship].[UserGroup_Client] WHERE [UserGroupID] = @userGroupID AND [ClientID] = @parentSecurableID )
			BEGIN
				SET @policyID = ( SELECT PolicyID FROM [relationship].[UserGroup_Client] WHERE [UserGroupID] = @userGroupID AND [ClientID] = @parentSecurableID )
				EXECUTE [config].[ApplyFileSecurity] @policyID = @policyID , @userGroupID = @userGroupID , @securableTypeID = @securableID , @adminUserID = @adminUserID
			END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TranCount <> 0
			ROLLBACK TRANSACTION
		SET @err = ( SELECT ERROR_MESSAGE() )
		RAISERROR (@err , 16 , 1)
	END CATCH
RAISERROR ( 'PERMRESTORE' , 16 , 1)
RETURN
END


IF @securableType = 'DOCUMENT'
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			DELETE [relationship].[UserGroup_Document] WHERE UserGroupID = @userGroupID
			IF EXISTS ( SELECT RelationshipID FROM [relationship].[UserGroup_File] WHERE [UserGroupID] = @userGroupID AND [FileID] = @parentSecurableID )
			BEGIN
				SET @policyID = ( SELECT PolicyID FROM [relationship].[UserGroup_File] WHERE [UserGroupID] = @userGroupID AND [FileID] = @parentSecurableID )
				EXECUTE [config].[ApplyDocumentSecurity] @policyID = @policyID , @userGroupID = @userGroupID , @securableTypeID = @securableID , @adminUserID = @adminUserID
			END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TranCount <> 0
			ROLLBACK TRANSACTION
		SET @err = ( SELECT ERROR_MESSAGE() )
		RAISERROR (@err , 16 , 1)
	END CATCH
RAISERROR ( 'PERMRESTORE' , 16 , 1)
RETURN
END



GO
GRANT EXECUTE
    ON OBJECT::[config].[DeleteUserGroup] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[DeleteUserGroup] TO [OMSAdminRole]
    AS [dbo];

