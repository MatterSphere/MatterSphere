

CREATE PROCEDURE [config].[ListConfiguredObjectPermissions]

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT  [Number] , [SecurableType] , [Permission] , [MajorType] , [NodeLevel] FROM [config].[ObjectPolicyConfig] WHERE [NodeLevel] IS NOT NULL ORDER BY [NodeLevel]



GO
GRANT EXECUTE
    ON OBJECT::[config].[ListConfiguredObjectPermissions] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListConfiguredObjectPermissions] TO [OMSAdminRole]
    AS [dbo];

