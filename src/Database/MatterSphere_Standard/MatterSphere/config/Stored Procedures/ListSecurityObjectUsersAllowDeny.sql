

CREATE PROCEDURE [config].[ListSecurityObjectUsersAllowDeny]
      @secObject uCodeLookup ,
      @secID bigint ,
      @secPermission uCodeLookup 
      
AS
SET NOCOUNT ON 
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

/* If Search Permissions excluded from Sharepoint and permissions are checked after the search returns.*/
IF exists (select 1 from dbo.dbRegInfo where regLocalSearchserverGroupsActive = 1)
begin
	return
end

/* If Search Permissions included from Sharepoint potential 64K limit in ACL and no support for Local Groups*/

IF @secPermission = 'LIST'
BEGIN
	IF @secObject = 'DOCUMENT'
	BEGIN
		SET @secPermission = 'VIEWDOC'
	END 
	IF @secObject = 'FILE'
	BEGIN
		SET @secPermission = 'VIEWFL'	
	END 
	IF @secObject = 'CLIENT'
	BEGIN
		SET @secPermission = 'VIEWCL'	
	END 
END

DECLARE @byteValue tinyint , @bitValue tinyint
SELECT @bitValue = BitValue , @byteValue = Byte FROM [config].[ObjectPolicyConfig] WHERE SecurableType = @secObject AND Permission = @secPermission

-- Document security
IF @secObject = 'DOCUMENT'
BEGIN
      SELECT  Z.NTLogin , CASE WHEN Coalesce(Sum(Z.[Allow]),0) = 0 THEN 0 ELSE 1 END as [Allow]
      FROM
            (
            -- Users
            SELECT U.NTLogin , X.Allow , X.[Deny] 
            FROM 
                  [item].[User] U 
            LEFT JOIN 
                  [Relationship].[UserGroup_Document] UGD ON UGD.UserGroupID = U.ID AND UGD.DocumentID = @secID
            OUTER APPLY
                  [config].[GetAllowDeny] ( @byteValue , @bitValue , UGD.PolicyID ) X

            UNION ALL
            -- Groups
            SELECT U.NTLogin , X.Allow , X.[Deny] 
            FROM 
                  [item].[User] U 
            JOIN
                  [relationship].[Group_User] GU ON GU.UserID = U.ID
            LEFT JOIN 
                  [Relationship].[UserGroup_Document] UGD ON UGD.UserGroupID = GU.GroupID AND UGD.DocumentID = @secID
            OUTER APPLY
                  [config].[GetAllowDeny] ( @byteValue , @bitValue , UGD.PolicyID ) X
 			UNION ALL
			-- Admins
			SELECT 
				U.[NTLogin] ,SMP.[Allow] , 0
			FROM
				[item].[User] U
			JOIN
				[config].[SystemPolicy] P ON P.[ID] = U.[PolicyID]
			CROSS APPLY 
				[config].[SystemMaskToPermissions] ( P.AllowMask , P.DenyMask ) SMP
			WHERE 
				SMP.Byte = 2 AND SMP.BitValue = 8 
			and exists (select top 1 UGD.RelationshipID from [Relationship].[UserGroup_Document] UGD where UGD.DocumentID = @secID)
			 ) 
      Z WHERE [Allow] = 1 GROUP BY Z.NTLogin
      ORDER BY 1
      SELECT  Z.NTLogin , CASE WHEN Coalesce(Sum(Z.[Deny]) , 0) = 0 THEN 0 ELSE 1 END as [Deny]
      FROM
            (
            -- Users
            SELECT U.NTLogin , X.Allow , X.[Deny] 
            FROM 
                  [item].[User] U 
            LEFT JOIN 
                  [Relationship].[UserGroup_Document] UGD ON UGD.UserGroupID = U.ID AND UGD.DocumentID = @secID
            OUTER APPLY
                  [config].[GetAllowDeny] ( @byteValue , @bitValue , UGD.PolicyID ) X

            UNION ALL
            -- Groups
            SELECT U.NTLogin , X.Allow , X.[Deny] 
            FROM 
                  [item].[User] U 
            JOIN
                  [relationship].[Group_User] GU ON GU.UserID = U.ID
            LEFT JOIN 
                  [Relationship].[UserGroup_Document] UGD ON UGD.UserGroupID = GU.GroupID AND UGD.DocumentID = @secID
            OUTER APPLY
                  [config].[GetAllowDeny] ( @byteValue , @bitValue , UGD.PolicyID ) X
            ) 
      Z WHERE [Deny] = 1 GROUP BY Z.NTLogin
      ORDER BY 1
      RETURN
END

-- Matter Security
IF @secObject = 'FILE'
BEGIN
      SELECT  Z.NTLogin , CASE WHEN Coalesce(Sum(Z.[Allow]),0) = 0 THEN 0 ELSE 1 END as [Allow]
      FROM
            (
            -- Users
            SELECT U.NTLogin , X.Allow , X.[Deny] 
            FROM 
                  [item].[User] U 
            LEFT JOIN 
                  [Relationship].[UserGroup_File] UGD ON UGD.UserGroupID = U.ID AND UGD.FileID = @secID
            OUTER APPLY
                  [config].[GetAllowDeny] ( @byteValue , @bitValue , UGD.PolicyID ) X

            UNION ALL
            -- Groups
            SELECT U.NTLogin , X.Allow , X.[Deny] 
            FROM 
                  [item].[User] U 
            JOIN
                  [relationship].[Group_User] GU ON GU.UserID = U.ID
            LEFT JOIN 
                  [Relationship].[UserGroup_File] UGD ON UGD.UserGroupID = GU.GroupID AND UGD.FileID = @secID
            OUTER APPLY
                  [config].[GetAllowDeny] ( @byteValue , @bitValue , UGD.PolicyID ) X
			UNION ALL
			-- Admins
			SELECT 
				U.[NTLogin] ,SMP.[Allow] , 0
			FROM
				[item].[User] U
			JOIN
				[config].[SystemPolicy] P ON P.[ID] = U.[PolicyID]
			CROSS APPLY 
				[config].[SystemMaskToPermissions] ( P.AllowMask , P.DenyMask ) SMP
			WHERE 
				SMP.Byte = 2 AND SMP.BitValue = 8   
			and exists (select top 1 UGF.RelationshipID from [Relationship].[UserGroup_File] UGF where UGF.FileID = @secID)  
			        ) 
      Z WHERE [Allow] = 1   GROUP BY Z.NTLogin

      ORDER BY 1
      SELECT  Z.NTLogin , CASE WHEN Coalesce(Sum(Z.[Deny]) , 0) = 0 THEN 0 ELSE 1 END as [Deny]
      FROM
            (
            -- Users
            SELECT U.NTLogin , X.Allow , X.[Deny] 
            FROM 
                  [item].[User] U 
            LEFT JOIN 
                  [Relationship].[UserGroup_File] UGD ON UGD.UserGroupID = U.ID AND UGD.FileID = @secID
            OUTER APPLY
                  [config].[GetAllowDeny] ( @byteValue , @bitValue , UGD.PolicyID ) X

            UNION ALL
            -- Groups
            SELECT U.NTLogin , X.Allow , X.[Deny] 
            FROM 
                  [item].[User] U 
            JOIN
                  [relationship].[Group_User] GU ON GU.UserID = U.ID
            LEFT JOIN 
                  [Relationship].[UserGroup_File] UGD ON UGD.UserGroupID = GU.GroupID AND UGD.FileID = @secID
            OUTER APPLY
                  [config].[GetAllowDeny] ( @byteValue , @bitValue , UGD.PolicyID ) X
            ) 
      Z WHERE [Deny] = 1  GROUP BY Z.NTLogin
      ORDER BY 1
      RETURN
END

IF @secObject = 'CLIENT'
BEGIN
      SELECT  Z.NTLogin , CASE WHEN Coalesce(Sum(Z.[Allow]),0) = 0 THEN 0 ELSE 1 END as [Allow]
      FROM
            (
            -- Users
            SELECT U.NTLogin , X.Allow , X.[Deny] 
            FROM 
                  [item].[User] U 
            LEFT JOIN 
                  [Relationship].[UserGroup_Client] UGD ON UGD.UserGroupID = U.ID AND UGD.ClientID = @secID
            OUTER APPLY
                  [config].[GetAllowDeny] ( @byteValue , @bitValue , UGD.PolicyID ) X

            UNION ALL
            -- Groups
            SELECT U.NTLogin , X.Allow , X.[Deny] 
            FROM 
                  [item].[User] U 
            JOIN
                  [relationship].[Group_User] GU ON GU.UserID = U.ID
            LEFT JOIN 
                  [Relationship].[UserGroup_Client] UGD ON UGD.UserGroupID = GU.GroupID AND UGD.ClientID = @secID
            OUTER APPLY
                  [config].[GetAllowDeny] ( @byteValue , @bitValue , UGD.PolicyID ) X
			UNION ALL
			-- Admins
			SELECT 
				U.[NTLogin] ,SMP.[Allow] , 0
			FROM
				[item].[User] U
			JOIN
				[config].[SystemPolicy] P ON P.[ID] = U.[PolicyID]
			CROSS APPLY 
				[config].[SystemMaskToPermissions] ( P.AllowMask , P.DenyMask ) SMP
			WHERE 
				SMP.Byte = 2 AND SMP.BitValue = 8  
			and exists (select top 1 UGC.RelationshipID from [Relationship].[UserGroup_Client] UGC where UGC.ClientID = @secID)  
          ) 
      Z WHERE [Allow] = 1 GROUP BY Z.NTLogin
      ORDER BY 1
      SELECT  Z.NTLogin , CASE WHEN Coalesce(Sum(Z.[Deny]) , 0) = 0 THEN 0 ELSE 1 END as [Deny]
      FROM
            (
            -- Users
            SELECT U.NTLogin , X.Allow , X.[Deny] 
            FROM 
                  [item].[User] U 
            LEFT JOIN 
                  [Relationship].[UserGroup_Client] UGD ON UGD.UserGroupID = U.ID AND UGD.ClientID = @secID
            OUTER APPLY
                  [config].[GetAllowDeny] ( @byteValue , @bitValue , UGD.PolicyID ) X

            UNION ALL
            -- Groups
            SELECT U.NTLogin , X.Allow , X.[Deny] 
            FROM 
                  [item].[User] U 
            JOIN
                  [relationship].[Group_User] GU ON GU.UserID = U.ID
            LEFT JOIN 
                  [Relationship].[UserGroup_Client] UGD ON UGD.UserGroupID = GU.GroupID AND UGD.ClientID = @secID
            OUTER APPLY
                  [config].[GetAllowDeny] ( @byteValue , @bitValue , UGD.PolicyID ) X
            ) 
      Z WHERE [Deny] = 1 GROUP BY Z.NTLogin
      ORDER BY 1
      RETURN
END


IF @secObject = 'CONTACT'
BEGIN
      SELECT  Z.NTLogin , CASE WHEN Coalesce(Sum(Z.[Allow]),0) = 0 THEN 0 ELSE 1 END as [Allow]
      FROM
            (
            -- Users
            SELECT U.NTLogin , X.Allow , X.[Deny] 
            FROM 
                  [item].[User] U 
            LEFT JOIN 
                  [Relationship].[UserGroup_Contact] UGD ON UGD.UserGroupID = U.ID AND UGD.ContactID = @secID
            OUTER APPLY
                  [config].[GetAllowDeny] ( @byteValue , @bitValue , UGD.PolicyID ) X

            UNION ALL
            -- Groups
            SELECT U.NTLogin , X.Allow , X.[Deny] 
            FROM 
                  [item].[User] U 
            JOIN
                  [relationship].[Group_User] GU ON GU.UserID = U.ID
            LEFT JOIN 
                  [Relationship].[UserGroup_Contact] UGD ON UGD.UserGroupID = GU.GroupID AND UGD.ContactID = @secID
            OUTER APPLY
                  [config].[GetAllowDeny] ( @byteValue , @bitValue , UGD.PolicyID ) X
 			UNION ALL
			-- Admins
			SELECT 
				U.[NTLogin] ,SMP.[Allow] , 0
			FROM
				[item].[User] U
			JOIN
				[config].[SystemPolicy] P ON P.[ID] = U.[PolicyID]
			CROSS APPLY 
				[config].[SystemMaskToPermissions] ( P.AllowMask , P.DenyMask ) SMP
			WHERE 
				SMP.Byte = 2 AND SMP.BitValue = 8 
			and exists (select top 1 UGC.RelationshipID from [Relationship].[UserGroup_Contact] UGC where UGC.ContactID = @secID)  
          ) 
      Z WHERE [Allow] = 1 GROUP BY Z.NTLogin
      ORDER BY 1
      SELECT  Z.NTLogin , CASE WHEN Coalesce(Sum(Z.[Deny]) , 0) = 0 THEN 0 ELSE 1 END as [Deny]
      FROM
            (
            -- Users
            SELECT U.NTLogin , X.Allow , X.[Deny] 
            FROM 
                  [item].[User] U 
            LEFT JOIN 
                  [Relationship].[UserGroup_Contact] UGD ON UGD.UserGroupID = U.ID AND UGD.ContactID = @secID
            OUTER APPLY
                  [config].[GetAllowDeny] ( @byteValue , @bitValue , UGD.PolicyID ) X

            UNION ALL
            -- Groups
            SELECT U.NTLogin , X.Allow , X.[Deny] 
            FROM 
                  [item].[User] U 
            JOIN
                  [relationship].[Group_User] GU ON GU.UserID = U.ID
            LEFT JOIN 
                  [Relationship].[UserGroup_Contact] UGD ON UGD.UserGroupID = GU.GroupID AND UGD.ContactID = @secID
            OUTER APPLY
                  [config].[GetAllowDeny] ( @byteValue , @bitValue , UGD.PolicyID ) X
            ) 
      Z WHERE [Deny] = 1 GROUP BY Z.NTLogin
      ORDER BY 1
      RETURN
END



GO
GRANT EXECUTE
    ON OBJECT::[config].[ListSecurityObjectUsersAllowDeny] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListSecurityObjectUsersAllowDeny] TO [OMSAdminRole]
    AS [dbo];

