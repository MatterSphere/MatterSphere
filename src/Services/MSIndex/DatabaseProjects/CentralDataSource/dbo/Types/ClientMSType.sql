CREATE TYPE [dbo].[ClientMSType] AS TABLE
(
	[mattersphereid] BIGINT PRIMARY KEY
	, [address-id] BIGINT
	, [clientType] NVARCHAR(1000)
	, [clName] NVARCHAR(128)
	, [clNo] NVARCHAR(12)
	, [clNotes] NVARCHAR(MAX)
	, [modifieddate] DATETIME
	, [ugdp] NVARCHAR(MAX)
	, [op] CHAR(1)
)
