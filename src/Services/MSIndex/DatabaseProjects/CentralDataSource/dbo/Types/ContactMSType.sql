CREATE TYPE [dbo].[ContactMSType] AS TABLE
(
	[mattersphereid] BIGINT PRIMARY KEY
	, [address-id] BIGINT
	, [contactType] NVARCHAR(1000)
	, [contName] NVARCHAR(128)
	, [contNotes] NVARCHAR(4000)
	, [modifieddate] DATETIME
	, [ugdp] NVARCHAR(MAX)
	, [op] CHAR(1)
)
