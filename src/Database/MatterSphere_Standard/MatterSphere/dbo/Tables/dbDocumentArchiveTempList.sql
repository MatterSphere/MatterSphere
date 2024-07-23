CREATE TABLE [dbo].[dbDocumentArchiveTempList] (
    [doctypedesc]   NVARCHAR (1000) NULL,
    [docID]         BIGINT          NOT NULL,
    [docDesc]       NVARCHAR (150)  NULL,
    [docType]       NVARCHAR (15)   NULL,
    [Updated]       DATETIME        NULL,
    [UpdatedBy]     INT             NULL,
    [Created]       DATETIME        NULL,
    [ImageColumn]   INT             NULL,
    [Excluded]      BIT             NULL,
    [ClNo]          NVARCHAR (12)   NULL,
    [FileID]        BIGINT          NULL,
    [FileNo]        NVARCHAR (20)   NULL,
    [ClientFileNo]  NVARCHAR (50)   NULL,
    [clName]        NVARCHAR (128)  NULL,
    [fileDesc]      NVARCHAR (255)  NULL,
    [Createdby]     INT             NULL,
    [Requestedby]   INT             NULL,
    [docdirID]      SMALLINT        NULL,
    [ArchiveAction] NVARCHAR (1)    NULL,
    [archInclude]   BIT             NULL
);




GO
CREATE NONCLUSTERED INDEX [dbDocumentArchiveTempList-requestedby-action]
    ON [dbo].[dbDocumentArchiveTempList]([Requestedby] ASC, [ArchiveAction] ASC)
    INCLUDE([ImageColumn]);


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDocumentArchiveTempList] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDocumentArchiveTempList] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDocumentArchiveTempList] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDocumentArchiveTempList] TO [OMSApplicationRole]
    AS [dbo];

