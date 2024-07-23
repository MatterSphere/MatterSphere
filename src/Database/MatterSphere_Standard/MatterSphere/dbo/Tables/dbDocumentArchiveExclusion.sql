CREATE TABLE [dbo].[dbDocumentArchiveExclusion] (
    [ID]            BIGINT             IDENTITY (1, 1) NOT NULL,
    [DocID]         BIGINT             NOT NULL,
    [FileID]        BIGINT             NOT NULL,
    [CreatedBy]     [dbo].[uCreatedBy] NULL,
    [Created]       [dbo].[uCreated]   NULL,
    [ArchiveAction] NVARCHAR (1)       NOT NULL,
    [rowguid]       UNIQUEIDENTIFIER   CONSTRAINT [DF_dbDocumentArchiveExclusion_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbDocumentArchiveExclusion] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDocumentArchiveExclusion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDocumentArchiveExclusion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDocumentArchiveExclusion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDocumentArchiveExclusion] TO [OMSApplicationRole]
    AS [dbo];

