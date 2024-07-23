CREATE TYPE [dbo].[DocumentMSType] AS TABLE
(
	[mattersphereid] BIGINT PRIMARY KEY
	, [associate-id] BIGINT
	, [client-id] BIGINT
	, [file-id] BIGINT
	, [docDeleted] BIT
	, [documentExtension] NVARCHAR(15)
	, [documentType] NVARCHAR(1000)
	, [docDesc] NVARCHAR(150)
	, [usrFullName] NVARCHAR(50)
	, [modifieddate] DATETIME
	, [ugdp] NVARCHAR(MAX)
	, [op] CHAR(1)
)
