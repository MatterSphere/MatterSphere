

CREATE FUNCTION [config].[CheckClientAccess] ( @USER nvarchar(200),@ID bigint  )
RETURNS @T table (CLID BIGINT,VAllow bit,VDeny bit,UAllow bit,UDeny bit,secure smallint , UserGroup nvarchar(200))

AS


begin
	DECLARE @Sec INT
		select @SEC = [config].[IsAdministrator] (@User)
		if @Sec = 0
		begin

			WITH [ClientAllowDeny] ( [ClID] , [VAllow] , [VDeny] , [UAllow] , [UDeny] , [UserGroupID] , [UserGroup]  ) AS
		(
			SELECT 
				RUGC.[ClientID],
				CASE WHEN Substring ( PC.AllowMask , 5 , 1 ) & 128 = 128 THEN 1 ELSE NULL END as [VAllow] ,
				CASE WHEN Substring ( PC.DenyMask , 5 , 1 ) & 128 = 128 THEN 1 ELSE NULL END as [VDeny] ,
				CASE WHEN Substring ( PC.AllowMask , 4 , 1 ) & 4 = 4 THEN 1 ELSE NULL END as [UAllow] ,
				CASE WHEN Substring ( PC.DenyMask , 4 , 1 ) & 4 = 4 THEN 1 ELSE NULL END as [UDeny] ,
				RUGC.[UserGroupID],
				UGM.[Name] as [userGroup]
			FROM
				[relationship].[UserGroup_Client] RUGC
			JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGC.[PolicyID]
			LEFT JOIN
				[config].[GetUserAndGroupMembershipNT] (@User) UGM ON RUGC.[UserGroupID] = UGM.[ID]
			WHERE RUGC.ClientID = @ID
			)
	
			INSERT INTO @T (CLID,VDeny,VAllow,UDeny,UAllow,SECURE , UserGroup )
			SELECT Z.[ClID] , X.[VDeny] , X.[VAllow] ,  X.[UDeny] , X.[UAllow], CASE WHEN X.[ClID] IS NULL THEN 1  END as [Secure] , [UserGroup]  
			FROM
			( SELECT [ClID] FROM [ClientAllowDeny] GROUP BY [ClID] ) Z
			LEFT JOIN
			( SELECT [ClID] , Sum([VDeny]) as [VDeny] , Sum([VAllow]) as [VAllow], Sum([UDeny]) as [UDeny] , Sum([UAllow]) as [UAllow] , [UserGroup]   FROM [ClientAllowDeny] WHERE [userGroup] IS NOT NULL GROUP BY [ClID] , [UserGroup]    ) X ON X.[ClID] = Z.[ClID]
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
				SMP.Byte = 3 AND SMP.BitValue = 16
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
				SMP.Byte = 3 AND SMP.BitValue = 16
			and u.NTLogin = @USER
			and smp.[Deny] = 1
		end
RETURN
END


