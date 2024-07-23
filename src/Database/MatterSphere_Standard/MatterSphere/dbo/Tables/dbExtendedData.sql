CREATE TABLE [dbo].[dbExtendedData] (
    [extCode]       [dbo].[uCodeLookup] NOT NULL,
    [extSourceType] [dbo].[uCodeLookup] CONSTRAINT [DF_dbExtendedData_extType] DEFAULT (N'OMS') NOT NULL,
    [extSource]     NVARCHAR (500)      NULL,
    [extCall]       NVARCHAR (500)      NULL,
    [extWhere]      NVARCHAR (500)      NULL,
    [extParameters] [dbo].[uXML]        CONSTRAINT [DF_dbExtendedData_extParameters] DEFAULT (N'<params></params>') NULL,
    [extSourceLink] NVARCHAR (30)       NULL,
    [extDestLink]   NVARCHAR (30)       NULL,
    [extModes]      TINYINT             CONSTRAINT [DF_dbCustomData_custType] DEFAULT ((0)) NOT NULL,
    [extFields]     [dbo].[uXML]        CONSTRAINT [DF_dbExtendedData_extFields] DEFAULT (N'<config/>') NOT NULL,
    [extSystem]     BIT                 CONSTRAINT [DF_dbExtendedData_extSystem] DEFAULT ((0)) NOT NULL,
    [Created]       [dbo].[uCreated]    CONSTRAINT [DF_dbExtendedData_Created] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]     [dbo].[uCreatedBy]  NULL,
    [Updated]       [dbo].[uCreated]    CONSTRAINT [DF_dbExtendedData_Updated] DEFAULT (getutcdate()) NULL,
    [UpdatedBy]     [dbo].[uCreatedBy]  NULL,
    [rowguid]       UNIQUEIDENTIFIER    CONSTRAINT [DF_dbExtendedData_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [extWhereMsApi] NVARCHAR (250)      NULL,
    [extApiExclude] BIT                 DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_dbExtendedData] PRIMARY KEY CLUSTERED ([extCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbExtendedData_rowguid]
    ON [dbo].[dbExtendedData]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbExtendedData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbExtendedData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbExtendedData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbExtendedData] TO [OMSApplicationRole]
    AS [dbo];

