CREATE TABLE [dbo].[fddbIntegrationEntity]
(
		[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_fddbIntegrationEntity_ID]  DEFAULT (NEWID()),
		[Name] [NVARCHAR](50) NOT NULL,
		CONSTRAINT [PK_fddbIntegrationEntity] PRIMARY KEY CLUSTERED 
		(
			[ID] ASC
		) ON [PRIMARY]
	) ON [PRIMARY];

	


GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_fddbIntegrationEntity_RowGuid] ON [dbo].[fddbIntegrationEntity] ([ID] ASC) ON [IndexGroup]

GO