CREATE TABLE [dbo].[dbDocumentArchiveRun] (
    [RunID]             BIGINT           IDENTITY (1, 1) NOT NULL,
    [runStartTime]      DATETIME         NULL,
    [runEndTime]        DATETIME         NULL,
    [runTotalDocs]      INT              NULL,
    [runProcessedDocs]  INT              NULL,
    [runProcessedFiles] INT              NULL,
    [runCompleted]      BIT              NULL,
    [runMessage]        NVARCHAR (MAX)   NULL,
    [runguid]           UNIQUEIDENTIFIER NULL,
    [rowguid]           UNIQUEIDENTIFIER CONSTRAINT [DF_dbDocumentArchiveRun_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbDocumentArchiveRun] PRIMARY KEY CLUSTERED ([RunID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDocumentArchiveRun] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDocumentArchiveRun] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDocumentArchiveRun] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDocumentArchiveRun] TO [OMSApplicationRole]
    AS [dbo];

