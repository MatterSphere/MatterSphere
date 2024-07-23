
CREATE VIEW [config].[vwdbFile]
AS
WITH FileDeny( FileID, [Deny], [Secure]) AS
(
	SELECT RUGF.FileID
		, MAX(CASE WHEN SUBSTRING ( PC.DenyMask , 6 , 1 ) & 32 = 32 AND UGM.ID IS NOT NULL THEN 1 END) AS [Deny]
		, CASE WHEN MAX(CASE WHEN UGM.ID IS NOT NULL THEN 1 END) IS NULL THEN 1 END AS Secure
	FROM relationship.UserGroup_File RUGF
		JOIN config.ObjectPolicy PC ON PC.ID = RUGF.PolicyID
		LEFT JOIN config.GetUserAndGroupMembershipNT_NS() UGM ON UGM.ID = RUGF.UserGroupID
		CROSS APPLY config.IsAdministratorTbl_NS() admins
	WHERE admins.IsAdmin = 0
		AND (PC.IsRemote = 0 OR PC.IsRemote IS NULL)
	GROUP BY RUGF.FileID
)
SELECT F.*
FROM config.dbFile AS F 
	LEFT JOIN FileDeny AS FA ON F.fileID = FA.FileID	--JOIN / APPLY to table valued function or ADO.NET dynamic SQL fails in matter centre
WHERE (FA.[Deny] IS NULL) AND (FA.Secure IS NULL) 


GO
CREATE TRIGGER [config].[MatterDelete]
    ON [config].[vwdbFile]
    INSTEAD OF DELETE
    NOT FOR REPLICATION
    AS 
BEGIN
DELETE DP
	FROM config.dbFile DP
	JOIN DELETED D ON D.fileID = DP.fileID
END


GO
GRANT UPDATE
    ON OBJECT::[config].[vwdbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbFile] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbFile] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[vwdbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[vwdbFile] TO [OMSApplicationRole]
    AS [dbo];

