

CREATE PROCEDURE [config].[ListUserAndGroup]
	@ui uCodeLookup = '{default}'

AS
SET NOCOUNT ON 
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT
	[ID] as [ID],
	COALESCE(CL.cdDesc, '~' +  NULLIF(G.[Name], '') + '~') as [UserGroup],
	[Active] as [Active]
FROM
	[config].[Group] G 
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( 'SECGROUPS', @ui ) CL ON CL.[cdCode] = G.[Name]
--
--UNION ALL
--	[ID] as [ID],
--	[Name] as [UserGroup],
--	[Active] as [Active],
--
--UNION ALL
--SELECT
--	NULL as [ID] 
--	'Everyone' as [UserGroup]
--	1 as [Active]



GO
GRANT EXECUTE
    ON OBJECT::[config].[ListUserAndGroup] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListUserAndGroup] TO [OMSAdminRole]
    AS [dbo];

