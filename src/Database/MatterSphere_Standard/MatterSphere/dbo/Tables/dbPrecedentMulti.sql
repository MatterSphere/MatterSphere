CREATE TABLE [dbo].[dbPrecedentMulti] (
    [multiID]              BIGINT              NOT NULL,
    [multiMasterID]        BIGINT              NOT NULL,
    [multiChildID]         BIGINT              NULL,
    [multiOrder]           INT                 CONSTRAINT [DF_dbPrecedentMulti_multiOrder] DEFAULT ((0)) NOT NULL,
    [multiPrecType]        [dbo].[uCodeLookup] NULL,
    [multiPrecTitle]       NVARCHAR (50)       NULL,
    [multiPrecLibrary]     [dbo].[uCodeLookup] NULL,
    [multiPrecCategory]    [dbo].[uCodeLookup] NULL,
    [multiPrecSubCategory] [dbo].[uCodeLookup] NULL,
    [multiContactType]     [dbo].[uCodeLookup] NULL,
    [multiAssocType]       [dbo].[uCodeLookup] NULL,
    [multiSaveMode]        SMALLINT            CONSTRAINT [DF_dbPrecedentMulti_multiSaveMode] DEFAULT ((0)) NOT NULL,
    [multiPrintMode]       SMALLINT            CONSTRAINT [DF_dbPrecedentMulti_multiPrintMode] DEFAULT ((0)) NOT NULL,
    [multiNewTemplate]     BIT                 CONSTRAINT [DF_dbPrecedentMulti_multiNewTemplate] DEFAULT ((0)) NOT NULL,
    [multiXML]             [dbo].[uXML]        CONSTRAINT [DF_dbPrecedentMulti_multiXML] DEFAULT (N'<config />') NOT NULL,
    [rowguid]              UNIQUEIDENTIFIER    CONSTRAINT [DF_dbPrecedentMulti_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [multiPrecMinorCategory] [dbo].[uCodeLookup] NULL, 
    CONSTRAINT [PK_dbPrecedentMulti] PRIMARY KEY CLUSTERED ([multiID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbPrecedentMulti_dbContactType] FOREIGN KEY ([multiContactType]) REFERENCES [dbo].[dbContactType] ([typeCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbPrecedentMulti_dbDocumentType] FOREIGN KEY ([multiPrecType]) REFERENCES [dbo].[dbDocumentType] ([typeCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbPrecedentMulti_dbPrecedents] FOREIGN KEY ([multiMasterID]) REFERENCES [dbo].[dbPrecedents] ([PrecID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbPrecedentMulti_dbPrecedents1] FOREIGN KEY ([multiChildID]) REFERENCES [dbo].[dbPrecedents] ([PrecID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbPrecedentMulti_rowguid]
    ON [dbo].[dbPrecedentMulti]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbPrecedentMulti_multiMasterID]
    ON [dbo].[dbPrecedentMulti]([multiMasterID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbPrecedentMulti] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbPrecedentMulti] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbPrecedentMulti] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbPrecedentMulti] TO [OMSApplicationRole]
    AS [dbo];

