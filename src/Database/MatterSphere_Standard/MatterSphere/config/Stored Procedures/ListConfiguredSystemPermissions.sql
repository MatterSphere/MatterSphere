

CREATE PROCEDURE [config].[ListConfiguredSystemPermissions]

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT  [Number] , [SecurableType] , [Permission] , [MajorType] , [NodeLevel] FROM [config].[SystemPolicyConfig] WHERE [NodeLevel] IS NOT NULL ORDER BY [NodeLevel]



GO
GRANT EXECUTE
    ON OBJECT::[config].[ListConfiguredSystemPermissions] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListConfiguredSystemPermissions] TO [OMSAdminRole]
    AS [dbo];

