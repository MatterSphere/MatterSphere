CREATE TYPE [dbo].[FileMSType] AS TABLE
(
	[mattersphereid] BIGINT PRIMARY KEY
	, [client-id] BIGINT
	, [fileType] NVARCHAR(1000)
	, [fileStatus] NVARCHAR(1000)
	, [fileDesc] NVARCHAR(255)
	, [fileNotes] NVARCHAR(MAX)
	, [modifieddate] DATETIME
	, [ugdp] NVARCHAR(MAX)
	, [op] CHAR(1)
)
