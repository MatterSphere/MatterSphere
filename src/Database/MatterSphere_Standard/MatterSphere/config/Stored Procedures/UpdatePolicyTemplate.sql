

CREATE PROCEDURE [config].[UpdatePolicyTemplate]
	@denyMask nchar(34) = NULL ,
	@allowMask nchar(34) = NULL ,
	@name nvarchar(100) = NULL ,
	@policyTypeCode uCodeLookup ,
	@isSystemPolicy bit ,
	@usrID int ,
	@id uniqueidentifier  ,
	@IsRemote bit = 0

AS
SET NOCOUNT ON
SET XACT_ABORT ON

-- Taken out at DTs request
--IF @policyTypeCode IN ( 'GLOBALOBJDEF' , 'GLOBALSYSDEF' )
--BEGIN
	--RAISERROR ( 'Cannot edit the global security groups' , 25 , 1 )
	--RETURN
--END

DECLARE  @allowMaskBin binary(34) , @denyMaskBin binary(34) , @sql nvarchar(400)
SET @sql = 'SELECT @allowMaskBin = ' + @allowMask + ' , @denyMaskBin = ' + @denyMask
EXECUTE sp_executesql @sql , N'@allowMaskBin varbinary(34) OUTPUT, @denyMaskBin varbinary(34) OUTPUT' , @allowMaskBin OUTPUT , @denyMaskBin OUTPUT

BEGIN TRY
BEGIN TRANSACTION
	-- Update the policy template
	IF @isSystemPolicy = 0
	BEGIN
		UPDATE [config].[ObjectPolicy]
		SET [AllowMask] = @allowMaskBin , [DenyMask] = @denyMaskBin, [IsRemote] = @IsRemote WHERE [ID] = @id
	END
	ELSE
		BEGIN
		UPDATE [config].[SystemPolicy]
		SET [AllowMask] = @allowMaskBin , [DenyMask] = @denyMaskBin WHERE [ID] = @id
	END
	-- Now Audit the event
	INSERT [audit].[PolicyTemplate] ( [PolicyID] , [PolicyTypeCode] , [AllowMask] , [DenyMask] , [Name] , [Updated] , [UpdatedBy] , [AuditEvent], [IsRemote] )
	VALUES ( @id , @policyTypeCode , @allowMaskBin , @denyMaskBin , @name , GetUTCDate() , @usrID , 'Policy Template Updated', @IsRemote )
COMMIT TRANSACTION
END TRY
BEGIN CATCH
	IF @@TranCount <> 0
		ROLLBACK TRANSACTION
	DECLARE @err nvarchar(max)
	SET @err = ERROR_MESSAGE()
	RAISERROR ( @err , 16 ,1 )
END CATCH



GO
GRANT EXECUTE
    ON OBJECT::[config].[UpdatePolicyTemplate] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[UpdatePolicyTemplate] TO [OMSAdminRole]
    AS [dbo];

