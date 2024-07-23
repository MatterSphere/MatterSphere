

CREATE VIEW [external].[FileAccessDenies]
AS
		SELECT
			RUGF.FileID as id
		FROM
			[relationship].[UserGroup_Document] RUGF
		JOIN
			[config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID]
		JOIN
			[config].[GetUserAndGroupMembershipNT] (config.GetUserLogin()) UGM ON RUGF.[UserGroupID] = UGM.[ID]
		WHERE
			(PC.IsRemote = 1 or PC.[Type] = 'EXPLICITOBJ') AND Substring ( PC.DenyMask , 6 , 1 ) & 32 = 32

GO
GRANT UPDATE
    ON OBJECT::[external].[FileAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[FileAccessDenies] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[FileAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[FileAccessDenies] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[FileAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[FileAccessDenies] TO [OMSApplicationRole]
    AS [dbo];

