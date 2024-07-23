

CREATE PROCEDURE [config].[ApplyContactSecurity]
	@policyID uniqueidentifier ,
	@userGroupID uniqueidentifier ,
	@securableTypeID bigint ,
	@adminUserID int


AS
SET NOCOUNT ON
SET ANSI_WARNINGS OFF

-- security level check
IF (SELECT [config].[GetSecurityLevel] () & 1024 ) = 0 
	RETURN

DECLARE @CLID bigint
select @clid = clid from [dbo].[dbClientContacts] where Contid = @securableTypeID
	
	
BEGIN TRY
	BEGIN TRANSACTION
	IF NOT EXISTS (SELECT 1 FROM [relationship].[UserGroup_Contact] WHERE UserGroupID = @userGroupID and ContactID = @securableTypeID)
	begin
		-- Create the contact security record
		INSERT [relationship].[UserGroup_Contact] ( [UserGroupID] , [ContactID] , [PolicyID], [CLID] )
		VALUES ( @userGroupID , @securableTypeID , @policyID, @clID )

		-- Audit the security event
		INSERT [audit].[UserGroup_Contact] ( [Created] , [CreatedBy] , [Event] , [UserGroupID] , [ContactID] , [PolicyID] )
		VALUES ( GetUTCDate() , @adminUserID , 'NEWSECCONT' ,  @userGroupID , @securableTypeID , @policyID )
	end
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
    ON OBJECT::[config].[ApplyContactSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ApplyContactSecurity] TO [OMSAdminRole]
    AS [dbo];

