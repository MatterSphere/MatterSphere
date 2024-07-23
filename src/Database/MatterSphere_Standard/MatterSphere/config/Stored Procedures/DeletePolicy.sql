

CREATE PROCEDURE [config].[DeletePolicy]
	@policyID uniqueidentifier

AS
DECLARE @er nvarchar(max)
SET @er = 'Cannot delete a Policy that is still in use'

-- Check if the policy is a default policy
IF ( SELECT [Name] FROM [config].[ObjectPolicy] WHERE [ID] = @policyID ) LIKE '%DEFAULT'
BEGIN
	RAISERROR ('Cannot delete a Default policy' , 16 , 1 )
	RETURN
END

-- Check the policy is not on use with the client securable
IF EXISTS ( SELECT [PolicyID] FROM [relationship].[UserGroup_Client] WHERE [PolicyID] = @policyID )
BEGIN
	RAISERROR (@er , 16 , 1 )
	RETURN
END

-- Check the policy is not on use with the file securable
IF EXISTS ( SELECT [PolicyID] FROM [relationship].[UserGroup_File] WHERE [PolicyID] = @policyID )
BEGIN
	RAISERROR (@er , 16 , 1 )
	RETURN
END

-- Check the policy is not on use with the document securable
IF EXISTS ( SELECT [PolicyID] FROM [relationship].[UserGroup_Document] WHERE [PolicyID] = @policyID )
BEGIN
	RAISERROR (@er , 16 , 1 )
	RETURN
END

-- Check the policy is not on use with the contact securable
IF EXISTS ( SELECT [PolicyID] FROM [relationship].[UserGroup_Contact] WHERE [PolicyID] = @policyID )
BEGIN
	RAISERROR (@er , 16 , 1 )
	RETURN
END



GO
GRANT EXECUTE
    ON OBJECT::[config].[DeletePolicy] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[DeletePolicy] TO [OMSAdminRole]
    AS [dbo];

