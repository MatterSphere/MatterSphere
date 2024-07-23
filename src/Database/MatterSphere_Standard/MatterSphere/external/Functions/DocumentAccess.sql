

CREATE FUNCTION [external].[DocumentAccess] ( )
RETURNS @T table (DocumentID BIGINT)

AS
BEGIN
	DECLARE @USER nvarchar(200)
	SET @USER = config.GetUserLogin() 

	IF EXISTS (SELECT 1 FROM [dbo].[dbRegInfo] WHERE [regBlockInheritence] = 0)
		Begin
		INSERT INTO @T (DocumentID)
            SELECT 
                  RUGD.[DocumentID]                 
            FROM
                  [relationship].[UserGroup_Document] RUGD
            JOIN
                  [config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID]
            JOIN
                  [config].[GetUserAndGroupMembershipNT] (@User) UGM ON RUGD.[UserGroupID] = UGM.[ID]
            WHERE (PC.IsRemote = 1 or PC.[Type] = 'EXPLICITOBJ') AND Substring ( PC.AllowMask , 9 , 1 ) & 128 = 128
		end
	else
		begin
		INSERT INTO @T (DocumentID)
		SELECT DISTINCT DocumentID from
				(SELECT 
					RUGD.[DocumentID]                 
				FROM
					[relationship].[UserGroup_Document] RUGD
				JOIN
					[config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID]
				JOIN
					[config].[GetUserAndGroupMembershipNT] (@User) UGM ON RUGD.[UserGroupID] = UGM.[ID]
				WHERE (PC.IsRemote = 1 or PC.[Type] = 'EXPLICITOBJ') AND Substring ( PC.AllowMask , 9 , 1 ) & 128 = 128
				UNION
				SELECT 
					D.[DocID]                 
				FROM
					[Config].[dbDocument] D
				JOIN
					[relationship].[UserGroup_File] RUGF ON RUGF.FileID = D.FileID
				JOIN
					[config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID]
				JOIN
					[config].[GetUserAndGroupMembershipNT] (@User) UGM ON RUGF.[UserGroupID] = UGM.[ID]
				WHERE (PC.IsRemote = 1 or PC.[Type] = 'EXPLICITOBJ') AND Substring ( PC.AllowMask , 9 , 1 ) & 128 = 128) as A
		end
RETURN
END

