CREATE FUNCTION [config].[CheckFileSaveDocAccess] ( @USER nvarchar(200), @ID bigint )
RETURNS @T table ( FileID bigint, UAllow bit, UDeny bit, Secure tinyint, UserGroup nvarchar(200) )
AS
BEGIN
	DECLARE @Sec INT
	SELECT @SEC = [config].[IsAdministrator] (@User)
	IF @Sec = 0
		BEGIN
			WITH [FileAllowDeny] ( [FileID] , [UAllow] , [UDeny] , [UserGroupID] , [UserGroup] ) AS
			(
				SELECT
					RUGF.[FileID],
					CASE WHEN Substring ( PC.AllowMask , 6 , 1 ) & 2 = 2 THEN 1 ELSE NULL END as [UAllow] ,
					CASE WHEN Substring ( PC.DenyMask , 6 , 1 ) & 2 = 2 THEN 1 ELSE NULL END as [UDeny] ,
					RUGF.[UserGroupID],
					UGM.[Name] as [UserGroup]
				FROM
					[relationship].[UserGroup_File] RUGF
				JOIN
					[config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID]
				LEFT JOIN
					[config].[GetUserAndGroupMembershipNT] (@User) UGM ON RUGF.[UserGroupID] = UGM.[ID]
				WHERE RUGF.FileID = @ID
			)

			INSERT INTO @T ( FileID, UAllow, UDeny, Secure , UserGroup )
			SELECT Z.[FileID] , X.[UAllow] , X.[UDeny] , CASE WHEN X.[FileID] IS NULL THEN 1 END as [Secure] , [UserGroup]  
			FROM
				( SELECT [FileID] FROM [FileAllowDeny] GROUP BY [FileID] ) Z
			LEFT JOIN
				( SELECT [FileID] , Sum([UAllow]) as [UAllow] , Sum([UDeny]) as [UDeny] , [UserGroup]
				  FROM [FileAllowDeny] WHERE [UserGroup] IS NOT NULL GROUP BY [FileID] , [UserGroup] ) X ON X.[FileID] = Z.[FileID]
			UNION
			SELECT
				null, null, 1, 2, U.[Name]
			FROM
				[item].[user] U
			JOIN 
				[config].[SystemPolicy] SP ON SP.[ID] = U.PolicyID 
			CROSS APPLY 
				[config].[SystemMaskToPermissions] ( SP.AllowMask , SP.DenyMask ) SMP
			WHERE 
				SMP.Byte = 3 AND SMP.BitValue = 1 AND SMP.[Deny] = 1
				AND U.NTLogin = @USER
			UNION
			SELECT
				null, null, 1, 2, G.[Description] 
			FROM
				[item].[User] U
			JOIN
				[relationship].[Group_User] GU ON GU.UserID = U.[ID]
			JOIN
				[item].[Group] G ON G.[ID] = GU.[GroupID]
			JOIN [config].[SystemPolicy] SP ON SP.[ID] = G.PolicyID 
			CROSS APPLY 
				[config].[SystemMaskToPermissions] ( SP.AllowMask , SP.DenyMask ) SMP
			WHERE 
				SMP.Byte = 3 AND SMP.BitValue = 1 AND SMP.[Deny] = 1
				AND U.NTLogin = @USER
	END
	RETURN
END
