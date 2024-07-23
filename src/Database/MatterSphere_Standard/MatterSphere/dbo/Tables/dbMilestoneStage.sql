CREATE TABLE [dbo].[dbMilestoneStage] (
    [datID]         BIGINT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [datmsID]       BIGINT           NULL,
    [datStgID]      TINYINT          NOT NULL,
    [datDue]        DATETIME         NULL,
    [datCompleted]  DATETIME         NULL,
    [datAcheivedBy] INT              NULL,
    [rowguid]       UNIQUEIDENTIFIER CONSTRAINT [DF_dbMilestoneStage_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbMilestoneData] PRIMARY KEY CLUSTERED ([datID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbMilestoneStage_dbMilestone] FOREIGN KEY ([datmsID]) REFERENCES [dbo].[dbMilestone] ([msID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbMilestoneStage_rowguid]
    ON [dbo].[dbMilestoneStage]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbMilestoneStage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbMilestoneStage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbMilestoneStage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbMilestoneStage] TO [OMSApplicationRole]
    AS [dbo];

