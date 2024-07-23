CREATE TABLE [dbo].[fddbIntegrationSystem]
(
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Table_1_IntergrationID]  DEFAULT (newid()),
	[Name] [nvarchar](50) NOT NULL,
	CONSTRAINT [PK_fddbIntegrationSystem] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	) ON [PRIMARY]

)ON [PRIMARY]

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_fddbIntegrationSystem_RowGuid] ON [dbo].[fddbIntegrationSystem] 
(
	[ID] ASC
) ON [IndexGroup]

GO
