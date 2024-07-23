

CREATE PROCEDURE [config].[DeleteUserFromGroup]
	@userID uniqueidentifier ,
	@groupID uniqueidentifier ,
	@adminUserID int

AS
SET NOCOUNT ON
DECLARE @log table
	(
	[GroupID] uniqueidentifier ,
	[UserID] uniqueidentifier 
	)

BEGIN TRY
	BEGIN TRANSACTION
		-- Delete the old relationship
		DELETE [relationship].[Group_User] 
		WHERE [GroupID] = @groupID AND [UserID] = @userID

		-- Audit the old relationship
		INSERT [audit].[Group_User] ( [Created] , [CreatedBy] , [Event] , [GroupID] , [UserID] )
		VALUES ( GetUTCDate() , @adminUserID , 'DELGROUPUSER' ,  @groupID , @userID )
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
    ON OBJECT::[config].[DeleteUserFromGroup] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[DeleteUserFromGroup] TO [OMSAdminRole]
    AS [dbo];

