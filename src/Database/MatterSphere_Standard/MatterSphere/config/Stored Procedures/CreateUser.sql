

CREATE PROCEDURE [config].[CreateUser]
	@name nvarchar(50) ,
	@ntLogin nvarchar(200) ,
	@active bit = 1 ,
	@adminUserID int ,
	@policyID uniqueidentifier

AS
SET NOCOUNT ON
DECLARE @log table
(
	[ID] uniqueidentifier
)


BEGIN TRY
	BEGIN TRANSACTION
		-- Create the security user
		INSERT [item].[User] ( [Name] , [NTLogin] , [Active] , [PolicyID])
		OUTPUT [Inserted].[ID] INTO @Log
		VALUES ( @name , @ntLogin , @active , @policyID )

		-- Audit the creation of a new security user
		INSERT [audit].[User] ( [Updated] , [UpdatedBy] , [Event] , [UserID] , [Name] , [NTLogin] , [Active] , [PolicyID] )
		SELECT GetUTCDate() , @adminUserID , 'NEWUSER' , [ID] , @name , @ntLogin , @active , @policyID  FROM @log
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
    ON OBJECT::[config].[CreateUser] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[CreateUser] TO [OMSAdminRole]
    AS [dbo];

