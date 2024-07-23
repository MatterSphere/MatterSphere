

CREATE VIEW [external].[ClientAccessDenies]
AS
		SELECT 
			RUGC.[ClientID] as ID			
		FROM
			[relationship].[UserGroup_Client] RUGC
		JOIN
			[config].[ObjectPolicy] PC ON PC.ID = RUGC.[PolicyID]
		JOIN
			[config].[GetUserAndGroupMembershipNT] (config.GetUserLogin()) UGM ON RUGC.[UserGroupID] = UGM.[ID]
		WHERE 
			(PC.IsRemote = 1 or PC.[Type] = 'EXPLICITOBJ') AND Substring ( PC.DenyMask , 5 , 1 ) & 128 = 128

GO
GRANT UPDATE
    ON OBJECT::[external].[ClientAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[ClientAccessDenies] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[ClientAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[ClientAccessDenies] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[ClientAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[ClientAccessDenies] TO [OMSApplicationRole]
    AS [dbo];

