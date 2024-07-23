

CREATE VIEW [config].[vwdbClient]
AS
WITH ClientDeny( ClientID, [Deny], [Secure]) AS
(
	SELECT RUGC.ClientID
		, MAX(CASE WHEN SUBSTRING ( PC.DenyMask , 5 , 1 ) & 128 = 128 AND UGM.ID IS NOT NULL THEN 1 END) AS [Deny]
		, CASE WHEN MAX(CASE WHEN UGM.ID IS NOT NULL THEN 1 END) IS NULL THEN 1 END AS Secure
	FROM relationship.UserGroup_Client RUGC
		JOIN config.ObjectPolicy PC ON PC.ID = RUGC.PolicyID
		LEFT JOIN config.GetUserAndGroupMembershipNT_NS() UGM ON UGM.ID = RUGC.UserGroupID
		CROSS APPLY config.IsAdministratorTbl_NS() admins
	WHERE admins.IsAdmin = 0
		AND (PC.IsRemote = 0 OR PC.IsRemote IS NULL)
	GROUP BY RUGC.ClientID
)
SELECT C.*
FROM config.dbClient AS C 
	LEFT JOIN ClientDeny AS CA ON C.clID = CA.ClientID	--JOIN / APPLY to table valued function or ADO.NET dynamic SQL fails in matter centre
WHERE (CA.[Deny] IS NULL) AND (CA.Secure IS NULL)

GO
CREATE TRIGGER [config].[ClientDelete]
    ON [config].[vwdbClient]
    INSTEAD OF DELETE
    NOT FOR REPLICATION
    AS 
BEGIN
DELETE DP
	FROM config.dbClient DP
	JOIN DELETED D ON D.clID = DP.clID
END


GO
GRANT UPDATE
    ON OBJECT::[config].[vwdbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbClient] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbClient] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[vwdbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[vwdbClient] TO [OMSApplicationRole]
    AS [dbo];

