

CREATE PROCEDURE [config].[UpdateDocumentSecurity]
	@policyID uniqueidentifier ,
	@securableID bigint ,
	@userGroupID uniqueidentifier ,
	@adminUserID int = NULL
	
AS
SET NOCOUNT ON

-- security level check
IF ( SELECT [config].[GetSecurityLevel]() & 128 ) = 0
	RETURN
		
DECLARE @oldPolicyID uniqueidentifier
SET @oldPolicyID = ( SELECT [PolicyID] FROM [relationship].[UserGroup_Document]  WHERE [UserGroupID] = @userGroupID AND [DocumentID] = @securableID )

BEGIN TRY
	BEGIN TRANSACTION

		-- Update Document
		UPDATE  [relationship].[UserGroup_Document] SET [PolicyID] = @policyID,inherited = null WHERE [PolicyID] = @oldPolicyID AND [UserGroupID] = @userGroupID AND [DocumentID] = @securableID

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
    ON OBJECT::[config].[UpdateDocumentSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[UpdateDocumentSecurity] TO [OMSAdminRole]
    AS [dbo];

