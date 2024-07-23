CREATE TABLE [dbo].[dbFileManagementApplication] (
    [appCode]   [dbo].[uCodeLookup] NOT NULL,
    [appXML]    [dbo].[uXML]        CONSTRAINT [DF_dbFileManagementApplication_appXML] DEFAULT (N'<Config/>') NOT NULL,
    [appScript] [dbo].[uCodeLookup] NULL,
    [appVer]    INT                 NULL,
    [appActive] BIT                 CONSTRAINT [DF_dbFileManagementApplication_appActive] DEFAULT ((1)) NOT NULL,
    [Created]   [dbo].[uCreated]    NULL,
    [CreatedBy] [dbo].[uCreatedBy]  NULL,
    [Updated]   [dbo].[uCreated]    NULL,
    [UpdatedBy] [dbo].[uCreatedBy]  NULL,
    [rowguid]   UNIQUEIDENTIFIER    CONSTRAINT [DF_dbFileManagementApplication_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbFileManagementApplication] PRIMARY KEY CLUSTERED ([appCode] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFileManagementApplication_rowguid]
    ON [dbo].[dbFileManagementApplication]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFileManagementApplication] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFileManagementApplication] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFileManagementApplication] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFileManagementApplication] TO [OMSApplicationRole]
    AS [dbo];

