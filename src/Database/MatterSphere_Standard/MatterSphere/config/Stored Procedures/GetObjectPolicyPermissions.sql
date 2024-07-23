

CREATE PROCEDURE [config].[GetObjectPolicyPermissions]
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
		[config].[ObjectPolicy] OP 
	JOIN
		[config].[PolicyType] PT ON PT.[PolicyTypeCode] = OP.[Type]
	CROSS APPLY 
		[config].[MaskToPermissions] ( OP.[AllowMask] , OP.[DenyMask] ) MTP WHERE PT.[PolicyTypeCode] = @policyTypeCode
END
ELSE
BEGIN
	SELECT 
		MTP.* 
	FROM 
		[config].[ObjectPolicy] OP 
	JOIN
		[config].[PolicyType] PT ON PT.[PolicyTypeCode] = OP.[Type]
	CROSS APPLY 
		[config].[MaskToPermissions] ( OP.[AllowMask] , OP.[DenyMask] ) MTP WHERE OP.[ID] = @policyID
END



GO
GRANT EXECUTE
    ON OBJECT::[config].[GetObjectPolicyPermissions] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetObjectPolicyPermissions] TO [OMSAdminRole]
    AS [dbo];

