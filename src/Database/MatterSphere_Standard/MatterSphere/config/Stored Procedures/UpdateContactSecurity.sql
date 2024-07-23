

CREATE PROCEDURE [config].[UpdateContactSecurity]
	@policyID uniqueidentifier ,
	@securableID bigint ,
	@userGroupID uniqueidentifier ,
	@adminUserID int

AS
SET NOCOUNT ON

-- security level check
IF ( SELECT [config].[GetSecurityLevel]() & 1024 ) = 0
	RETURN

DECLARE @oldPolicyID uniqueidentifier
SET @oldPolicyID = ( SELECT [PolicyID] FROM [relationship].[UserGroup_Contact]  WHERE [UserGroupID] = @userGroupID AND [ContactID] = @securableID )

BEGIN TRY
	BEGIN TRANSACTION
		-- Update Contact
		UPDATE  [relationship].[UserGroup_Contact] SET [PolicyID] = @policyID,inherited = null WHERE [PolicyID] = @oldPolicyID AND [UserGroupID] = @userGroupID AND [ContactID] = @securableID
		-- Audit the security event
		INSERT [audit].[UserGroup_Contact] ( [Created] , [CreatedBy] , [Event] , [UserGroupID] , [ContactID] , [PolicyID] )
		VALUES ( GetUTCDate() , @adminUserID , 'UPDTSECCONT' ,  @userGroupID , @securableID , @policyID )

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	IF @@Trancount <> 0
		ROLLBACK TRANSACTION
	DECLARE @er nvarchar(max)
	SET @er = ERROR_MESSAGE()
	RAISERROR ( @er , 16 ,1 )
END CATCH


GO
GRANT EXECUTE
    ON OBJECT::[config].[UpdateContactSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[UpdateContactSecurity] TO [OMSAdminRole]
    AS [dbo];

