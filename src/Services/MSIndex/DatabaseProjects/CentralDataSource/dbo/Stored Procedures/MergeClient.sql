CREATE PROCEDURE dbo.MergeClient (@Source AS dbo.ClientMSType READONLY)
AS
SET NOCOUNT ON;

WITH g AS
(
	SELECT * 
	FROM dbo.UserGroup_Client g 
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

MERGE dbo.Client AS target
USING @Source AS source ON (target.mattersphereid = source.mattersphereid)
WHEN NOT MATCHED BY TARGET THEN
	INSERT (mattersphereid, clName, clNo, clNotes, modifieddate, [address-id], clientType)
	VALUES(source.mattersphereid, source.clName, source.clNo, source.clNotes, source.modifieddate, source.[address-id], source.clientType)
WHEN MATCHED AND source.op = 'D'
    THEN DELETE  
WHEN MATCHED
	THEN UPDATE SET target.clName = source.clName
		, target.clNo = source.clNo
		, target.clNotes = source.clNotes
		, target.modifieddate = source.modifieddate
		, target.[address-id] = source.[address-id]
		, target.clientType = source.clientType
;
