CREATE TABLE [dbo].[dbSearchListConfig] (
    [schCode]             [dbo].[uCodeLookup] NOT NULL,
    [schGroup]            [dbo].[uCodeLookup] NULL,
    [schIsReport]         BIT                 CONSTRAINT [DF_dbSearchListConfig_schIsReport] DEFAULT ((0)) NOT NULL,
    [schStyle]            TINYINT             CONSTRAINT [DF_dbSearchListConfig_schIsList] DEFAULT ((1)) NOT NULL,
    [schType]             [dbo].[uCodeLookup] NULL,
    [schVersion]          BIGINT              CONSTRAINT [DF_dbSearchListConfig_schVersion] DEFAULT ((0)) NOT NULL,
    [schEnquiry]          [dbo].[uCodeLookup] NULL,
    [schSourceType]       [dbo].[uCodeLookup] CONSTRAINT [DF_dbSearchListConfig_schSourceType] DEFAULT (N'OMS') NOT NULL,
    [schSource]           NVARCHAR (200)      NULL,
    [schSourceCall]       NVARCHAR (MAX)      NULL,
    [schSourceParameters] [dbo].[uXML]        CONSTRAINT [DF_dbSearchListConfig_schSourceParameters] DEFAULT (N'<params></params>') NOT NULL,
    [schListView]         [dbo].[uXML]        CONSTRAINT [DF_dbSearchListConfig_schListView] DEFAULT (N'<searchList>
	<buttons>
	<button id="0" name="cmdSearch" lookup="BTNSEARCH" visible="True"/>
	<button id="1" name="cmdSelect" lookup="BTNSELECT" visible="True"/>
	<button id="2" name="cmdAdd" lookup="BTNADD" visible="True"/>
	<button id="3" name="cmdEdit" lookup="BTNEDIT" visible="True"/>
	</buttons></searchList>') NOT NULL,
    [schReturnField]      [dbo].[uXML]        CONSTRAINT [DF_dbSearchListConfig_schReturnField] DEFAULT (N'<fields></fields>') NOT NULL,
    [schSecurityLevel]    INT                 CONSTRAINT [DF_dbSearchListConfig_schSecurityLevel] DEFAULT ((0)) NOT NULL,
    [schActive]           BIT                 CONSTRAINT [DF_dbSearchListConfig_schActive] DEFAULT ((1)) NULL,
    [schSystem]           BIT                 CONSTRAINT [DF_dbSearchListConfig_schSystem] DEFAULT ((0)) NOT NULL,
    [Created]             [dbo].[uCreated]    NULL,
    [CreatedBy]           [dbo].[uCreatedBy]  NULL,
    [Updated]             [dbo].[uCreated]    NULL,
    [UpdatedBy]           [dbo].[uCreatedBy]  NULL,
    [schScript]           [dbo].[uCodeLookup] NULL,
    [rowguid]             UNIQUEIDENTIFIER    CONSTRAINT [DF_dbSearchListConfig_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [schApiExclude]       BIT                 DEFAULT ((1)) NOT NULL,
	[schPagination]       BIT NULL,
	[schActionButton]     BIT NULL,
    CONSTRAINT [PK_dbSearchListConfig] PRIMARY KEY CLUSTERED ([schCode] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbSearchListConfig_dbEnquiry] FOREIGN KEY ([schEnquiry]) REFERENCES [dbo].[dbEnquiry] ([enqCode]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbSearchListConfig_rowguid]
    ON [dbo].[dbSearchListConfig]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];

GO

CREATE TRIGGER [dbo].[tgrUpdateSearchListConfig] ON [dbo].[dbSearchListConfig]
FOR UPDATE NOT FOR REPLICATION
AS
	SET NOCOUNT ON;
	DELETE dbo.dbUserSearchListColumns WHERE schCode in (SELECT i.schCode FROM inserted i INNER JOIN deleted d ON d.schCode = i.schCode AND ISNULL(d.schListView, '') <> ISNULL(i.schListView, ''))

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbSearchListConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbSearchListConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbSearchListConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbSearchListConfig] TO [OMSApplicationRole]
    AS [dbo];

