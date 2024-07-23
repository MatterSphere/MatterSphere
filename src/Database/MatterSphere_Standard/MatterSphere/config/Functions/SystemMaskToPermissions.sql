

CREATE FUNCTION [config].[SystemMaskToPermissions] ( @allowMask binary(32) , @denyMask binary(32) )
RETURNS table

RETURN
(
	SELECT
		OPC.[SecurableType] ,
		COALESCE(GCL.cdDesc, '~' + NULLIF(OPC.Permission, '') + '~') as Permission ,
		Permission as PermissionCode ,
--		GCL.[cdDesc] ,
		CASE WHEN Substring ( @allowMask , OPC.[Byte] , 1 ) & OPC.[BitValue] = OPC.[BitValue] THEN 1 ELSE 0 END as [Allow] ,
		CASE WHEN Substring ( @denyMask , OPC.[Byte] , 1 ) & OPC.[BitValue] = OPC.[BitValue] THEN 1 ELSE 0 END as [Deny] ,
		OPC.[Byte],
		OPC.[BitValue],
		OPC.[MajorType],
		OPC.[NodeLevel]
	FROM
		[config].[SystemPolicyConfig] OPC
	LEFT JOIN
		[dbo].[GetCodeLookupDescription] ( 'PERMISSION', '{default}') GCL ON GCL.[cdCode] = OPC.[Permission]
	WHERE 
		SecurableType IS NOT NULL 
	AND
		[Permission] IS NOT NULL
)



GO
GRANT UPDATE
    ON OBJECT::[config].[SystemMaskToPermissions] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[SystemMaskToPermissions] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[SystemMaskToPermissions] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[SystemMaskToPermissions] TO [OMSApplicationRole]
    AS [dbo];

