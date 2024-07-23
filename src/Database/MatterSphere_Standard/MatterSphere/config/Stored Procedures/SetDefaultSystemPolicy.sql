


CREATE PROCEDURE [config].[SetDefaultSystemPolicy] 
	@policyID uniqueidentifier
AS
SET NOCOUNT ON

BEGIN TRY
	
	BEGIN TRANSACTION
		UPDATE config.SystemPolicy SET isDefault = 0
		UPDATE config.SystemPolicy SET IsDefault = 1 WHERE ID = @policyID
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
    ON OBJECT::[config].[SetDefaultSystemPolicy] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[SetDefaultSystemPolicy] TO [OMSAdminRole]
    AS [dbo];

