CREATE PROCEDURE [config].[GetDocumentSecurity]
	@documentID bigint ,
	@ui uCodeLookup = '{default}'

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT DISTINCT
	UGM.[ID] as [ID] ,
	UGM.[Name] as GroupName,
	COALESCE(CL.cdDesc, '~' + ISNULL(MP.[SecurableType], '') + '~') as [SecurableType] ,
	MP.[Permission] as [Permission] ,
	Permission as PermissionCode ,
	MP.[Allow] ,
	MP.[Deny] ,
	MP.[Byte] ,
	MP.[BitValue] ,
	MP.[MajorType] ,
	MP.[NodeLevel] ,
	CASE PT.[Type] WHEN 'EXPLICITOBJ' THEN NULL ELSE UGD.[PolicyID] END as [PolicyID]
FROM
	[config].[GetUserAndGroupMembership]() UGM
JOIN
	[relationship].[UserGroup_Document] UGD ON UGD.[UserGroupID] = UGM.[ID]
JOIN 
	[config].[ObjectPolicy] PT ON PT.[ID] = UGD.[PolicyID]
CROSS APPLY 
	[config].[MaskToPermissions] ( PT.[AllowMask] , PT.[DenyMask] ) MP
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( 'SECURABLE', @ui ) CL ON CL.[cdCode] = MP.[SecurableType]


WHERE 
	UGD.[DocumentID] = @documentID
ORDER BY 
	MP.NodeLevel
