CREATE TABLE [dbo].[dbDocumentVersion] (
    [verID]           UNIQUEIDENTIFIER CONSTRAINT [DF_dbDocumentVersion_verID] DEFAULT (newid()) NOT NULL,
    [docID]           BIGINT           NOT NULL,
    [verNumber]       INT              CONSTRAINT [DF_dbDocumentVersion_verNumber] DEFAULT ((0)) NOT NULL,
    [verParent]       UNIQUEIDENTIFIER NULL,
    [verDepth]        TINYINT          CONSTRAINT [DF_dbDocumentVersion_verDepth] DEFAULT ((0)) NOT NULL,
    [verLabel]        NVARCHAR (50)    NULL,
    [verStatus]       NVARCHAR (15)    NULL,
    [verComments]     NVARCHAR (500)   NULL,
    [verToken]        NVARCHAR (255)   NULL,
    [verChecksum]     VARCHAR (50)     NULL,
    [verLastEditedBy] INT              NULL,
    [verLastEdited]   DATETIME         NULL,
    [verAuthoredBy]   INT              NULL,
    [verAuthored]     DATETIME         NULL,
    [CreatedBy]       INT              NULL,
    [Created]         DATETIME         NULL,
    [UpdatedBy]       INT              NULL,
    [Updated]         DATETIME         NULL,
    [verDeleted]      BIT              CONSTRAINT [DF_dbDocumentVersion_verDeleted] DEFAULT ((0)) NOT NULL,
    [rowguid]         UNIQUEIDENTIFIER CONSTRAINT [DF_dbDocumentVersion_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbDocumentVersion] PRIMARY KEY CLUSTERED ([docID] ASC, [verID] ASC),
    CONSTRAINT [FK_dbDocumentVersion_dbDocument] FOREIGN KEY ([docID]) REFERENCES [dbo].[dbDocument] ([docID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocumentVersion_dbDocumentVersion] FOREIGN KEY ([verParent]) REFERENCES [dbo].[dbDocumentVersion] ([verID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocumentVersion_dbDocumentVersion1] FOREIGN KEY ([verID]) REFERENCES [dbo].[dbDocumentVersion] ([verID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDocumentVersion_verID]
    ON [dbo].[dbDocumentVersion]([verID] DESC)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDocumentVersion_rowguid]
    ON [dbo].[dbDocumentVersion]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbDocumentVersion_docID]
    ON [dbo].[dbDocumentVersion]([docID] DESC)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDocumentVersion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDocumentVersion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDocumentVersion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDocumentVersion] TO [OMSApplicationRole]
    AS [dbo];

