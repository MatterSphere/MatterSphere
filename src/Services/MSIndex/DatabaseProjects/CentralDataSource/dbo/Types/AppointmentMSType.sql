CREATE TYPE [dbo].[AppointmentMSType] AS TABLE
(
	[mattersphereid] BIGINT PRIMARY KEY
	, [file-id] BIGINT
	, [client-id] BIGINT
	, [associate-id] BIGINT
	, [document-id] BIGINT
	, [appointmentType] NVARCHAR(1000)
	, [appDesc] NVARCHAR(255)
	, [appLocation] NVARCHAR(255)
	, [modifieddate] DATETIME
	, [ugdp] NVARCHAR(MAX)
	, [op] CHAR(1)
)
