


CREATE FUNCTION [config].[ContactAccess] ( )
RETURNS @T table ([ContactID] BIGINT,Allow bit,[Deny] bit,secure bit)

AS
begin
	DECLARE @USER nvarchar(200)
	SET @USER = config.GetUserLogin() 
	DECLARE @Sec INT
		select @SEC = [config].[IsAdministrator] (@User)
		if @Sec = 0
		begin

			WITH [ContactAllowDeny] ( [ContactID] , [Allow] , [Deny] , [UserGroupID] , [UserGroup]  ) AS
				(
					SELECT 
						RUGC.[ContactID],
						CASE WHEN Substring ( PC.AllowMask , 10 , 1 ) & 128 = 128 THEN 1 ELSE NULL END as [Allow] ,
						CASE WHEN Substring ( PC.DenyMask , 10 , 1 ) & 128 = 128 THEN 1 ELSE NULL END as [Deny] ,
						RUGC.[UserGroupID],
						UGM.[Name] as [userGroup]
					FROM
						[relationship].[UserGroup_Contact] RUGC
					JOIN
						[config].[ObjectPolicy] PC ON PC.ID = RUGC.[PolicyID]
					LEFT JOIN
						[config].[GetUserAndGroupMembershipNT] (@User) UGM ON RUGC.[UserGroupID] = UGM.[ID]
					WHERE (PC.IsRemote = 0 or PC.IsRemote is Null)
				)
	
					INSERT INTO @T ([ContactID],[Deny],Allow,SECURE)
					SELECT Z.[ContactID] , CASE WHEN X.[Allow] IS NULL AND X.[Deny] IS NULL THEN 1 ELSE X.[Deny] END AS [Deny] , X.[Allow] , CASE WHEN X.[ContactID] IS NULL THEN 1  END as [Secure]
					FROM
					( SELECT [ContactID] FROM [ContactAllowDeny] GROUP BY [ContactID] ) Z
					LEFT JOIN
					( SELECT [ContactID] , Sum([Deny]) as [Deny] , Sum([Allow]) as [Allow] FROM [ContactAllowDeny] WHERE [userGroup] IS NOT NULL GROUP BY [ContactID]  ) X ON X.[ContactID] = Z.[ContactID]
		end
	RETURN 
end

