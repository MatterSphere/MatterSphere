

CREATE PROCEDURE [config].[ListConfigurableTypeUserGroups]
	@securableType uCodeLookup ,
	@configurableTypeCode uCodeLookup ,
	@ui uCodeLookup = '{default}'
	,@AccessType nvarchar(15) ='INTERNAL'

AS
SET NOCOUNT ON
IF  EXISTS (
SELECT 1
FROM
	[config].[PolicyType] PT
JOIN
	[config].[ObjectPolicy] OP ON OP.[Type] = PT.[PolicyTypeCode]
JOIN
	[config].[ConfigurableTypePolicy_UserGroup] PUG ON PUG.[PolicyID] = OP.[ID]
WHERE
	PUG.[SecurableTypeCode] = @configurableTypeCode
AND
	PUG.[SecurableType] = @securableType
	--AND U.AccessType = @AccessType
)

BEGIN
SELECT 
	CASE WHEN U.[ID] IS NULL THEN 51 ELSE 52 END as [ImageIndex] ,
	OP.[ID] as [Policy] ,
	CASE WHEN U.[ID] IS NULL THEN G.[ID] ELSE U.[ID] END as [GroupID] ,
	CASE WHEN U.[ID] IS NULL THEN G.[Name] ELSE U.[Name] END as [GroupName] ,
	CASE WHEN U.[ID] IS NULL THEN COALESCE(CL.cdDesc, '~' + NULLIF(G.[Name], '') + '~') ELSE U.[Name] END as [GroupNameDesc]
	,U.AccessType
	,null as [SelectedMattersOnly]
	,null as [Inherited]
	,CASE WHEN U.[ID] IS NULL THEN G.[Active] ELSE U.[Active] END as [Active]
FROM
	[config].[PolicyType] PT
JOIN
	[config].[ObjectPolicy] OP ON OP.[Type] = PT.[PolicyTypeCode]
JOIN
	[config].[ConfigurableTypePolicy_UserGroup] PUG ON PUG.[PolicyID] = OP.[ID]
LEFT JOIN
		( SELECT X.ID, X.Active, R.[usrFullname] as [Name], R.AccessType FROM [item].[User] X 	
		  JOIN [dbo].[dbUser] R ON R.usrADID = X.NTLogin 
		) U ON U.[ID] = PUG.[UserGroupID]                
LEFT JOIN
	[item].[Group] G ON G.[ID] = PUG.[UserGroupID]       
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( 'SECGROUPS', @ui ) CL ON CL.[cdCode] = G.[Name]
WHERE
	PUG.[SecurableTypeCode] = @configurableTypeCode
AND
	PUG.[SecurableType] = @securableType
	--AND U.AccessType = @AccessType 
END
ELSE
BEGIN
SELECT
	51 as [ImageIndex] ,
	( SELECT [ID] FROM [config].[ObjectPolicy] WHERE [Type] = 'GLOBALOBJDEF' ) as [Policy],
	newid() as [GroupID], 
	dbo.GetCodeLookupDesc('SECGROUPS' , 'EVERYONE' , @ui )  as [GroupName], 
	'Internal Users' as [GroupNameDesc] 
	,'' as [AccessType]
	,null as [SelectedMattersOnly]
	,null as [Inherited]
	,null as [Active]
END


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListConfigurableTypeUserGroups] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListConfigurableTypeUserGroups] TO [OMSAdminRole]
    AS [dbo];

