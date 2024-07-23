CREATE PROCEDURE [config].[ApplyObjectPolicy]
	@policyID uniqueidentifier = NULL ,
	@policyTypeCode uCodeLookup = NULL ,
	@allowMask nchar(34) ,
	@denyMask nchar(34) ,
	@securableType uCodeLookup ,
	@userGroupID uniqueidentifier ,
	@securableID nvarchar(15) ,
	@adminUserID int


AS
SET NOCOUNT ON
SET XACT_ABORT ON
-- security level check
IF ( SELECT [config].[GetSecurityLevel] () & 1920 ) = 0 
	RETURN

DECLARE @allowMaskBin binary(34) , @denyMaskBin binary(34) , @oldAllowMask varbinary(34) , @oldDenyMask varbinary(34) , @oldPolicyTypeCode uCodeLookup , @oldPolicyID uniqueidentifier , @passedPolicyID uniqueidentifier , @sql nvarchar(400)
SET @sql = 'SELECT @allowMaskBin = ' + @allowMask + ' , @denyMaskBin = ' + @denyMask

EXECUTE sp_executesql @sql , N'@allowMaskBin varbinary(34) OUTPUT, @denyMaskBin varbinary(34) OUTPUT' , @allowMaskBin OUTPUT , @denyMaskBin OUTPUT

IF @policyTypeCode IS NULL
	SET @policyTypeCode = ( SELECT TOP 1 [Type] FROM [config].[ObjectPolicy] WHERE [ID] = @policyID )

-- If the policy is an Explicit one create the policy
IF @policyID IS NULL
BEGIN
	SET @passedPolicyID = NULL
	DECLARE @output table
		(
		[ID] uniqueidentifier
		)

		INSERT [config].[ObjectPolicy] ( [Type] , [AllowMask] , [DenyMask]  )
		OUTPUT Inserted.[ID] INTO @output
		VALUES ( 'EXPLICITOBJ' , @allowMaskBin , @denyMaskBin )
		SET @policyID = ( SELECT [ID] FROM @output )
		-- Now audit the policy creation
		INSERT [audit].[PolicyTemplate] (PolicyID , PolicyTypeCode , AllowMask , DenyMask , Name , Updated , UpdatedBy , AuditEvent )
		SELECT [ID] , 'EXPLICITOBJ' , @allowMaskBin , @denyMaskBin , 'Explicit object policy' , Getdate() , @adminUserID , 'NEWOBJPOLICY' FROM @output
END



IF @securableType IN ('ClientType' , 'FileType' , 'DocumentType' , 'ContactType' )
BEGIN
	SELECT @oldAllowMask = OP.[AllowMask] , @oldDenyMask =  OP.[DenyMask] , @oldPolicyTypeCode = OP.[Type] , @oldPolicyID = OP.[ID] FROM [config].[ObjectPolicy] OP WHERE [ID] = @policyID
	IF EXISTS ( SELECT @allowMask , @denyMask EXCEPT SELECT @oldAllowMask , @oldDenyMask ) AND @oldPolicyTypeCode = 'EXPLICITOBJ' AND @policyTypeCode = 'EXPLICITOBJ'
	BEGIN
		-- Update to the mask values only
		UPDATE [config].[ObjectPolicy] SET [AllowMask] = @allowMaskBin , [DenyMask] = @denyMaskBin WHERE [ID] = @policyID
		-- Now audit the event
		INSERT [audit].[PolicyTemplate] (PolicyID , PolicyTypeCode , AllowMask , DenyMask , Name , Updated , UpdatedBy , AuditEvent )
		VALUES ( @policyID , 'EXPLICITOBJ' , @allowMaskBin , @denyMaskBin , 'Explicit object policy' , Getdate() , @adminUserID , 'UPTOBJPOLICY' )
		GOTO Finish
	END

	IF EXISTS ( SELECT UserGroupID , SecurableType , SecurableTypeCode FROM [config].[ConfigurableTypePolicy_UserGroup] INTERSECT
		SELECT @userGroupID , @securableType , @securableID )
	BEGIN
		UPDATE [config].[ConfigurableTypePolicy_UserGroup]
		SET PolicyID = @policyID
		WHERE UserGroupID = @userGroupID AND SecurableType = @securableType AND SecurableTypeCode = @securableID
		-- Now audit the event
		INSERT [audit].[ConfigurableTypePolicy_UserGroup] ( AuditDate , AuditUserID , AuditEvent , UserGroupID , PolicyID , SecurableType , SecurableTypeCode , rowguid )
		SELECT Getdate() , @adminUserID , 'UPDTOBJPOLICY' , @userGroupID , @policyID , @securableType , @securableID , rowguid FROM [config].[ConfigurableTypePolicy_UserGroup] 
		WHERE PolicyID = @policyID AND UserGroupID = @userGroupID AND SecurableType = @securableType AND SecurableTypeCode = @securableID
	END
	ELSE
	BEGIN
		INSERT [config].[ConfigurableTypePolicy_UserGroup] ( UserGroupID , PolicyID , SecurableType , SecurableTypeCode )
		VALUES ( @userGroupID , @policyID , @securableType , @securableID )
		-- Now audit the event
		INSERT [audit].[ConfigurableTypePolicy_UserGroup] ( AuditDate , AuditUserID , AuditEvent , UserGroupID , PolicyID , SecurableType , SecurableTypeCode , rowguid )
		SELECT Getdate() , @adminUserID , 'NEWOBJPOLICY' , @userGroupID , @policyID , @securableType , @securableID , rowguid FROM [config].[ConfigurableTypePolicy_UserGroup] 
		WHERE PolicyID = @policyID AND UserGroupID = @userGroupID AND SecurableType = @securableType AND SecurableTypeCode = @securableID		
	END
	RETURN
END


-- Check for an update to the masks for an Explicit Policy


-- Check if the policy is an update or a new one
IF @securableType = 'CLIENT'
	GOTO Client_Policy

IF @securableType = 'CONTACT'
	GOTO Contact_Policy

IF @securableType = 'FILE'
	GOTO File_Policy

IF @securableType = 'DOCUMENT'
	GOTO Document_Policy


Client_Policy:
-- security level check
IF (SELECT [config].[GetSecurityLevel] () & 512 ) = 512
BEGIN 
	IF EXISTS ( SELECT [RelationshipID] FROM [relationship].[UserGroup_Client] WHERE [UserGroupID] = @userGroupID AND [ClientID] = @securableID )
	BEGIN
		-- Check for an update to the masks for an Explicit Policy
		SELECT @oldAllowMask = OP.[AllowMask] , @oldDenyMask =  OP.[DenyMask] , @oldPolicyTypeCode = OP.[Type] , @oldPolicyID = OP.[ID] FROM [relationship].[UserGroup_Client] UC JOIN [config].[ObjectPolicy] OP 
			ON UC.[PolicyID] = OP.[ID] WHERE [UserGroupID] = @userGroupID AND [ClientID] = @securableID
		IF EXISTS ( SELECT @allowMask , @denyMask EXCEPT SELECT @oldAllowMask , @oldDenyMask ) AND @oldPolicyTypeCode = 'EXPLICITOBJ' AND @policyTypeCode = 'EXPLICITOBJ'
		BEGIN
			-- Update to the mask values only
			UPDATE [config].[ObjectPolicy] SET [AllowMask] = @allowMaskBin , [DenyMask] = @denyMaskBin WHERE [ID] = @policyID
			-- Now audit the event
			INSERT [audit].[PolicyTemplate] (PolicyID , PolicyTypeCode , AllowMask , DenyMask , Name , Updated , UpdatedBy , AuditEvent )
			VALUES ( @policyID , 'EXPLICITOBJ' , @allowMaskBin , @denyMaskBin , 'Explicit object policy' , Getdate() , @adminUserID , 'UPTOBJPOLICY' )
			GOTO Finish
		END
		-- Update to policy
		EXECUTE [config].[UpdateClientSecurity] @policyID = @policyID , @securableID = @securableID , @userGroupID = @userGroupID , @adminUserID = @adminUserID 
		GOTO Finish
	END
	ELSE
	BEGIN
		EXECUTE [config].[ApplyClientSecurity] @policyID = @policyID , @userGroupID = @userGroupID , @securableTypeID = @securableID , @adminUserID = @adminUserID 
		GOTO Finish
	END
END

File_Policy:
-- security level check
IF (SELECT [config].[GetSecurityLevel] () & 256 ) = 256
BEGIN
	IF EXISTS ( SELECT [RelationshipID] FROM [relationship].[UserGroup_File] WHERE [UserGroupID] = @userGroupID AND [FileID] = @securableID)
	BEGIN
		-- Check for an update to the masks for an Explicit Policy
		SELECT @oldAllowMask = OP.[AllowMask] , @oldDenyMask =  OP.[DenyMask] , @oldPolicyTypeCode = OP.[Type] , @oldPolicyID = OP.[ID] FROM [relationship].[UserGroup_File] UF JOIN [config].[ObjectPolicy] OP 
			ON UF.[PolicyID] = OP.[ID] WHERE [UserGroupID] = @userGroupID AND [FileID] = @securableID
		IF EXISTS ( SELECT @allowMask , @denyMask EXCEPT SELECT @oldAllowMask , @oldDenyMask ) AND @oldPolicyTypeCode = 'EXPLICITOBJ' AND @policyTypeCode = 'EXPLICITOBJ'
		BEGIN
			-- Update to the mask values only
			UPDATE [config].[ObjectPolicy] SET [AllowMask] = @allowMaskBin , [DenyMask] = @denyMaskBin WHERE [ID] = @policyID
			-- Now audit the event
			INSERT [audit].[PolicyTemplate] (PolicyID , PolicyTypeCode , AllowMask , DenyMask , Name , Updated , UpdatedBy , AuditEvent )
			VALUES ( @policyID , 'EXPLICITOBJ' , @allowMaskBin , @denyMaskBin , 'Explicit object policy' , Getdate() , @adminUserID , 'UPTOBJPOLICY' )
			GOTO Finish
		END
		-- Update to policy
		EXECUTE [config].[UpdateFileSecurity] @policyID = @policyID , @securableID = @securableID , @userGroupID = @userGroupID , @adminUserID = @adminUserID 
		GOTO Finish
	END
	ELSE
	BEGIN
		EXECUTE [config].[ApplyFileSecurity] @policyID = @policyID , @userGroupID = @userGroupID , @securableTypeID = @securableID , @adminUserID = @adminUserID 
		GOTO Finish
	END
END

Document_Policy:
-- security level check
IF ( SELECT [config].[GetSecurityLevel] () & 128 ) = 128
BEGIN
	IF EXISTS ( SELECT [RelationshipID] FROM [relationship].[UserGroup_Document] WHERE [UserGroupID] = @userGroupID AND [DocumentID] = @securableID )
	BEGIN
		-- Check for an update to the masks for an Explicit Policy
		SELECT @oldAllowMask = OP.[AllowMask] , @oldDenyMask =  OP.[DenyMask] , @oldPolicyTypeCode = OP.[Type] , @oldPolicyID = OP.[ID] FROM [relationship].[UserGroup_Document] UD JOIN [config].[ObjectPolicy] OP 
			ON UD.[PolicyID] = OP.[ID] WHERE [UserGroupID] = @userGroupID AND [DocumentID] = @securableID
		IF EXISTS ( SELECT @allowMask , @denyMask EXCEPT SELECT @oldAllowMask , @oldDenyMask ) AND @oldPolicyTypeCode = 'EXPLICITOBJ' AND @policyTypeCode = 'EXPLICITOBJ'
		BEGIN
			-- Update to the mask values only
			UPDATE [config].[ObjectPolicy] SET [AllowMask] = @allowMaskBin , [DenyMask] = @denyMaskBin WHERE [ID] = @policyID
			-- Now audit the event
			INSERT [audit].[PolicyTemplate] (PolicyID , PolicyTypeCode , AllowMask , DenyMask , Name , Updated , UpdatedBy , AuditEvent )
			VALUES ( @policyID , 'EXPLICITOBJ' , @allowMaskBin , @denyMaskBin , 'Explicit object policy' , Getdate() , @adminUserID , 'UPTOBJPOLICY' )
			GOTO Finish
		END
		-- Update to policy
		EXECUTE [config].[UpdateDocumentSecurity] @policyID = @policyID , @securableID = @securableID , @userGroupID = @userGroupID , @adminUserID = @adminUserID 
		GOTO Finish
	END
	ELSE
	BEGIN
		EXECUTE [config].[ApplyDocumentSecurity] @policyID = @policyID , @userGroupID = @userGroupID , @securableTypeID = @securableID , @adminUserID = @adminUserID 
		GOTO Finish
	END
END

Contact_Policy:
-- security level check
IF (SELECT [config].[GetSecurityLevel] () & 1024 ) = 1024
	BEGIN
	IF EXISTS ( SELECT [RelationshipID] FROM [relationship].[UserGroup_Contact] WHERE [UserGroupID] = @userGroupID AND [ContactID] = @securableID )
	BEGIN
		-- Check for an update to the masks for an Explicit Policy
		SELECT @oldAllowMask = OP.[AllowMask] , @oldDenyMask =  OP.[DenyMask] , @oldPolicyTypeCode = OP.[Type] , @oldPolicyID = OP.[ID] FROM [relationship].[UserGroup_Contact] UC JOIN [config].[ObjectPolicy] OP 
			ON UC.[PolicyID] = OP.[ID] WHERE [UserGroupID] = @userGroupID AND [ContactID] = @securableID
		IF EXISTS ( SELECT @allowMask , @denyMask EXCEPT SELECT @oldAllowMask , @oldDenyMask ) AND @oldPolicyTypeCode = 'EXPLICITOBJ' AND @policyTypeCode = 'EXPLICITOBJ'
		BEGIN
			-- Update to the mask values only
			UPDATE [config].[ObjectPolicy] SET [AllowMask] = @allowMaskBin , [DenyMask] = @denyMaskBin WHERE [ID] = @policyID
			-- Now audit the event
			INSERT [audit].[PolicyTemplate] (PolicyID , PolicyTypeCode , AllowMask , DenyMask , Name , Updated , UpdatedBy , AuditEvent )
			VALUES ( @policyID , 'EXPLICITOBJ' , @allowMaskBin , @denyMaskBin , 'Explicit object policy' , Getdate() , @adminUserID , 'UPTOBJPOLICY' )
			GOTO Finish
		END
		-- Update to policy
		EXECUTE [config].[UpdateContactSecurity] @policyID = @policyID , @securableID = @securableID , @userGroupID = @userGroupID , @adminUserID = @adminUserID 
		GOTO Finish
	END
	ELSE
	BEGIN
		EXECUTE [config].[ApplyContactSecurity] @policyID = @policyID , @userGroupID = @userGroupID , @securableTypeID = @securableID , @adminUserID = @adminUserID 
		GOTO Finish
	END
END

Finish:
RETURN

GO
GRANT EXECUTE
    ON OBJECT::[config].[ApplyObjectPolicy] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ApplyObjectPolicy] TO [OMSAdminRole]
    AS [dbo];

