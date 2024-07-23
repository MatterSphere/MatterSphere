

CREATE PROCEDURE [config].[ListSecurityUsers]

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT 
	U.[Name] , U.[NTLogin] , U.[Active] , P.[Name] as Policy
FROM
	[item].[User] U
JOIN
	[config].[SystemPolicy] P ON P.[ID] = U.[PolicyID]



GO
GRANT EXECUTE
    ON OBJECT::[config].[ListSecurityUsers] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListSecurityUsers] TO [OMSAdminRole]
    AS [dbo];

