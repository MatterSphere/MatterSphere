CREATE TABLE [dbo].[dbCoreImportConfig] (
    [ciID]      INT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ciType]    [dbo].[uCodeLookup] NOT NULL,
    [ciSource]  [dbo].[uCodeLookup] NULL,
    [ciFilter1] NVARCHAR (500)      NULL,
    [ciFilter2] NVARCHAR (500)      NULL,
    [ciFilter3] NVARCHAR (500)      NULL,
    [ciData]    NVARCHAR (500)      NULL,
    [rowguid]   UNIQUEIDENTIFIER    CONSTRAINT [DF_dbCoreImportConfig_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbCoreImportConfig] PRIMARY KEY CLUSTERED ([ciID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbCoreImportConfig_rowguid]
    ON [dbo].[dbCoreImportConfig]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbCoreImportConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbCoreImportConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbCoreImportConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbCoreImportConfig] TO [OMSApplicationRole]
    AS [dbo];

