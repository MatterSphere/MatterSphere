

CREATE PROCEDURE [config].[CreatePolicyTemplate]
	@allowMask nchar(34) ,
	@denyMask nchar(34) ,
	@name nvarchar(100) = NULL ,
	@policyTypeCode uCodeLookup ,
	@isSystemPolicy bit ,
	@usrID int ,
	@id uniqueidentifier ,
	@IsRemote bit = 0


AS
SET NOCOUNT ON
SET XACT_ABORT ON
DECLARE @allowMaskBin binary(34) , @denyMaskBin binary(34) , @sql nvarchar(400)
SET @sql = 'SELECT @allowMaskBin = ' + @allowMask + ' , @denyMaskBin = ' + @denyMask

EXECUTE sp_executesql @sql , N'@allowMaskBin varbinary(34) OUTPUT, @denyMaskBin varbinary(34) OUTPUT' , @allowMaskBin OUTPUT , @denyMaskBin OUTPUT


BEGIN TRY
BEGIN TRANSACTION
	-- Create the policy
	IF NOT EXISTS ( SELECT [PolicyTypeCode] FROM [config].[PolicyType] WHERE [PolicyTypeCode] = @policyTypeCode )
	BEGIN
		INSERT [config].[PolicyType] ( PolicyTypeCode , CanEdit , CanDelete , Created , CreatedBy , IsSystemPolicy , IncludeInFilters )
		VALUES ( @policyTypeCode , 1 , 1 , GetUTCDate() , @usrID , @isSystemPolicy , 1 )
	END
	-- Create the policy template
	IF @isSystemPolicy = 0
	BEGIN
		INSERT [config].[ObjectPolicy] ( [ID] , [Type] , [AllowMask] , [DenyMask] , [Name], [IsRemote] )
		VALUES ( @id , @policyTypeCode , @allowMaskBin , @denyMaskBin , @name, @IsRemote )
	END
	ELSE
		BEGIN
		INSERT [config].[SystemPolicy] ( [ID] , [Type] , [AllowMask] , [DenyMask] , [Name] )
		VALUES ( @id , @policyTypeCode , @allowMaskBin , @denyMaskBin , @name )
	END
	-- Now Audit the event
	INSERT [audit].[PolicyTemplate] ( [PolicyID] , [PolicyTypeCode] , [AllowMask] , [DenyMask] , [Name] , [Updated] , [UpdatedBy] , [AuditEvent], [IsRemote] )
	VALUES ( @id , @policyTypeCode , @allowMaskBin , @denyMaskBIn , @name , GetUTCDate() , @usrID , 'Policy Template Created', @IsRemote )
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
    ON OBJECT::[config].[CreatePolicyTemplate] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[CreatePolicyTemplate] TO [OMSAdminRole]
    AS [dbo];

