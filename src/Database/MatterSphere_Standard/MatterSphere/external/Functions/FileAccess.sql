

CREATE FUNCTION [external].[FileAccess] ( )
RETURNS @T table (FileID BIGINT)
AS

BEGIN
	DECLARE @USER nvarchar(200)
	SET @USER = config.GetUserLogin() 

		INSERT INTO @T (FileID)
		SELECT
			RUGF.[FileID]
		FROM
			[relationship].[UserGroup_File] RUGF
		JOIN
			[config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID]
		JOIN
			[config].[GetUserAndGroupMembershipNT] (@User) UGM ON RUGF.[UserGroupID] = UGM.[ID]
        WHERE (PC.IsRemote = 1 or PC.[Type] = 'EXPLICITOBJ') AND Substring ( PC.AllowMask , 6 , 1 ) & 32 = 32

RETURN
END


