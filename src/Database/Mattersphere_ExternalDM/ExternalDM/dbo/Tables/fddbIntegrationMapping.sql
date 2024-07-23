CREATE TABLE [dbo].[fddbIntegrationMapping]
(
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_fddbIntegrationMapping_ID]  DEFAULT (newid()),
	[SystemID] [uniqueidentifier] NOT NULL,
	[EntityID] [uniqueidentifier] NOT NULL,
	[InternalID] [nvarchar](50) NULL,
	[ExternalID] [nvarchar](50) NULL,
	[MappingType] [nvarchar](15) NULL,
	[Setting] [bit] NOT NULL CONSTRAINT [DF_fddbIntegrationMapping_Setting]  DEFAULT ((0)),
    CONSTRAINT [PK_fddbIntegrationMapping] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_fddbIntegrationMapping_RowGuid] ON [dbo].[fddbIntegrationMapping] 
(
	[ID] ASC
) ON [IndexGroup]

GO

ALTER TABLE [dbo].[fddbIntegrationMapping]  WITH CHECK ADD  CONSTRAINT [FK_fddbIntegrationMapping_fddbIntegrationEntity] FOREIGN KEY([EntityID])
REFERENCES [dbo].[fddbIntegrationEntity] ([ID])

GO

ALTER TABLE [dbo].[fddbIntegrationMapping] CHECK CONSTRAINT [FK_fddbIntegrationMapping_fddbIntegrationEntity]

GO

ALTER TABLE [dbo].[fddbIntegrationMapping]  WITH CHECK ADD  CONSTRAINT [FK_fddbIntegrationMapping_fddbIntegrationSystem] FOREIGN KEY([SystemID])
REFERENCES [dbo].[fddbIntegrationSystem] ([ID])

GO

ALTER TABLE [dbo].[fddbIntegrationMapping] CHECK CONSTRAINT [FK_fddbIntegrationMapping_fddbIntegrationSystem]

GO