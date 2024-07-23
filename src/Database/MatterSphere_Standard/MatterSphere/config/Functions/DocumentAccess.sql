

CREATE FUNCTION [config].[DocumentAccess] ( )
RETURNS table 

AS
RETURN
(
      WITH [DocumentAllowDeny] ( [DocumentID] , [Allow] , [Deny] , [UserGroupID] , [UserGroup]  ) AS
      (
            SELECT 
                  RUGD.[DocumentID],
                  CASE WHEN Substring ( PC.AllowMask , 9 , 1 ) & 128 = 128 THEN 1 ELSE NULL END as [Allow] ,
                  CASE WHEN Substring ( PC.DenyMask , 9 , 1 ) & 128 = 128 THEN 1 ELSE NULL END as [Deny] ,
                  RUGD.[UserGroupID],
                  UGM.[Name] as [userGroup]
            FROM
                  [relationship].[UserGroup_Document] RUGD
            JOIN
                  [config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID]
            LEFT JOIN
                  [config].[GetUserAndGroupMembershipNT_NS] () UGM ON RUGD.[UserGroupID] = UGM.[ID]
                  CROSS APPLY config.IsAdministratorTbl_NS() admins
            WHERE admins.IsAdmin = 0 AND (PC.IsRemote = 0 or PC.IsRemote is Null)
      )      
                    
            SELECT Z.[DocumentID] , CASE WHEN X.[Allow] IS NULL AND X.[Deny] IS NULL THEN 1 ELSE X.[Deny] END AS [Deny] , X.[Allow] , CASE WHEN X.[DocumentID] IS NULL THEN 1  END as [Secure]
            FROM
            ( SELECT [DocumentID] FROM [DocumentAllowDeny] GROUP BY [DocumentID] ) Z
            LEFT JOIN
            ( SELECT [DocumentID] , Sum([Deny]) as [Deny] , Sum([Allow]) as [Allow] FROM [DocumentAllowDeny] WHERE [userGroup] IS NOT NULL GROUP BY [DocumentID]  ) X ON X.[DocumentID] = Z.[DocumentID]
  
)



GO
GRANT UPDATE
    ON OBJECT::[config].[DocumentAccess] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[DocumentAccess] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[DocumentAccess] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[DocumentAccess] TO [OMSApplicationRole]
    AS [dbo];

