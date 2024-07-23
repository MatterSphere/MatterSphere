CREATE TABLE [dbo].[dbDocumentArchiveInfo] (
    [ArchID]        BIGINT             IDENTITY (1, 1) NOT NULL,
    [ArchDocID]     BIGINT             NOT NULL,
    [ArchDocFileID] BIGINT             NOT NULL,
    [ArchAction]    NVARCHAR (1)       NULL,
    [ArchMessage]   NVARCHAR (MAX)     NULL,
    [ArchDirID]     SMALLINT           NOT NULL,
    [CreatedBy]     [dbo].[uCreatedBy] NULL,
    [Created]       [dbo].[uCreated]   NULL,
    [RunID]         BIGINT             NOT NULL,
    [Archived]      BIT                NULL,
    [docdirID]      SMALLINT           NULL,
    [ArchStatus]    INT                CONSTRAINT [DF_dbDocumentArchiveInfo_Status] DEFAULT ((1)) NULL,
    [rowguid]       UNIQUEIDENTIFIER   CONSTRAINT [DF_dbDocumentArchiveInfo_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbDocumentArchiveInfo] PRIMARY KEY CLUSTERED ([ArchID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDocumentArchiveInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDocumentArchiveInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDocumentArchiveInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDocumentArchiveInfo] TO [OMSApplicationRole]
    AS [dbo];

