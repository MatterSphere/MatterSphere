CREATE PROCEDURE dbo.MergeTask (@Source AS dbo.TaskMSType READONLY)
AS
SET NOCOUNT ON;

MERGE dbo.Task AS target
USING @Source AS source ON (target.mattersphereid = source.mattersphereid)
WHEN NOT MATCHED BY TARGET THEN
	INSERT (mattersphereid, tskDesc, tskNotes, modifieddate, [file-id], [document-id], taskType)
	VALUES(source.mattersphereid, source.tskDesc, source.tskNotes, source.modifieddate, source.[file-id], source.[document-id], source.taskType)
WHEN MATCHED AND source.op = 'D'
    THEN DELETE  
WHEN MATCHED
	THEN UPDATE SET target.tskDesc = source.tskDesc
		, target.tskNotes = source.tskNotes
		, target.modifieddate = source.modifieddate
		, target.[file-id] = source.[file-id]
		, target.[document-id] = source.[document-id]
		, target.taskType = source.taskType
;
