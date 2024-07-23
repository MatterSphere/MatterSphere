CREATE TABLE [dbo].[dbDocumentPreview] (
    [docID]      BIGINT           NOT NULL,
    [docPreview] NVARCHAR (MAX)   NULL,
    [rowguid]    UNIQUEIDENTIFIER CONSTRAINT [DF_dbDocumentPreview_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbDocumentPreview] PRIMARY KEY CLUSTERED ([docID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbDocumentPreview_dbDocument] FOREIGN KEY ([docID]) REFERENCES [dbo].[dbDocument] ([docID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDocumentPreview_rowguid]
    ON [dbo].[dbDocumentPreview]([rowguid] ASC)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDocumentPreview] TO [OMSApplicationRole]
    AS [dbo];

