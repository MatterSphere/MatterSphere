

CREATE PROCEDURE [config].[ListPolicyPermissions] 
	@policyID uniqueidentifier

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @allowMask varbinary(16) , @denyMask varbinary(16)
SET @allowMask = ( SELECT [AllowMask] FROM [config].[ObjectPolicy] WHERE [ID] = @policyID )
SET @denyMask = ( SELECT [DenyMask] FROM [config].[ObjectPolicy] WHERE [ID] = @policyID )
	SELECT
		[dbo].[GetCodeLookupDesc] ( 'SECURABLE' , [SecurableType] , '{default}' ) as [SecurableType] ,
		[dbo].[GetCodeLookupDesc] ( 'SECURABLE' , [SecurableType] , '{default}' )  + ' ' + [dbo].[GetCodeLookupDesc] ( 'PERMISSION' , [Permission] , '{default}' ) as [Permission] ,
		CASE WHEN Substring ( @allowMask , [Byte] , 1 ) & [BitValue] = [BitValue] THEN 1 ELSE 0 END as [Allow] ,
		CASE WHEN Substring ( @denyMask , [Byte] , 1 ) & [BitValue] = [BitValue] THEN 1 ELSE 0 END as [Deny] ,
		[MajorType] ,
		[Byte] ,
		[BitValue] 
	FROM
		[config].[PolicyConfig] 
	WHERE 
		SecurableType IS NOT NULL 
	AND
		[Permission] IS NOT NULL



GO
GRANT EXECUTE
    ON OBJECT::[config].[ListPolicyPermissions] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListPolicyPermissions] TO [OMSAdminRole]
    AS [dbo];

