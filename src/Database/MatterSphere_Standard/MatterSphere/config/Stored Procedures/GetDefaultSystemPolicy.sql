

CREATE PROCEDURE [config].[GetDefaultSystemPolicy] 

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED


SELECT 
	ISNULL([ID], '3cc3bd00-7d7e-4d4a-96c6-44e44e140c5e') as [SystemPolicyID]
	, ISNULL([Type], 'GLOBALSYSDEF') as [SystemPolicyCode]
FROM 
	[config].[SystemPolicy]
WHERE
	IsDefault = 1

SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[config].[GetDefaultSystemPolicy] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetDefaultSystemPolicy] TO [OMSAdminRole]
    AS [dbo];

