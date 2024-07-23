IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[external].[ContactAccessDenies]'))
BEGIN
DECLARE @VIEW nvarchar(max)
SET @VIEW = N'
ALTER VIEW [external].[ContactAccessDenies]
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
			(PC.IsRemote = 1 or PC.[Type] = ''EXPLICITOBJ'') AND Substring ( PC.DenyMask , 10 , 1 ) & 128 = 128
'
EXEC (@VIEW)
END
GO

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[external].[DocumentAccessDenies]'))
BEGIN
DECLARE @VIEW nvarchar(max)
SET @VIEW = N'
ALTER VIEW [external].[DocumentAccessDenies]
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
			(PC.IsRemote = 1 or PC.[Type] = ''EXPLICITOBJ'') AND Substring ( PC.DenyMask , 9 , 1 ) & 128 = 128
'
EXEC (@VIEW)
END
GO

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[external].[FileAccessDenies]'))
BEGIN
DECLARE @VIEW nvarchar(max)
SET @VIEW = N'
ALTER VIEW [external].[FileAccessDenies]
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
			(PC.IsRemote = 1 or PC.[Type] = ''EXPLICITOBJ'') AND Substring ( PC.DenyMask , 6 , 1 ) & 32 = 32
'
EXEC (@VIEW)
END
GO