CREATE TABLE [dbo].[dbCaptainsLog] (
    [logID]       BIGINT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [logtypeID]   SMALLINT         NOT NULL,
    [logWhen]     DATETIME         CONSTRAINT [DF_dbCaptainsLog_LogWhen] DEFAULT (getutcdate()) NOT NULL,
    [logusrID]    INT              NULL,
    [logDesc]     NVARCHAR (1200)  NULL,
    [logDataS]    NVARCHAR (100)   NULL,
    [logDataN]    BIGINT           NULL,
    [logDelete]   BIT              CONSTRAINT [DF_dbCaptainsLog_logDelete] DEFAULT ((0)) NOT NULL,
    [logExtended] NVARCHAR (MAX)   NULL,
    [rowguid]     UNIQUEIDENTIFIER CONSTRAINT [DF_dbCaptainsLog_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbCaptainsLog] PRIMARY KEY CLUSTERED ([logID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbCaptainsLog_dbCaptainsLogType] FOREIGN KEY ([logtypeID]) REFERENCES [dbo].[dbCaptainsLogType] ([typeID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbCaptainsLog_dbUser] FOREIGN KEY ([logusrID]) REFERENCES [dbo].[dbUser] ([usrID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbCaptainsLog_rowguid]
    ON [dbo].[dbCaptainsLog]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbCaptainsLog] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbCaptainsLog] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbCaptainsLog] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbCaptainsLog] TO [OMSApplicationRole]
    AS [dbo];

