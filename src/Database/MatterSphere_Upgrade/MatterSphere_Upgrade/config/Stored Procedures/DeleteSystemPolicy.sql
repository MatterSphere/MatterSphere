


CREATE PROCEDURE [config].[DeleteSystemPolicy]
	@policyID uniqueidentifier


AS
DECLARE @er nvarchar(max)
SET @er = 'Cannot delete a Policy that is still in use'

-- Check if the policy is a default policy
IF ( SELECT [Name] FROM [config].[SystemPolicy] WHERE [ID] = @policyID ) LIKE '%DEFAULT'
BEGIN
	RAISERROR ('Cannot delete a Default policy' , 16 , 1 )
	RETURN
END

-- Check the policy is not on use with a user securable
IF EXISTS ( SELECT [PolicyID] FROM [item].[User] WHERE [PolicyID] = @policyID )
BEGIN
	RAISERROR (@er , 16 , 1 )
	RETURN
END

-- Check the policy is not on use with a user securable
IF EXISTS ( SELECT [PolicyID] FROM [item].[Group] WHERE [PolicyID] = @policyID )
BEGIN
	RAISERROR (@er , 16 , 1 )
	RETURN
END

DELETE [config].[PolicyType] WHERE PolicyTypeCode = (SELECT [Type] from [config].[SystemPolicy] WHERE ID = @policyID)
DELETE [config].[SystemPolicy] WHERE ID = @policyID

GO

GRANT EXECUTE
    ON OBJECT::[config].[DeleteSystemPolicy] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[DeleteSystemPolicy] TO [OMSAdminRole]
    AS [dbo];