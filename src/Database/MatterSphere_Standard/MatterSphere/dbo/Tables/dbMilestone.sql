CREATE TABLE [dbo].[dbMilestone] (
    [msID]           BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [fileID]         BIGINT              NOT NULL,
    [msCode]         [dbo].[uCodeLookup] NOT NULL,
    [msNextDueDate]  DATETIME            NULL,
    [msNextDueStage] TINYINT             NULL,
    [Created]        [dbo].[uCreated]    NULL,
    [rowguid]        UNIQUEIDENTIFIER    CONSTRAINT [DF_dbMilestone_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbMilestone] PRIMARY KEY CLUSTERED ([fileID] ASC, [msCode] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbMilestone_dbFile] FOREIGN KEY ([fileID]) REFERENCES [dbo].[dbFile] ([fileID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbMilestone_dbMilestoneConfig] FOREIGN KEY ([msCode]) REFERENCES [dbo].[dbMilestoneConfig] ([msCode]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbMilestone_msID]
    ON [dbo].[dbMilestone]([msID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbMilestone_rowguid]
    ON [dbo].[dbMilestone]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbMilestone] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbMilestone] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbMilestone] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbMilestone] TO [OMSApplicationRole]
    AS [dbo];

