

CREATE PROCEDURE [config].[UpdateClientSecurity]
	@policyID uniqueidentifier ,
	@securableID bigint , 
	@userGroupID uniqueidentifier ,
	@adminUserID int 

AS
SET NOCOUNT ON

-- security level check
IF ( SELECT [config].[GetSecurityLevel]() & 1920 ) = 0
	RETURN
	
DECLARE @oldPolicyID uniqueidentifier
SET @oldPolicyID = ( SELECT [PolicyID] FROM [relationship].[UserGroup_Client]  WHERE [UserGroupID] = @userGroupID AND [ClientID] = @securableID )

DECLARE @fileTable table (fileID bigint)
DECLARE @contTable table (contID bigint)
DECLARE @docTable table (docID bigint)

BEGIN TRY
	BEGIN TRANSACTION
		-- Update Client
		-- security level check
		IF ( SELECT [config].[GetSecurityLevel]() & 512 ) = 512
		BEGIN
			UPDATE [relationship].[UserGroup_Client] SET [PolicyID] = @policyID WHERE [PolicyID] = @oldPolicyID AND [UserGroupID] = @userGroupID AND [ClientID] = @securableID
			-- Audit the security event
			INSERT [audit].[UserGroup_Client] ( [Created] , [CreatedBy] , [Event] , [UserGroupID] , [ClientID] , [PolicyID] )
			VALUES ( GetUTCDate() , @adminUserID , 'UPDTSECCL' , @userGroupID , @securableID , @policyID )
		END
		
		-- Update Contact
		-- security level check
		IF ( SELECT [config].[GetSecurityLevel]() & 1024 ) = 1024
		BEGIN
			UPDATE X 	SET X.[PolicyID] = @policyID 
			OUTPUT Inserted.ContactID INTO @contTable		
			FROM [relationship].[UserGroup_Contact] X JOIN [dbo].[dbClientContacts] CC ON CC.contID = X.contactID
			WHERE X.[PolicyID] = @oldPolicyID AND X.[UserGroupID] = @userGroupID AND CC.[ClID] = @securableID
			-- Audit the security event
			INSERT [audit].[UserGroup_Contact] ( [Created] , [CreatedBy] , [Event] , [UserGroupID] , [ContactID] , [PolicyID] )
			SELECT GetUTCDate() , @adminUserID , 'UPDTSECCONT' ,  @userGroupID , contID , @policyID FROM @contTable
		END
				
		-- Update File
		-- security level check
		IF ( SELECT [config].[GetSecurityLevel]() & 256 ) = 256
		BEGIN
			UPDATE F 	SET F.[PolicyID] = @policyID
			OUTPUT Inserted.FileID INTO @fileTable
			FROM [relationship].[UserGroup_File] F JOIN dbo.dbFile FI ON F.fileID = FI.fileID
			WHERE F.[PolicyID] = @oldPolicyID AND F.[UserGroupID] = @userGroupID AND FI.[clID] = @securableID
			-- Audit the security event
			INSERT [audit].[UserGroup_File] (  [Created] , [CreatedBy] , [Event] , [UserGroupID] , [FileID] , [PolicyID] )
			SELECT GetUTCDate() , @adminUserID , 'UPDTSECFILE' ,  @userGroupID , [FileID] , @policyID FROM @fileTable
		END
			
			
		-- Update Document
		-- security level check
		IF ( SELECT [config].[GetSecurityLevel]() & 128 ) = 128
		BEGIN
			UPDATE D SET D.[PolicyID] = @policyID
			OUTPUT Inserted.DocumentID INTO @docTable
			FROM [relationship].[UserGroup_Document] D JOIN dbo.dbDocument DO ON DO.docID = D.DocumentID 
			WHERE D.[PolicyID] = @oldPolicyID AND D.[UserGroupID] = @userGroupID AND DO.[clID] = @securableID
			-- Audit the security event
			INSERT [audit].[UserGroup_Document] ( [Created] , [CreatedBy] , [Event] ,  [UserGroupID] , [DocumentID] , [PolicyID] )
			SELECT GetUTCDate() , @adminUserID , 'UPDTSECDOC' , @userGroupID , [DocID] , @policyID FROM @docTable
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
    ON OBJECT::[config].[UpdateClientSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[UpdateClientSecurity] TO [OMSAdminRole]
    AS [dbo];

