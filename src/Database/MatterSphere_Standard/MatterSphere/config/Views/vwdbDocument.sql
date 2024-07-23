

CREATE VIEW [config].[vwdbDocument]
AS

WITH [DocumentAllowDeny] ( [DocumentID], [Allow], [Deny], [Secure]) AS
(
	SELECT 
			RUGD.[DocumentID],
			MAX(CASE WHEN Substring ( PC.AllowMask , 9 , 1 ) & 128 = 128 AND UGM.ID IS NOT NULL THEN 1 ELSE NULL END) as [Allow] ,
			MAX(CASE WHEN Substring ( PC.DenyMask , 9 , 1 ) & 128 = 128 AND UGM.ID IS NOT NULL THEN 1 ELSE NULL END) as [Deny] ,
			CASE WHEN MAX(CASE WHEN UGM.ID IS NOT NULL THEN 1 END) IS NULL THEN 1 END AS Secure
	FROM
			[relationship].[UserGroup_Document] RUGD
	 CROSS APPLY config.IsAdministratorTbl_NS() admins
	JOIN
			[config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID]
	LEFT JOIN
			[config].[GetUserAndGroupMembershipNT_NS] () UGM ON RUGD.[UserGroupID] = UGM.[ID]
    WHERE admins.IsAdmin = 0 AND (PC.IsRemote = 0 or PC.IsRemote is Null)
	GROUP BY [DocumentID]
)

SELECT D.*
FROM config.dbDocument AS D 
	LEFT OUTER JOIN (
		SELECT [DocumentID]
			, CASE WHEN [Allow] IS NULL AND [Deny] IS NULL THEN 1 ELSE [Deny] END AS [Deny] 
			, [Secure]
		FROM [DocumentAllowDeny]
		) AS DA ON D.DocID = DA.DocumentID	--JOIN / APPLY to table valued function or ADO.NET dynamic SQL fails in matter centre
WHERE (DA.[Deny] IS NULL) AND (DA.Secure IS NULL) 


GO
CREATE TRIGGER [config].[DocumentDelete]
    ON [config].[vwdbDocument]
    INSTEAD OF DELETE
    NOT FOR REPLICATION
    AS 
BEGIN
DELETE DP
	FROM config.dbDocument DP
	JOIN DELETED D ON D.docID = DP.docID
END


GO
GRANT UPDATE
    ON OBJECT::[config].[vwdbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbDocument] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbDocument] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[vwdbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[vwdbDocument] TO [OMSApplicationRole]
    AS [dbo];

