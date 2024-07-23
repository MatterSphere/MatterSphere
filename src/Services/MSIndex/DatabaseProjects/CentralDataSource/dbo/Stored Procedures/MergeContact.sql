CREATE PROCEDURE dbo.MergeContact (@Source AS dbo.ContactMSType READONLY)
AS
SET NOCOUNT ON;
WITH g AS
(
	SELECT * 
	FROM dbo.UserGroup_Contact g 
	WHERE EXISTS(SELECT 1 FROM @Source s WHERE s.mattersphereid = g.mattersphereid)
)
MERGE g AS target
USING (
	SELECT s.mattersphereid
 		, REPLACE(REPLACE(ugdp.Value, '(ALLOW)', ''), '(DENY)', '') AS UserGroupID
		, CASE WHEN CHARINDEX('(DENY)', ugdp.Value) > 0 THEN 1 ELSE 0 END AS gDeny
	FROM @Source s
	CROSS APPLY dbo.SplitString(s.ugdp, ' ') ugdp
	) AS source ON (target.mattersphereid = source.mattersphereid AND target.UserGroupID = source.UserGroupID)
WHEN NOT MATCHED BY TARGET THEN
	INSERT (mattersphereid, UserGroupID, gDeny)
	VALUES(source.mattersphereid, source.UserGroupID, source.gDeny)
WHEN NOT MATCHED BY SOURCE
    THEN DELETE  
WHEN MATCHED AND target.gDeny <> source.gDeny
	THEN UPDATE SET target.gDeny = source.gDeny
;

MERGE dbo.Contact AS target
USING @Source AS source ON (target.mattersphereid = source.mattersphereid)
WHEN NOT MATCHED BY TARGET THEN
	INSERT (mattersphereid, contName, contNotes, modifieddate, [address-id], contactType)
	VALUES(source.mattersphereid, source.contName, source.contNotes, source.modifieddate, source.[address-id], source.contactType)
WHEN MATCHED AND source.op = 'D'
    THEN DELETE  
WHEN MATCHED
	THEN UPDATE SET target.contName = source.contName
		, target.contNotes = source.contNotes
		, target.modifieddate = source.modifieddate
		, target.[address-id] = source.[address-id]
		, target.contactType = source.contactType
;
