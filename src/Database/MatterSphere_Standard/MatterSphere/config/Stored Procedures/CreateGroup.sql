

CREATE PROCEDURE [config].[CreateGroup]
	@name uCodeLookup ,
	@description nvarchar(200) ,
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
		-- Create the security Group
		INSERT [item].[Group] ( [Name] , [Description] , [Active] , [PolicyID] )
		OUTPUT [Inserted].[ID] INTO @log
		VALUES ( @name , @description , @active , @policyID )

		-- Audit the creation of a new security group
		INSERT [audit].[Group] ( [Updated] , [UpdatedBy] , [Event] ,  [GroupID] , [Name] , [Description] , [Active] , [PolicyID] )
		SELECT GetUTCDate() , @adminUserID , 'NEWGROUP' , [ID] , @name , @description , @active , @policyID FROM @log
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
    ON OBJECT::[config].[CreateGroup] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[CreateGroup] TO [OMSAdminRole]
    AS [dbo];

