


CREATE FUNCTION [config].[CheckIsAdministrator] (@USER nvarchar(100))
RETURNS table

RETURN
(

		SELECT SMP.[Allow] , SMP.[Deny],GMT.Name FROM 
			( SELECT PolicyID,[Name] FROM [config].[GetUserAndGroupMembershipNT] ( @USER ) ) GMT 
		JOIN
			[config].[SystemPolicy] SP ON SP.[ID] = GMT.PolicyID 
		CROSS APPLY 
			[config].[SystemMaskToPermissions] ( SP.AllowMask , SP.DenyMask ) SMP
		WHERE 
			SMP.Byte = 2 AND SMP.BitValue = 8

)



GO
GRANT UPDATE
    ON OBJECT::[config].[CheckIsAdministrator] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[CheckIsAdministrator] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[CheckIsAdministrator] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[CheckIsAdministrator] TO [OMSApplicationRole]
    AS [dbo];

