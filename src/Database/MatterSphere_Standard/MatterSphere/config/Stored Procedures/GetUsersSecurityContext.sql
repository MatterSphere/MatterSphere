CREATE PROCEDURE [config].[GetUsersSecurityContext]
@ntLogin NVARCHAR (200)
AS 
SET NOCOUNT ON

SELECT 
	MTP.[SecurableType] ,
	MTP.[PermissionCode] as [Permission] ,
	NULL as ObjectID
FROM 
	[config].[SystemPolicy] SP 
JOIN
	[config].[GetUserAndGroupMembershipSupport] ( @ntLogin ) GM ON GM.[PolicyID] = SP.[ID] CROSS APPLY [config].[SystemMaskToPermissions] ( SP.[AllowMask] , SP.[DenyMask] ) MTP
GROUP BY 
	MTP.[PermissionCode] ,  MTP.[SecurableType] 
HAVING
	Sum([Allow]) = 0



GO
GRANT EXECUTE
    ON OBJECT::[config].[GetUsersSecurityContext] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetUsersSecurityContext] TO [OMSAdminRole]
    AS [dbo];

