

CREATE PROCEDURE [config].[AddUserToGroup]
	@userID uniqueidentifier ,
	@groupID uniqueidentifier ,
	@adminUserID int


AS
SET NOCOUNT ON

BEGIN TRY
	BEGIN TRANSACTION
		-- Create the new relationship
		INSERT [relationship].[Group_User] ( [GroupID] , [UserID] )
		VALUES ( @groupID , @userID )

		-- Audit the new relationship
		INSERT [audit].[Group_User] ( [Created] , [CreatedBy] , [Event] , [GroupID] , [UserID] )
		VALUES ( GetUTCDate() , @adminUserID , 'NEWGROUPUSER' ,  @groupID , @userID )
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
    ON OBJECT::[config].[AddUserToGroup] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[AddUserToGroup] TO [OMSAdminRole]
    AS [dbo];

