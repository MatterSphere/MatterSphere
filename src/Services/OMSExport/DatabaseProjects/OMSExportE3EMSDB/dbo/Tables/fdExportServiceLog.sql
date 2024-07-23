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

