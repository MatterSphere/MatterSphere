

CREATE PROCEDURE [config].[GetObjectPolicy]
	@id uniqueidentifier
	
AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT [ID] , [Type] , [AllowMask] , [DenyMask] , [Name] FROM [config].[ObjectPolicy] WHERE [ID] = @id



GO
GRANT EXECUTE
    ON OBJECT::[config].[GetObjectPolicy] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetObjectPolicy] TO [OMSAdminRole]
    AS [dbo];

