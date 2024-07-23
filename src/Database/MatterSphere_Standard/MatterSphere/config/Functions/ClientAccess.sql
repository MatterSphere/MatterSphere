

CREATE FUNCTION [config].[ClientAccess] ( )
RETURNS @T table (CLID BIGINT,Allow bit,[Deny] bit,secure bit)

AS


begin
	DECLARE @USER nvarchar(200)
	SET @USER  = config.GetUserLogin() 
	DECLARE @Sec INT
		select @SEC = [config].[IsAdministrator] (@User)
		if @Sec = 0
		begin

			WITH [ClientAllowDeny] ( [ClID] , [Allow] , [Deny] , [UserGroupID] , [UserGroup]  ) AS
		(
			SELECT 
				RUGC.[ClientID],
				CASE WHEN Substring ( PC.AllowMask , 5 , 1 ) & 128 = 128 THEN 1 ELSE NULL END as [Allow] ,
				CASE WHEN Substring ( PC.DenyMask , 5 , 1 ) & 128 = 128 THEN 1 ELSE NULL END as [Deny] ,
				RUGC.[UserGroupID],
				UGM.[Name] as [userGroup]
			FROM
				[relationship].[UserGroup_Client] RUGC
			JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGC.[PolicyID]
			LEFT JOIN
				[config].[GetUserAndGroupMembershipNT] (@User) UGM ON RUGC.[UserGroupID] = UGM.[ID]
			WHERE  (PC.IsRemote = 0 or PC.IsRemote is Null)
		)
	
			INSERT INTO @T (CLID,[Deny],Allow,SECURE)
			SELECT Z.[ClID] , CASE WHEN X.[Allow] IS NULL AND X.[Deny] IS NULL THEN 1 ELSE X.[Deny] END AS [Deny] , X.[Allow] , CASE WHEN X.[ClID] IS NULL THEN 1  END as [Secure]
			FROM
			( SELECT [ClID] FROM [ClientAllowDeny] GROUP BY [ClID] ) Z
			LEFT JOIN
			( SELECT [ClID] , Sum([Deny]) as [Deny] , Sum([Allow]) as [Allow] FROM [ClientAllowDeny] WHERE [userGroup] IS NOT NULL GROUP BY [ClID]  ) X ON X.[ClID] = Z.[ClID]
		end
RETURN
END