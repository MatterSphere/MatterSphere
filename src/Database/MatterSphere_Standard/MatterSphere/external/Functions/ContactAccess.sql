
CREATE FUNCTION [external].[ContactAccess] ( )
RETURNS @T table (ContactID BIGINT)

AS
BEGIN
	DECLARE @USER nvarchar(200) 
	SET @USER = config.GetUserLogin() 

		INSERT INTO @T (ContactID)
 		SELECT 
			RUGC.[ContactID]
		FROM
			[relationship].[UserGroup_Contact] RUGC
		JOIN
			[config].[ObjectPolicy] PC ON PC.ID = RUGC.[PolicyID]
		JOIN
			[config].[GetUserAndGroupMembershipNT] (@User) UGM ON RUGC.[UserGroupID] = UGM.[ID]
		WHERE (PC.IsRemote = 1 or PC.[Type] = 'EXPLICITOBJ') and Substring ( PC.AllowMask , 10 , 1 ) & 128 = 128
RETURN
END
