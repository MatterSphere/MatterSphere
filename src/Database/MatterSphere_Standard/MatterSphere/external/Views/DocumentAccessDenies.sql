

CREATE VIEW [external].[DocumentAccessDenies]
AS
		SELECT
			RUGD.DocumentID as id
		FROM
			[relationship].[UserGroup_Document] RUGD
		JOIN
			[config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID]
		JOIN
			[config].[GetUserAndGroupMembershipNT] (config.GetUserLogin()) UGM ON RUGD.[UserGroupID] = UGM.[ID]
		WHERE
			(PC.IsRemote = 1 or PC.[Type] = 'EXPLICITOBJ') AND Substring ( PC.DenyMask , 9 , 1 ) & 128 = 128

GO
GRANT UPDATE
    ON OBJECT::[external].[DocumentAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[DocumentAccessDenies] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[DocumentAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[DocumentAccessDenies] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[DocumentAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[DocumentAccessDenies] TO [OMSApplicationRole]
    AS [dbo];

