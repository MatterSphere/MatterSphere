CREATE TABLE [dbo].[dbTimeLedger] (
    [ID]                  BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [fileID]              BIGINT              NULL,
    [clID]                BIGINT              NOT NULL,
    [assocID]             BIGINT              NULL,
    [feeusrID]            INT                 CONSTRAINT [DF_dbTimeLedger_feeusrID] DEFAULT ((0)) NOT NULL,
    [docID]               BIGINT              NULL,
    [timeActivityCode]    [dbo].[uCodeLookup] NOT NULL,
    [timeLegalAidCat]     SMALLINT            NULL,
    [timeLegalAidGrade]   TINYINT             NULL,
    [timeDesc]            NVARCHAR (150)      NOT NULL,
    [timeRecorded]        DATETIME            CONSTRAINT [DF_dbTimeLedger_TimeRecorded] DEFAULT (getutcdate()) NOT NULL,
    [timeUnits]           INT                 NOT NULL,
    [timeMins]            INT                 NOT NULL,
    [timeActualMins]      INT                 CONSTRAINT [DF_dbTimeLedger_timeActualMins] DEFAULT ((0)) NOT NULL,
    [timeFormat]          SMALLINT            CONSTRAINT [DF_dbTimeLedger_timeFormat] DEFAULT ((0)) NOT NULL,
    [timeCost]            MONEY               CONSTRAINT [DF_dbTimeLedger_timeCost] DEFAULT ((0)) NOT NULL,
    [timeActualCost]      MONEY               CONSTRAINT [DF_dbTimeLedger_timeActualCost] DEFAULT ((0)) NOT NULL,
    [timeCharge]          MONEY               CONSTRAINT [DF_dbTimeLedger_timeCharge] DEFAULT ((0)) NOT NULL,
    [timeBand]            INT                 NULL,
    [timeBilled]          BIT                 CONSTRAINT [DF_dbTimeLedger_timeBilled] DEFAULT ((0)) NOT NULL,
    [timeBillNo]          NVARCHAR (30)       NULL,
    [timeBillID]          INT                 NULL,
    [timeStatus]          SMALLINT            CONSTRAINT [DF_dbTimeLedger_timeStatus] DEFAULT ((0)) NOT NULL,
    [timeTransferred]     BIT                 CONSTRAINT [DF_dbTimeLedger_timeTransferred] DEFAULT ((0)) NOT NULL,
    [timeTransferredID]   NVARCHAR (50)       NULL,
    [Created]             [dbo].[uCreated]    CONSTRAINT [DF_dbTimeLedger_Created] DEFAULT (getutcdate()) NOT NULL,
    [Createdby]           [dbo].[uCreatedBy]  CONSTRAINT [DF_dbTimeLedger_Createdby] DEFAULT ((0)) NOT NULL,
    [Updated]             [dbo].[uCreated]    NULL,
    [UpdatedBy]           [dbo].[uCreatedBy]  NULL,
    [timeTransferredDate] DATETIME            NULL,
    [timeBilledDate]      DATETIME            NULL,
    [timeRecoveredDate]   DATETIME            NULL,
    [phID]                BIGINT              NULL,
    [rowguid]             UNIQUEIDENTIFIER    CONSTRAINT [DF_dbTimeLedger_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbTimeLedger] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbTimeLedger_dbActivities] FOREIGN KEY ([timeActivityCode]) REFERENCES [dbo].[dbActivities] ([actCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbTimeLedger_dbAssociates] FOREIGN KEY ([assocID]) REFERENCES [dbo].[dbAssociates] ([assocID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbTimeLedger_dbClient] FOREIGN KEY ([clID]) REFERENCES [dbo].[dbClient] ([clID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbTimeLedger_dbDocument] FOREIGN KEY ([docID]) REFERENCES [dbo].[dbDocument] ([docID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbTimeLedger_dbFile] FOREIGN KEY ([fileID]) REFERENCES [dbo].[dbFile] ([fileID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbTimeLedger_dbFilePhase] FOREIGN KEY ([phID]) REFERENCES [dbo].[dbFilePhase] ([phID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbTimeLedger_dbLegalAidCategory] FOREIGN KEY ([timeLegalAidCat]) REFERENCES [dbo].[dbLegalAidCategory] ([LegAidCategory]) NOT FOR REPLICATION
);




GO
CREATE NONCLUSTERED INDEX [IX_dbTimeLedger_ActivityCode]
    ON [dbo].[dbTimeLedger]([timeActivityCode] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbTimeLedger_clid]
    ON [dbo].[dbTimeLedger]([clID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbTimeLedger_docID]
    ON [dbo].[dbTimeLedger]([docID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbTimeLedger_FileID]
    ON [dbo].[dbTimeLedger]([fileID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbTimeLedger_fileWIP]
    ON [dbo].[dbTimeLedger]([clID] ASC, [fileID] ASC, [timeCharge] ASC, [timeBilled] ASC)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbTimeLedger_Recorded]
    ON [dbo].[dbTimeLedger]([timeRecorded] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbTimeLedger_rowguid]
    ON [dbo].[dbTimeLedger]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbTimeLedger_TransferredID]
    ON [dbo].[dbTimeLedger]([timeTransferredID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbTimeLedger] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbTimeLedger] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbTimeLedger] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbTimeLedger] TO [OMSApplicationRole]
    AS [dbo];

