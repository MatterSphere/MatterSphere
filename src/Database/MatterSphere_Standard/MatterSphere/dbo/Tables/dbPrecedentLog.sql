CREATE TABLE [dbo].[dbPrecedentLog] (
    [logID]   BIGINT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [precid]  BIGINT           NOT NULL,
    [verID]   UNIQUEIDENTIFIER NULL,
    [logType] NVARCHAR (15)    NOT NULL,
    [logCode] NVARCHAR (15)    NULL,
    [usrID]   INT              NULL,
    [logTime] DATETIME         CONSTRAINT [DF_dbPrecedentLog_LogTime] DEFAULT (getutcdate()) NOT NULL,
    [rowguid] UNIQUEIDENTIFIER CONSTRAINT [DF_dbPrecedentLog_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [LogData] NVARCHAR (200)   NULL,
    CONSTRAINT [PK_dbPrecedentLog] PRIMARY KEY CLUSTERED ([logID] ASC),
    CONSTRAINT [FK_dbPrecedentLog_dbPrecedents] FOREIGN KEY ([precid]) REFERENCES [dbo].[dbPrecedents] ([PrecID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbPrecedentLog_dbPrecedentVersion] FOREIGN KEY ([verID]) REFERENCES [dbo].[dbPrecedentVersion] ([verID]) NOT FOR REPLICATION
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbPrecedentLog] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbPrecedentLog] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbPrecedentLog] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbPrecedentLog] TO [OMSApplicationRole]
    AS [dbo];

