/*
Script to create the new Log Table for the Export Service.
Optionally in addition to writing to the event log the service can 
also write to this database table.
This assists with remote administration when the users does not have access 
to the server event log.
*/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fdExportServiceLog]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[fdExportServiceLog](
	[logID] [int] IDENTITY(1,1) NOT NULL,
	[DateLogged] [datetime] NOT NULL CONSTRAINT [DF_fdExportServiceLog_Date]  DEFAULT (getdate()),
	[Application] char(3) NOT NULL,
	[Entity] nvarchar(50) NOT NULL,
	[ErrorMessage] nvarchar(MAX) NOT NULL,
	[RecordID] nvarchar(15) NOT NULL,
	[Action] char(1) NOT NULL,
 CONSTRAINT [PK_fdExportServiceLog] PRIMARY KEY CLUSTERED 
(
	[logID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
