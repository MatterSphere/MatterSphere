

CREATE VIEW [config].[vwdbContact]
AS
	WITH admins AS
	(SELECT * FROM  config.IsAdministratorTbl_NS() AS C )

	, ContactDeny( ContactID, [Deny], [Secure]) AS
	(
		SELECT RUGC.ContactID
			, MAX(CASE WHEN SUBSTRING( PC.DenyMask , 10 , 1 ) & 128 = 128 AND UGM.ID IS NOT NULL THEN 1 END) AS [Deny]
			, CASE WHEN MAX(CASE WHEN UGM.ID IS NOT NULL THEN 1 END) IS NULL THEN 1 END AS Secure
		FROM relationship.UserGroup_Contact RUGC
			JOIN config.ObjectPolicy PC ON PC.ID = RUGC.PolicyID
			LEFT JOIN config.GetUserAndGroupMembershipNT_NS() UGM ON UGM.ID = RUGC.UserGroupID
		WHERE(SELECT IsAdmin FROM admins) = 0
			AND (PC.IsRemote = 0 OR PC.IsRemote IS NULL)
		GROUP BY RUGC.ContactID
	)
	SELECT C.*
	FROM         
		config.dbContact AS C LEFT OUTER JOIN
		ContactDeny AS CA ON C.contID = CA.ContactID	--JOIN / APPLY to table valued function or ADO.NET dynamic SQL fails in matter centre
	WHERE     
		(CA.[Deny] IS NULL) AND (CA.Secure IS NULL)

GO
CREATE TRIGGER [config].[ContactDelete]
    ON [config].[vwdbContact]
    INSTEAD OF DELETE
    NOT FOR REPLICATION
    AS 
BEGIN
DELETE DP
	FROM config.dbContact DP
	JOIN DELETED D ON D.contID = DP.contID
END


GO
GRANT UPDATE
    ON OBJECT::[config].[vwdbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbContact] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbContact] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[vwdbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[vwdbContact] TO [OMSApplicationRole]
    AS [dbo];

