CREATE PROCEDURE dbo.MergeDocument (@Source AS dbo.DocumentMSType READONLY)
AS
SET NOCOUNT ON;

WITH g AS
(
	SELECT * 
	FROM dbo.UserGroup_Document g 
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

MERGE dbo.Document AS target
USING @Source AS source ON (target.mattersphereid = source.mattersphereid)
WHEN NOT MATCHED BY TARGET THEN
	INSERT (mattersphereid, docDesc, usrFullName, modifieddate, [associate-id], [client-id], [file-id], docDeleted, documentExtension, documentType)
	VALUES(source.mattersphereid, source.docDesc, source.usrFullName, source.modifieddate, source.[associate-id], source.[client-id], source.[file-id], docDeleted, documentExtension, documentType)
WHEN MATCHED AND source.op = 'D'
    THEN DELETE  
WHEN MATCHED
	THEN UPDATE SET target.docDesc  = source.docDesc
		, target.usrFullName = source.usrFullName
		, target.modifieddate = source.modifieddate
		, target.[associate-id] = source.[associate-id]
		, target.[client-id] = source.[client-id]
		, target.[file-id] = source.[file-id]
		, target.docDeleted = source.docDeleted
		, target.documentExtension = source.documentExtension
		, target.documentType = source.documentType
;
