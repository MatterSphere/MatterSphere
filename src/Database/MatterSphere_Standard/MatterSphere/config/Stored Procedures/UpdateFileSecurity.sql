

CREATE PROCEDURE [config].[UpdateFileSecurity]
	@policyID uniqueidentifier ,
	@securableID bigint ,
	@userGroupID uniqueidentifier ,
	@adminUserID int
	
AS
SET NOCOUNT ON

-- security level check
IF ( SELECT [config].[GetSecurityLevel]() & 384 ) = 0
	RETURN
	
DECLARE @oldPolicyID uniqueidentifier
SET @oldPolicyID = ( SELECT [PolicyID] FROM [relationship].[UserGroup_File]  WHERE [UserGroupID] = @userGroupID AND [FileID] = @securableID )

DECLARE @docTable table ( DocID bigint )

BEGIN TRY
	BEGIN TRANSACTION
		-- Update File
		-- security level check
		IF ( SELECT [config].[GetSecurityLevel]() & 256 ) = 256
		BEGIN
			UPDATE  [relationship].[UserGroup_File] SET [PolicyID] = @policyID,inherited = null WHERE [PolicyID] = @oldPolicyID AND [UserGroupID] = @userGroupID AND [FileID] = @securableID
			INSERT [audit].[UserGroup_File] ( Created , CreatedBy , [Event] , UserGroupID , FileID , PolicyID )
			VALUES ( Getdate() , @adminUserID , 'UPDTSECFIL' , @userGroupID , @securableID , @policyID )
		END
		-- Update Document
		-- security level check
		IF ( SELECT [config].[GetSecurityLevel]() & 128 ) = 128
		BEGIN
			UPDATE X SET X.[PolicyID] = @policyID, X.Inherited='F' 
			OUTPUT Deleted.DocumentID INTO @docTable
			FROM [relationship].[UserGroup_Document] X JOIN config.dbDocument D ON D.docID = X.DocumentID
			WHERE X.[PolicyID] = @oldPolicyID AND X.[UserGroupID] = @userGroupID AND D.[FileID] = @securableID
			INSERT [audit].[UserGroup_Document] ( Created , CreatedBy , [Event] , UserGroupID , DocumentID , PolicyID )
			SELECT Getdate() , @adminUserID , 'UPDTSECDOC' , @userGroupID , DocID , @policyID FROM @docTable
		END		
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
    ON OBJECT::[config].[UpdateFileSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[UpdateFileSecurity] TO [OMSAdminRole]
    AS [dbo];

