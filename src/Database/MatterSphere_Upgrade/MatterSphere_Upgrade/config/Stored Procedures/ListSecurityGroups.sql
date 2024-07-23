
CREATE PROCEDURE [config].[ListSecurityGroups]
 @ui uCodeLookup = '{default}'

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT 
	G.[ID] , COALESCE(CL.cdDesc, '~' +  NULLIF(G.[Name], '') + '~') as [Name] , G.[Description] , G.[Active]
	, case [ADDistinguishedName] when ' ' then 'Local' else 'AD' end as GroupType
	, P.[Name] as [Policy]
FROM
	[item].[Group] G 
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( 'SECGROUPS', @ui ) CL ON CL.[cdCode] = G.[Name]
LEFT JOIN
	[config].[SystemPolicy] P ON P.[ID] = G.PolicyID
WHERE G.[Name] <> 'Everyone'


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListSecurityGroups] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListSecurityGroups] TO [OMSAdminRole]
    AS [dbo];

