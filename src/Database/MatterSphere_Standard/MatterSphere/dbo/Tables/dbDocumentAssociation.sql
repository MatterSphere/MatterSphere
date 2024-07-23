CREATE TABLE [dbo].[dbDocumentAssociation] (
    [ID]                   UNIQUEIDENTIFIER    CONSTRAINT [DF_dbDocumentAssociation_ID] DEFAULT (newid()) NOT NULL,
    [DocumentID]           BIGINT              NULL,
    [VersionID]            UNIQUEIDENTIFIER    NOT NULL,
    [AssociatedDocumentID] BIGINT              NULL,
    [AssociatedVersionID]  UNIQUEIDENTIFIER    NOT NULL,
    [Code]                 [dbo].[uCodeLookup] NOT NULL,
    [rowguid]              UNIQUEIDENTIFIER    CONSTRAINT [DF_dbDocumentAssociation_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbDocumentAssociation] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_dbDocumentAssociation_dbDocument] FOREIGN KEY ([DocumentID]) REFERENCES [dbo].[dbDocument] ([docID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocumentAssociation_dbDocumentVersion] FOREIGN KEY ([VersionID]) REFERENCES [dbo].[dbDocumentVersion] ([verID]) NOT FOR REPLICATION
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDocumentAssociation] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDocumentAssociation] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDocumentAssociation] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDocumentAssociation] TO [OMSApplicationRole]
    AS [dbo];

