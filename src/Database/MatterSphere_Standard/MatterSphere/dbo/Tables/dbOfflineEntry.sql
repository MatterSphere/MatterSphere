CREATE TABLE [dbo].[dbOfflineEntry] (
    [offID]       INT                 NOT NULL,
    [fileID]      BIGINT              NULL,
    [docID]       BIGINT              NULL,
    [offDate]     DATETIME            NULL,
    [offReturned] DATETIME            NULL,
    [offBy]       INT                 NULL,
    [offType]     [dbo].[uCodeLookup] NULL,
    [offForced]   BIT                 CONSTRAINT [DF_dbOfflineEntry_offForced] DEFAULT ((0)) NOT NULL,
    [rowguid]     UNIQUEIDENTIFIER    CONSTRAINT [DF_dbOfflineEntry_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbOfflineEntry] PRIMARY KEY CLUSTERED ([offID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbOfflineEntry_rowguid]
    ON [dbo].[dbOfflineEntry]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbOfflineEntry] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbOfflineEntry] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbOfflineEntry] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbOfflineEntry] TO [OMSApplicationRole]
    AS [dbo];

