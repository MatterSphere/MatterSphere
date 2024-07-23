CREATE TABLE [dbo].[Appointment]
(
	[mattersphereid] BIGINT 
	, [file-id] BIGINT
	, [client-id] BIGINT
	, [associate-id] BIGINT
	, [document-id] BIGINT
	, [appointmentType] NVARCHAR(1000)
	, [appDesc] NVARCHAR(255)
	, [appLocation] NVARCHAR(255)
	, [modifieddate] DATETIME
	, [appSearch] AS CONCAT([appDesc], ' ', [appLocation])
	, CONSTRAINT PK_Appointment PRIMARY KEY ([mattersphereid])
)
GO
CREATE INDEX IX_Appointment_fileid ON [dbo].[Appointment] ([file-id])
GO
CREATE INDEX IX_Appointment_clientid ON [dbo].[Appointment] ([client-id])
GO
CREATE INDEX IX_Appointment_associateid ON [dbo].[Appointment] ([associate-id])
GO
CREATE INDEX IX_Appointment_documentid ON [dbo].[Appointment] ([document-id])
GO

CREATE FULLTEXT INDEX ON [dbo].[Appointment] ([appSearch])
KEY INDEX PK_Appointment
WITH CHANGE_TRACKING = AUTO, STOPLIST = OFF;
GO
