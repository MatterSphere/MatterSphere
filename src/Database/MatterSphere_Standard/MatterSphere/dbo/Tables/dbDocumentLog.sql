CREATE TABLE [dbo].[dbDocumentLog] (
    [logID]   BIGINT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [docID]   BIGINT           NOT NULL,
    [verID]   UNIQUEIDENTIFIER NULL,
    [logType] NVARCHAR (15)    NOT NULL,
    [logCode] NVARCHAR (15)    NULL,
    [usrID]   INT              NULL,
    [logTime] DATETIME         CONSTRAINT [DF_dbDocumentLog_LogTime] DEFAULT (getutcdate()) NOT NULL,
    [rowguid] UNIQUEIDENTIFIER CONSTRAINT [DF_dbDocumentLog_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [LogData] NVARCHAR (200)   NULL,
    CONSTRAINT [PK_dbDocumentLog] PRIMARY KEY CLUSTERED ([logID] ASC),
    CONSTRAINT [FK_dbDocumentLog_dbDocument] FOREIGN KEY ([docID]) REFERENCES [dbo].[dbDocument] ([docID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDocumentLog_dbDocumentVersion] FOREIGN KEY ([verID]) REFERENCES [dbo].[dbDocumentVersion] ([verID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDocumentLog_rowguid]
    ON [dbo].[dbDocumentLog]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbDocumentLog_DocID_verID]
    ON [dbo].[dbDocumentLog]([docID] ASC, [verID] ASC)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbDocumentLog_logType]
	ON [dbo].[dbDocumentLog]([logType] ASC) INCLUDE ([docID])
	ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDocumentLog] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDocumentLog] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDocumentLog] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDocumentLog] TO [OMSApplicationRole]
    AS [dbo];

