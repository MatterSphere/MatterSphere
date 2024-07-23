

CREATE PROCEDURE [config].[ListSecurityObjectUsersAccess]
      @secObject uCodeLookup ,
      @secID bigint ,
      @secPermission uCodeLookup 
      
AS
SET NOCOUNT ON 
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @byteValue tinyint , @bitValue tinyint
SELECT @bitValue = BitValue , @byteValue = Byte FROM [config].[ObjectPolicyConfig] WHERE SecurableType = @secObject AND Permission = @secPermission

-- Document security
IF @secObject = 'DOCUMENT'
BEGIN
      SELECT  Z.NTLogin , CASE WHEN Sum(Z.[Deny]) = 0 AND Sum(Z.Allow) > 0 THEN 1 ELSE 0 END as AllowDeny
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
      Z GROUP BY Z.NTLogin
      ORDER BY 1
      RETURN
END

-- Matter Security
IF @secObject = 'FILE'
BEGIN
      SELECT  Z.NTLogin , CASE WHEN Sum(Z.[Deny]) = 0 AND Sum(Z.Allow) > 0 THEN 1 ELSE 0 END as AllowDeny
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
			and exists (select top 1 UGD.RelationshipID from [Relationship].[UserGroup_File] UGD where UGD.FileID = @secID)
           ) 
      Z GROUP BY Z.NTLogin
      ORDER BY 1
      RETURN
END

IF @secObject = 'CLIENT'
BEGIN
      SELECT  Z.NTLogin , CASE WHEN Sum(Z.[Deny]) = 0 AND Sum(Z.Allow) > 0 THEN 1 ELSE 0 END as AllowDeny
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
			and exists (select top 1 UGD.RelationshipID from [Relationship].[UserGroup_Client] UGD where UGD.ClientID = @secID)
           ) 
      Z GROUP BY Z.NTLogin
      ORDER BY 1
      RETURN
END


IF @secObject = 'CONTACT'
BEGIN
      SELECT  Z.NTLogin , CASE WHEN Sum(Z.[Deny]) = 0 AND Sum(Z.Allow) > 0 THEN 1 ELSE 0 END as AllowDeny
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
			and exists (select top 1 UGD.RelationshipID from [Relationship].[UserGroup_Contact] UGD where UGD.ContactID = @secID)
           ) 
      Z GROUP BY Z.NTLogin
      ORDER BY 1
      RETURN
END

GO
GRANT EXECUTE
    ON OBJECT::[config].[ListSecurityObjectUsersAccess] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListSecurityObjectUsersAccess] TO [OMSAdminRole]
    AS [dbo];

