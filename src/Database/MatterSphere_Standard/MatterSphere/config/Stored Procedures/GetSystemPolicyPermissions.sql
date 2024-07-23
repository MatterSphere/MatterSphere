

CREATE PROCEDURE [config].[GetSystemPolicyPermissions]
	@policyTypeCode uCodeLookup ,
	@policyID uniqueidentifier = NULL

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

IF @policyID IS NULL
BEGIN
	SELECT 
		MTP.*
	FROM 
		[config].[SystemPolicy] SP 
	JOIN
		[config].[PolicyType] PT ON PT.[PolicyTypeCode] = SP.[Type]
	CROSS APPLY 
		[config].[SystemMaskToPermissions] ( SP.[AllowMask] , SP.[DenyMask] ) MTP 
	WHERE PT.[PolicyTypeCode] = @policyTypeCode
END
ELSE
BEGIN
	SELECT 
		MTP.*
	FROM 
		[config].[SystemPolicy] SP 
	JOIN
		[config].[PolicyType] PT ON PT.[PolicyTypeCode] = SP.[Type]
	CROSS APPLY 
		[config].[SystemMaskToPermissions] ( SP.[AllowMask] , SP.[DenyMask] ) MTP 
	WHERE 
		SP.[ID] = @policyID

END



GO
GRANT EXECUTE
    ON OBJECT::[config].[GetSystemPolicyPermissions] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetSystemPolicyPermissions] TO [OMSAdminRole]
    AS [dbo];

