CREATE TABLE [dbo].[dbDocumentVersionPreview] (
    [verID]      UNIQUEIDENTIFIER NOT NULL,
    [verPreview] NVARCHAR (MAX)   NULL,
    [rowguid]    UNIQUEIDENTIFIER CONSTRAINT [DF_dbDocumentVersionPreview_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbDocumentVersionPreview] PRIMARY KEY CLUSTERED ([verID] ASC),
    CONSTRAINT [FK_dbDocumentVersionPreview_dbDocumentVersion] FOREIGN KEY ([verID]) REFERENCES [dbo].[dbDocumentVersion] ([verID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDocumentVersionPreview_rowguid]
    ON [dbo].[dbDocumentVersionPreview]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDocumentVersionPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDocumentVersionPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDocumentVersionPreview] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDocumentVersionPreview] TO [OMSApplicationRole]
    AS [dbo];

