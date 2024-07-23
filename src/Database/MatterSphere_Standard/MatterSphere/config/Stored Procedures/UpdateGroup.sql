

CREATE PROCEDURE [config].[UpdateGroup]
	@id uniqueidentifier ,
	@description nvarchar(200) ,
	@name uCodeLookup,
	@active bit ,	
	@policyID uniqueidentifier ,
	@adminUserID int
	
AS

SET NOCOUNT ON

BEGIN TRY
BEGIN TRANSACTION
	-- Update the group record
	UPDATE [item].[Group] 
	SET [Description] = @description , [Name] = @name , [Active] = @active , [PolicyID] = @policyID WHERE [ID] = @id
	-- Now audit the event
	INSERT [audit].[Group] ( [Updated] , [UpdatedBy] , [Event] ,  [GroupID] , [Name] , [Description] , [Active] , [PolicyID] )
	VALUES ( GetUTCDate() , @adminUserID , 'UPDTGROUP' , @id , @name , @description , @active , @policyID )
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
    ON OBJECT::[config].[UpdateGroup] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[UpdateGroup] TO [OMSAdminRole]
    AS [dbo];

