

CREATE PROCEDURE [config].[UpdateUser]
	@id uniqueidentifier ,
	@ntLogin nvarchar(200) ,
	@name nvarchar(50),
	@active bit ,	
	@policyID uniqueidentifier ,
	@adminUserID int

AS

SET NOCOUNT ON

BEGIN TRY
BEGIN TRANSACTION
	-- Update the user record
	UPDATE [item].[User] 
	SET [NTLogin] = @ntLogin , [Name] = @name , [Active] = @active , [PolicyID] = @policyID WHERE [ID] = @id
	-- Now audit the event
	INSERT [audit].[User] ( [Updated] , [UpdatedBy] , [Event] ,  [UserID] , [Name] , [NTLogin] , [Active] , [PolicyID] )
	VALUES ( GetUTCDate() , @adminUserID , 'UPDTUSER' , @id , @name , @ntLogin , @active , @policyID )
COMMIT TRANSACTION
END TRY
BEGIN CATCH
	IF @@TranCount <> 0
		ROLLBACK TRANSACTION
	DECLARE @err nvarchar(max)
	SET @err = ERROR_MESSAGE()
	RAISERROR ( @err , 16 , 1 )
END CATCH



GO
GRANT EXECUTE
    ON OBJECT::[config].[UpdateUser] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[UpdateUser] TO [OMSAdminRole]
    AS [dbo];

