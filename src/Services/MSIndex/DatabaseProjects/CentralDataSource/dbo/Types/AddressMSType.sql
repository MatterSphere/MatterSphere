CREATE TYPE [dbo].[AddressMSType] AS TABLE
(
	[mattersphereid] BIGINT PRIMARY KEY
	, [sc] NVARCHAR(MAX)
	, [modifieddate] DATETIME
	, [ugdp] NVARCHAR(MAX)
	, [op] CHAR(1)
)
