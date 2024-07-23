


CREATE PROCEDURE [config].[IsSystemPolicyAssigned] 
	@policyID uniqueidentifier 
AS

DECLARE @assigned bit 
SET @assigned = 0

IF EXISTS ( SELECT [PolicyID] FROM [item].[User] WHERE [PolicyID] = @policyID ) 
	OR EXISTS ( SELECT [PolicyID] FROM [item].[Group] WHERE [PolicyID] = @policyID )
BEGIN
	SET @assigned = 1
END

SELECT @assigned
GO
GRANT EXECUTE
    ON OBJECT::[config].[IsSystemPolicyAssigned] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[IsSystemPolicyAssigned] TO [OMSAdminRole]
    AS [dbo];

