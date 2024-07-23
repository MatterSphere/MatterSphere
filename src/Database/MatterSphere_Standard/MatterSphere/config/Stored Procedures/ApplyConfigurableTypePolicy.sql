

CREATE PROCEDURE [config].[ApplyConfigurableTypePolicy]
	@configurableTypeCode uCodeLookup ,
	@securableType uCodeLookup ,
	@securableID bigint ,
	@adminUserID int


AS
SET NOCOUNT ON

DECLARE @PID bigint
-- security level check
IF ( SELECT [config].[GetSecurityLevel]() & 1920 ) = 0
	RETURN
		
SET XACT_ABORT ON
DECLARE @userGroupID uniqueidentifier , @policyID uniqueidentifier
DECLARE @users table ( UserGroupID uniqueidentifier , PolicyID uniqueidentifier )

SET @policyID = ( SELECT TOP 1 [PolicyID] FROM [config].[ConfigurableTypePolicy_UserGroup] WHERE [SecurableType] = @securableType AND [SecurableTypeCode] = @configurableTypeCode )
IF @securableType = 'CLIENTTYPE'
	GOTO Client_Policy

IF @securableType = 'FILETYPE'
	GOTO File_Policy

IF @securableType = 'DOCUMENTTYPE'
	GOTO Document_Policy


IF @securableType = 'CONTACTTYPE'
	GOTO Contact_Policy

Client_Policy:
-- security level check
IF ( SELECT [config].[GetSecurityLevel]() & 512 ) = 512
BEGIN
	IF EXISTS ( SELECT [PolicyID] FROM [config].[ConfigurableTypePolicy_UserGroup] WHERE  [SecurableType] = 'CLIENTTYPE' AND [SecurableTypeCode] = @configurableTypeCode )

	BEGIN 
		-- Get a list of Users/Groups that are configured to this policy
		INSERT @users SELECT DISTINCT  UserGroupID , PolicyID  FROM [config].[ConfigurableTypePolicy_UserGroup] WHERE  [SecurableType] = 'CLIENTTYPE' AND [SecurableTypeCode] = @configurableTypeCode
		-- Now apply the security 
		WHILE EXISTS ( SELECT UserGroupID FROM @users )
		BEGIN
			SELECT TOP 1 @userGroupID = UserGroupID , @policyID = PolicyID FROM @users 
			EXECUTE [config].[ApplyClientSecurity] @policyID = @policyID , @userGroupID = @userGroupID , @securableTypeID = @securableID , @adminUserID = @adminUserID 
			DELETE @users WHERE UserGroupID = @userGroupID 
		END
	END
	GOTO Finish
END



File_Policy:
-- security level check
IF ( SELECT [config].[GetSecurityLevel]() & 256 ) = 256
BEGIN
	IF EXISTS ( SELECT [PolicyID] FROM [config].[ConfigurableTypePolicy_UserGroup] WHERE  [SecurableType] = 'FILETYPE' AND [SecurableTypeCode] = @configurableTypeCode )

	BEGIN 
		-- Get a list of Users/Groups that are configured to this policy
		INSERT @users SELECT DISTINCT UserGroupID , PolicyID  FROM [config].[ConfigurableTypePolicy_UserGroup] WHERE  [SecurableType] = 'FILETYPE' AND [SecurableTypeCode] = @configurableTypeCode
		-- Now apply the security 
		WHILE EXISTS ( SELECT UserGroupID FROM @users )
		BEGIN
			SELECT TOP 1 @userGroupID = UserGroupID , @policyID = PolicyID FROM @users 
			/************************* CORRECTION CODE ***********************/
			select @PID=clid from config.dbFile where fileid = @securableID
			exec config.ApplyChildSecurity @ParentType=N'CLIENT',@ParentID=@PID,@ChildID=@securableID,@AdminUserID=@adminUserID
			IF NOT EXISTS (SELECT 1 from relationship.UserGroup_File where fileid =  @securableID and UserGroupID = @userGroupID)
			/******************** END *****************************************/
				EXECUTE [config].[ApplyFileSecurity] @policyID = @policyID , @userGroupID = @userGroupID , @securableTypeID = @securableID , @adminUserID = @adminUserID 
			DELETE @users WHERE UserGroupID = @userGroupID 
		END
	END
	GOTO Finish
END



Document_Policy:
-- security level check
IF ( SELECT [config].[GetSecurityLevel]() & 128 ) = 128
BEGIN
	IF EXISTS ( SELECT [PolicyID] FROM [config].[ConfigurableTypePolicy_UserGroup] WHERE  [SecurableType] = 'DOCUMENTTYPE' AND [SecurableTypeCode] = @configurableTypeCode )

	BEGIN 
		-- Get a list of Users/Groups that are configured to this policy
		INSERT @users SELECT DISTINCT UserGroupID , PolicyID  FROM [config].[ConfigurableTypePolicy_UserGroup] WHERE  [SecurableType] = 'DOCUMENTTYPE' AND [SecurableTypeCode] = @configurableTypeCode
		-- Now apply the security 
		WHILE EXISTS ( SELECT UserGroupID FROM @users )
		BEGIN
			SELECT TOP 1 @userGroupID = UserGroupID , @policyID = PolicyID FROM @users 
			/************************* CORRECTION CODE ***********************/
			select @PID=clid from config.dbFile where fileid = @securableID
			exec config.ApplyChildSecurity @ParentType=N'File',@ParentID=@PID,@ChildID=@securableID,@AdminUserID=@adminUserID
			IF NOT EXISTS (SELECT 1 from relationship.UserGroup_document where DocumentID =  @securableID and UserGroupID = @userGroupID)
			/******************** END *****************************************/
			EXECUTE [config].[ApplyDocumentSecurity] @policyID = @policyID , @userGroupID = @userGroupID , @securableTypeID = @securableID , @adminUserID = @adminUserID 
			DELETE @users WHERE UserGroupID = @userGroupID 
		END
	END
	GOTO Finish
END


Contact_Policy:
-- security level check
IF ( SELECT [config].[GetSecurityLevel]() & 1024 ) = 1024
BEGIN
	IF EXISTS ( SELECT [PolicyID] FROM [config].[ConfigurableTypePolicy_UserGroup] WHERE  [SecurableType] = 'CONTACTTYPE' AND [SecurableTypeCode] = @configurableTypeCode )

	BEGIN 
		-- Get a list of Users/Groups that are configured to this policy
		INSERT @users SELECT DISTINCT UserGroupID , PolicyID FROM [config].[ConfigurableTypePolicy_UserGroup] WHERE  [SecurableType] = 'CONTACTTYPE' AND [SecurableTypeCode] = @configurableTypeCode
		-- Now apply the security 
		WHILE EXISTS ( SELECT UserGroupID FROM @users )
		BEGIN
			SELECT TOP 1 @userGroupID = UserGroupID , @policyID = PolicyID FROM @users 
			/************************* CORRECTION CODE ***********************/
			select @PID=clid from dbo.dbClientContacts where contid = @securableID
			exec config.ApplyChildSecurity @ParentType=N'CONTACT',@ParentID=@PID,@ChildID=@securableID,@AdminUserID=@adminUserID
			IF NOT EXISTS (SELECT 1 from relationship.UserGroup_Contact where contactid =  @securableID and UserGroupID = @userGroupID)
			/******************** END *****************************************/
			EXECUTE [config].[ApplyContactSecurity] @policyID = @policyID , @userGroupID = @userGroupID , @securableTypeID = @securableID , @adminUserID = @adminUserID 
			DELETE @users WHERE UserGroupID = @userGroupID 
		END
	END
	GOTO Finish
END


Finish:
RETURN


GO
GRANT EXECUTE
    ON OBJECT::[config].[ApplyConfigurableTypePolicy] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ApplyConfigurableTypePolicy] TO [OMSAdminRole]
    AS [dbo];

