CREATE PROCEDURE dbo.MergeAssociate (@Source AS dbo.AssociateMSType READONLY)
AS
SET NOCOUNT ON;

WITH g AS
(
	SELECT * 
	FROM dbo.UserGroup_Associate g 
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

MERGE dbo.Associate AS target
USING @Source AS source ON (target.mattersphereid = source.mattersphereid)
WHEN NOT MATCHED BY TARGET THEN
	INSERT (mattersphereid, assocHeading, assocSalut, assocAddressee, assocNotes, modifieddate, [file-id], [contact-id], associateType)
	VALUES(source.mattersphereid, source.assocHeading, source.assocSalut, source.assocAddressee, source.assocNotes, source.modifieddate, source.[file-id], source.[contact-id], source.associateType)
WHEN MATCHED AND source.op = 'D'
    THEN DELETE  
WHEN MATCHED
	THEN UPDATE SET target.assocHeading = source.assocHeading
		, target.assocSalut = source.assocSalut
		, target.assocAddressee = source.assocAddressee
		, target.assocNotes = source.assocNotes
		, target.modifieddate = source.modifieddate
		, target.[file-id] = source.[file-id]
		, target.[contact-id] = source.[contact-id]
		, target.associateType = source.associateType
;
