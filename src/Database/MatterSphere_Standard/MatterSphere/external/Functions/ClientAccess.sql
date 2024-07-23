

CREATE FUNCTION [external].[ClientAccess] ( )
RETURNS @T table (ClientID BIGINT)

AS
BEGIN
	DECLARE @USER nvarchar(200) 
	SET @USER = config.GetUserLogin() 

		INSERT INTO @T (ClientID)
		SELECT 
			RUGC.[ClientID] as clid			
		FROM
			[relationship].[UserGroup_Client] RUGC
		JOIN
			[config].[ObjectPolicy] PC ON PC.ID = RUGC.[PolicyID]
		JOIN
			[config].[GetUserAndGroupMembershipNT] (@User) UGM ON RUGC.[UserGroupID] = UGM.[ID]
		WHERE 
			(PC.IsRemote = 1 or PC.[Type] = 'EXPLICITOBJ') AND Substring ( PC.AllowMask , 5 , 1 ) & 128 = 128
RETURN
END
