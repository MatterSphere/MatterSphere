CREATE FUNCTION [config].[CheckFileAccess] ( @USER nvarchar(200),@ID bigint  )
RETURNS @T table (FileID BIGINT,VAllow bit,[VDeny] bit,UAllow bit,[UDeny] bit,secure tinyint , [UserGroup] nvarchar(200)  )
AS
begin
	DECLARE @Sec INT
		select @SEC = [config].[IsAdministrator] (@User)
		if @Sec = 0
		begin
	
			WITH [FileAllowDeny] ( [FileID] , [VAllow] , [VDeny] , [UAllow] , [UDeny] , [UserGroupID] , [UserGroup]  ) AS
			(
				SELECT
					RUGF.[FileID],
					CASE WHEN Substring ( PC.AllowMask , 6 , 1 ) & 32 = 32 THEN 1 ELSE NULL END as [VAllow] ,
					CASE WHEN Substring ( PC.DenyMask , 6 , 1 ) & 32 = 32 THEN 1 ELSE NULL END as [VDeny] ,
					CASE WHEN Substring ( PC.AllowMask , 6 , 1 ) & 64 = 64 THEN 1 ELSE NULL END as [UAllow] ,
					CASE WHEN Substring ( PC.DenyMask , 6 , 1 ) & 64 = 64 THEN 1 ELSE NULL END as [UDeny] ,
					RUGF.[UserGroupID],
					UGM.[Name] as [userGroup]
				FROM
					[relationship].[UserGroup_File] RUGF
				JOIN
					[config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID]
				LEFT JOIN
					[config].[GetUserAndGroupMembershipNT] (@User) UGM ON RUGF.[UserGroupID] = UGM.[ID]
				WHERE RUGF.FileID = @ID
			)

			INSERT INTO @T (FileID,[VDeny],VAllow,[UDeny],UAllow,SECURE , [UserGroup]  )
			SELECT Z.[FileID] ,  X.[VDeny]  , X.[VAllow], X.[UDeny] , X.[UAllow] , CASE WHEN X.[FileID] IS NULL THEN 1  END as [Secure] , [UserGroup]  
			FROM
			( SELECT [FileID] FROM [FileAllowDeny] GROUP BY [FileID] ) Z
			LEFT JOIN
			( SELECT [FileID] , Sum([VDeny]) as [VDeny] , Sum([VAllow]) as [VAllow] , Sum([UDeny]) as [UDeny] , Sum([UAllow]) as [UAllow] , [UserGroup]   FROM [FileAllowDeny] WHERE [userGroup] IS NOT NULL GROUP BY [FileID] , [UserGroup]    ) X ON X.[FileID] = Z.[FileID]
			UNION
			SELECT
				null,null,null,1,null,2,U.[Name]
			FROM
				[item].[user] U
			JOIN 
				[config].[SystemPolicy] SP ON SP.[ID] = U.PolicyID 
			CROSS APPLY 
				[config].[SystemMaskToPermissions] ( SP.AllowMask , SP.DenyMask ) SMP
			WHERE 
				SMP.Byte = 3 AND SMP.BitValue = 32
			and u.NTLogin = @USER
			and smp.[Deny] = 1
			union
			SELECT
				null,null,null,1,null,2,G.[Description] 
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
				SMP.Byte = 3 AND SMP.BitValue = 32
			and u.NTLogin = @USER
			and smp.[Deny] = 1

		end
	RETURN
end