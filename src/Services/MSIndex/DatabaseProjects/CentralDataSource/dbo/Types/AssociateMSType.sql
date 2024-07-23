CREATE TYPE [dbo].[AssociateMSType] AS TABLE
(
	[mattersphereid] BIGINT PRIMARY KEY
	, [file-id] BIGINT
	, [contact-id] BIGINT
	, [associateType] NVARCHAR(1000)
	, [assocHeading] NVARCHAR(255)
	, [assocSalut]  NVARCHAR(100)
	, [assocAddressee] NVARCHAR(100)
	, [assocNotes] NVARCHAR(200)
	, [modifieddate] DATETIME
	, [ugdp] NVARCHAR(MAX)
	, [op] CHAR(1)
)
