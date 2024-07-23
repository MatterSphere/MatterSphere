CREATE TABLE [dbo].[dbDocumentArchiveDeletionPeriod] (
    [ID]             INT                 IDENTITY (1, 1) NOT NULL,
    [fileType]       [dbo].[uCodeLookup] NULL,
    [brID]           INT                 NULL,
    [deletionPeriod] SMALLINT            NULL,
    [Created]        [dbo].[uCreated]    NULL,
    [CreatedBy]      [dbo].[uCreatedBy]  NULL,
    [Updated]        [dbo].[uCreated]    NULL,
    [UpdatedBy]      [dbo].[uCreatedBy]  NULL,
    [rowguid]        UNIQUEIDENTIFIER    CONSTRAINT [DF_dbDocumentArchiveDeletionPeriod_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbDocumentArchiveDeletionPeriod] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [dbDocumentArchiveDeletionPeriod-filetype-brid]
    ON [dbo].[dbDocumentArchiveDeletionPeriod]([fileType] ASC, [brID] ASC)
    INCLUDE([ID], [deletionPeriod]);


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDocumentArchiveDeletionPeriod] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDocumentArchiveDeletionPeriod] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDocumentArchiveDeletionPeriod] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDocumentArchiveDeletionPeriod] TO [OMSApplicationRole]
    AS [dbo];

