

CREATE PROCEDURE [config].[ListSystemPolicy]

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT [ID] , [Name] FROM [config].[SystemPolicy]



GO
GRANT EXECUTE
    ON OBJECT::[config].[ListSystemPolicy] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListSystemPolicy] TO [OMSAdminRole]
    AS [dbo];

