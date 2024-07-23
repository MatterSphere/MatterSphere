CREATE TYPE [dbo].[TaskMSType] AS TABLE
(
	[mattersphereid] BIGINT PRIMARY KEY
	, [file-id] BIGINT
	, [document-id] BIGINT
	, [taskType] NVARCHAR(1000)
	, [tskDesc] NVARCHAR(100)
	, [tskNotes] NVARCHAR(MAX)
	, [modifieddate] DATETIME
	, [ugdp] NVARCHAR(MAX)
	, [op] CHAR(1)
)
