

CREATE VIEW [external].[ContactAccessDenies]
AS
		SELECT
			RUGC.[ContactID] as ID
		FROM
			[relationship].[UserGroup_Contact] RUGC
		JOIN
			[config].[ObjectPolicy] PC ON PC.ID = RUGC.[PolicyID]
		JOIN
			[config].[GetUserAndGroupMembershipNT] (config.GetUserLogin()) UGM ON RUGC.[UserGroupID] = UGM.[ID]
		WHERE
			(PC.IsRemote = 1 or PC.[Type] = 'EXPLICITOBJ') AND Substring ( PC.DenyMask , 10 , 1 ) & 128 = 128

GO
GRANT UPDATE
    ON OBJECT::[external].[ContactAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[ContactAccessDenies] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[ContactAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[ContactAccessDenies] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[ContactAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[ContactAccessDenies] TO [OMSApplicationRole]
    AS [dbo];

