CREATE TABLE [dbo].[Task]
(
	[mattersphereid] BIGINT
	, [file-id] BIGINT
	, [document-id] BIGINT
	, [taskType] NVARCHAR(1000)
	, [tskDesc] NVARCHAR(100)
	, [tskNotes] NVARCHAR(MAX)
	, [modifieddate] DATETIME
	, [tskSearch] AS CONCAT([tskDesc], ' ', [tskNotes])
	, CONSTRAINT PK_Task PRIMARY KEY ([mattersphereid])
)
GO
CREATE INDEX IX_Task_fileid ON [dbo].[Task] ([file-id])
GO
CREATE INDEX IX_Task_documentid ON [dbo].[Task] ([document-id])
GO

CREATE FULLTEXT INDEX ON [dbo].[Task] ([tskSearch])
KEY INDEX PK_Task
WITH CHANGE_TRACKING = AUTO, STOPLIST = OFF;
GO