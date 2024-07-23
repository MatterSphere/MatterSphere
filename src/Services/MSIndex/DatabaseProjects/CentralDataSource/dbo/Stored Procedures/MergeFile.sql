CREATE PROCEDURE dbo.MergeFile (@Source AS dbo.FileMSType READONLY)
AS
SET NOCOUNT ON;

WITH g AS
(
	SELECT * 
	FROM dbo.UserGroup_File g 
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

MERGE dbo.[File] AS target
USING @Source AS source ON (target.mattersphereid = source.mattersphereid)
WHEN NOT MATCHED BY TARGET THEN
	INSERT (mattersphereid, fileDesc, fileNotes, modifieddate, [client-id], fileType, fileStatus)
	VALUES(source.mattersphereid, source.fileDesc, source.fileNotes, source.modifieddate, source.[client-id], source.fileType, source.fileStatus)
WHEN MATCHED AND source.op = 'D'
    THEN DELETE  
WHEN MATCHED
	THEN UPDATE SET target.fileDesc = source.fileDesc
		, target.fileNotes = source.fileNotes
		, target.modifieddate = source.modifieddate
		, target.[client-id] = source.[client-id]
		, target.fileType = source.fileType
		, target.fileStatus = source.fileStatus
;
