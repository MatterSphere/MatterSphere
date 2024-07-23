CREATE TABLE [dbo].[dbDocumentStorage] (
    [docID]   BIGINT           NOT NULL,
    [docBLOB] VARBINARY (MAX)  NOT NULL,
    [rowguid] UNIQUEIDENTIFIER CONSTRAINT [DF_dbDocumentStorage_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbDocumentStorage] PRIMARY KEY CLUSTERED ([docID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbDocumentStorage_dbDocument] FOREIGN KEY ([docID]) REFERENCES [dbo].[dbDocument] ([docID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDocumentStorage_rowguid]
    ON [dbo].[dbDocumentStorage]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDocumentStorage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDocumentStorage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDocumentStorage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDocumentStorage] TO [OMSApplicationRole]
    AS [dbo];

