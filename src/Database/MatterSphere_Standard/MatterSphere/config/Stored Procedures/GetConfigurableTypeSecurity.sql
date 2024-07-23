

CREATE PROCEDURE [config].[GetConfigurableTypeSecurity]
	@securableType uCodeLookup ,
	@configurableTypeCode uCodeLookup ,
	@ui uCodeLookup = '{default}'

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
DECLARE @nodeLevel  decimal(7,2)
--SET @nodeLevel = ( SELECT min([NodeLevel]) FROM [config].[ObjectPolicyConfig] WHERE [SecurableType] =  Replace ( @securableType , 'type' , '' ))

SELECT 
	UGM.[ID] as [ID] ,
	UGM.[Name] as GroupName,
	COALESCE(CL.cdDesc, '~' + NULLIF(MP.[SecurableType], '') + '~') as [SecurableType] ,
	MP.[Permission] as [Permission] ,
	Permission as PermissionCode ,
	MP.[Allow] ,
	MP.[Deny] ,
	MP.[Byte] ,
	MP.[BitValue] ,
	MP.[MajorType] ,
	MP.[NodeLevel] 
FROM
	[config].[GetUserAndGroupMembership]() UGM
JOIN
	[config].[ConfigurableTypePolicy_UserGroup] PUG ON PUG.[UserGroupID] = UGM.[ID]
JOIN
	[config].[ObjectPolicy] OP ON OP.[ID] = PUG.[PolicyID]
CROSS APPLY 
	[config].[MaskToPermissions] ( OP.[AllowMask] , OP.[DenyMask] ) MP
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( 'SECURABLE', '{default}' ) CL ON CL.[cdCode] = MP.[SecurableType]
WHERE
	PUG.[SecurableTypeCode] = @configurableTypeCode
AND
	PUG.[SecurableType] = @securableType
--AND
--	MP.[NodeLevel] >= @nodeLevel



GO
GRANT EXECUTE
    ON OBJECT::[config].[GetConfigurableTypeSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetConfigurableTypeSecurity] TO [OMSAdminRole]
    AS [dbo];

