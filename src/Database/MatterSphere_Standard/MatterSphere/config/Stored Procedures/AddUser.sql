

CREATE PROCEDURE [config].[AddUser]
	@userID int,
	@adminUserID int,
	@ntLogin varchar(200),
	@name varchar(50),
	@active bit
	

AS
SET NOCOUNT ON

BEGIN TRY
BEGIN TRANSACTION
	DECLARE @policyID uniqueidentifier
	SET @policyID = ( SELECT ID FROM config.SystemPolicy WHERE [Type] = 'GLOBALSYSDEF' )
	IF @policyID is NULL 
	BEGIN 
	IF @@TRANCOUNT <> 0
		ROLLBACK TRANSACTION
	RETURN
	END 
		
	DECLARE @table table ( ID uniqueidentifier )
	
	if not exists (SELECT 1 FROM item.[User] WHERE NTLogin = @ntLogin)
	begin
		INSERT [item].[User] ( NTLogin , [Name] , Active , PolicyID )
		OUTPUT inserted.ID INTO @table
		VALUES ( @ntLogin , @name , @active , @policyID )
	
		INSERT [audit].[User] ( Updated , UpdatedBy , [Event] , UserID , [Name] , NTLogin , Active ,  PolicyID )
		SELECT getdate() , @adminUserID , 'NEWSECUSER' , ID , @name , @ntLogin , @active , @policyID FROM @table
	end
	
	UPDATE [dbo].[dbUser] set SecurityID = (SELECT ID FROM item.[User] WHERE NTLogin = @ntLogin) WHERE usrID = @userID



COMMIT TRANSACTION
END TRY

BEGIN CATCH
IF @@TRANCOUNT <> 0
	ROLLBACK TRANSACTION
RAISERROR ( 'Cannot create user' , 16 , 1)
END CATCH

GO
GRANT EXECUTE
    ON OBJECT::[config].[AddUser] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[AddUser] TO [OMSAdminRole]
    AS [dbo];

