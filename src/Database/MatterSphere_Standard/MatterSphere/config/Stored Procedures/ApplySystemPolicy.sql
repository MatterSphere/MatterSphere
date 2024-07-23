

CREATE PROCEDURE [config].[ApplySystemPolicy]
	@policyID uniqueidentifier = NULL OUTPUT,
	@allowMask nchar(34) ,
	@denyMask nchar(34) ,
	@userID int = NULL ,
	@groupID uniqueidentifier = NULL ,
	@adminUserID int


AS
SET NOCOUNT ON
SET XACT_ABORT ON
DECLARE @output table
	(
	[ID] uniqueidentifier
	)
DECLARE @allowMaskBin binary(34) , @denyMaskBin binary(34) , @sql nvarchar(400)
SET @sql = 'SELECT @allowMaskBin = ' + @allowMask + ' , @denyMaskBin = ' + @denyMask

EXECUTE sp_executesql @sql , N'@allowMaskBin varbinary(34) OUTPUT, @denyMaskBin varbinary(34) OUTPUT' , @allowMaskBin OUTPUT , @denyMaskBin OUTPUT


-- If the policy is an Explicit one create the policy
IF @policyID IS NULL
BEGIN
	INSERT [config].[SystemPolicy] ( [Type] , [AllowMask] , [DenyMask]  )
	OUTPUT Inserted.[ID] INTO @output
	VALUES ( 'EXPLICITSYS' , @allowMaskBin , @denyMaskBin)
	SET @policyID = ( SELECT [ID] FROM @output )
END

BEGIN TRY
BEGIN TRANSACTION
	IF @userID IS NOT NULL
	BEGIN
		-- Update the user record
		UPDATE IU 
		SET IU.[PolicyID] = @policyID 
		FROM [item].[User] IU JOIN [dbo].[dbUser] U ON U.[usrADID] = IU.[NTLogin] WHERE U.[usrID] = @userID
		-- Now audit the event
		INSERT [audit].[User] ( [Updated] , [UpdatedBy] , [Event] ,  [UserID] , [Name] , [NTLogin] , [Active] , [PolicyID] )
		SELECT GetUTCDate() , @adminUserID , 'UPDTUSER' , [ID] , [Name] , [NTLogin] , [Active] , @policyID FROM [item].[User] IU JOIN [dbo].[dbUser] U ON U.usrADID = IU.NTLogin
		WHERE U.[usrID] = @userID
	END

	ELSE IF @groupID IS NOT NULL
	BEGIN
		-- Update the group record
		UPDATE [item].[Group] 
		SET [PolicyID] = @policyID WHERE [ID] = @groupID
		-- Now audit the event
		INSERT [audit].[Group] ( [Updated] , [UpdatedBy] , [Event] ,  [GroupID] , [Name] , [Description] , [Active] , [PolicyID] )
		SELECT GetUTCDate() , @adminUserID , 'UPDTGRP' , @groupID , [Name] , [Description] , [Active] , @policyID FROM [item].[Group] WHERE [ID] = @groupID
	END
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
    ON OBJECT::[config].[ApplySystemPolicy] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ApplySystemPolicy] TO [OMSAdminRole]
    AS [dbo];

