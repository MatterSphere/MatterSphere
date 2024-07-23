CREATE FUNCTION [config].[IsPIAdministrator_NS] ( )
RETURNS bit

AS
BEGIN
	DECLARE @isAdmin bit;
 
	WITH ResultantSecurity ( [Allow] , [Deny] ) as 
		( 
		SELECT SMP.[Allow] , SMP.[Deny] FROM 
			( SELECT PolicyID FROM [config].[GetUserAndGroupMembershipNT_NS] () GROUP BY PolicyID ) GMT 
		JOIN
			[config].[SystemPolicy] SP ON SP.[ID] = GMT.PolicyID 
		CROSS APPLY 
			[config].[SystemMaskToPermissions] ( SP.AllowMask , SP.DenyMask ) SMP
		WHERE 
			SMP.PermissionCode = 'PIIADMIN'
		)

		SELECT @isAdmin = CASE WHEN ( SELECT Sum([Deny]) FROM ResultantSecurity) = 0  AND ( SELECT Sum([Allow]) FROM ResultantSecurity ) > 0 
		THEN 1	ELSE 0 END

	RETURN @isAdmin
END

GO
