CREATE PROCEDURE [config].[DeleteObjectPolicy]
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

-- Check the policy is not on use with the contact securable
IF EXISTS ( SELECT [PolicyID] FROM [config].[ConfigurableTypePolicy_UserGroup] WHERE [PolicyID] = @policyID )
BEGIN
	RAISERROR (@er , 16 , 1 )
	RETURN
END

DELETE [config].[PolicyType] WHERE PolicyTypeCode = (SELECT [Type] from [config].[ObjectPolicy] WHERE ID = @policyID)
DELETE [config].[ObjectPolicy] WHERE ID = @policyID
GO
