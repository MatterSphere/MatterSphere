CREATE TABLE [dbo].[dbTasks] (
    [tskID]           BIGINT              IDENTITY (1000, 1) NOT FOR REPLICATION NOT NULL,
    [fileID]          BIGINT              NOT NULL,
    [feeusrID]        INT                 NOT NULL,
    [docID]           BIGINT              NULL,
    [tskRelatedID]    UNIQUEIDENTIFIER    NULL,
    [tskType]         [dbo].[uCodeLookup] NULL,
    [tskDesc]         NVARCHAR (100)      NOT NULL,
    [tskDue]          DATETIME            NULL,
    [tskNotes]        NVARCHAR (MAX)      NULL,
    [tskCompleted]    DATETIME            NULL,
    [tskComplete]     BIT                 CONSTRAINT [DF_dbTasks_tskComplete] DEFAULT ((0)) NOT NULL,
    [tskReminder]     DATETIME            NULL,
    [tskMAPI]         BIT                 CONSTRAINT [DF_dbTasks_tskMAPI] DEFAULT ((1)) NOT NULL,
    [tskMAPIEntryID]  VARCHAR (300)       NULL,
    [tskMAPIFolderID] VARCHAR (300)       NULL,
    [tskMAPIStoreID]  VARCHAR (300)       NULL,
    [tskDirty]        BIT                 CONSTRAINT [DF_dbTasks_tskDirty] DEFAULT ((1)) NOT NULL,
    [tskActive]       BIT                 CONSTRAINT [DF_dbTasks_tskActive] DEFAULT ((1)) NOT NULL,
    [Created]         [dbo].[uCreated]    NULL,
    [CreatedBy]       [dbo].[uCreatedBy]  NULL,
    [Updated]         [dbo].[uCreated]    NULL,
    [UpdatedBy]       [dbo].[uCreatedBy]  NULL,
    [rowguid]         UNIQUEIDENTIFIER    CONSTRAINT [DF_dbTasks_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [tskCompletedBy]  [dbo].[uCreatedBy]  NULL,
    [tskFilter]       NVARCHAR (50)       NULL,
    [tskGroup]        [dbo].[uCodeLookup] NULL,
    [tskManual]       BIT                 CONSTRAINT [DF_dbTasks_tskManual] DEFAULT ((1)) NOT NULL,
    [tskMSPlan]       [dbo].[uCodeLookup] NULL,
    [tskMSStage]      TINYINT             NULL,
    [tmID]            INT                 NULL,
    [usrID]           INT                 NULL,
    [phID]            BIGINT              NULL,
    [tskCategory]     [dbo].[uCodeLookup] NULL,
    [tskEscalatedID]  BIGINT              NULL,
    [tskImportance]   TINYINT             NULL,
    [tskMileage]      SMALLINT            NULL,
    [tskPriority]     TINYINT             NULL,
    [tskRole]         [dbo].[uCodeLookup] NULL,
    [tskSuspended]    [dbo].[uCreated]    NULL,
    [tskSuspendedBy]  [dbo].[uCreatedBy]  NULL,
    [tskTimeEstimate] SMALLINT            NULL,
    [tskTimeTaken]    SMALLINT            NULL,
    CONSTRAINT [PK_dbTasks] PRIMARY KEY NONCLUSTERED ([tskID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbTasks_dbDocument] FOREIGN KEY ([docID]) REFERENCES [dbo].[dbDocument] ([docID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbTasks_dbFeeEarner] FOREIGN KEY ([feeusrID]) REFERENCES [dbo].[dbFeeEarner] ([feeusrID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbTasks_dbFile] FOREIGN KEY ([fileID]) REFERENCES [dbo].[dbFile] ([fileID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbUser_dbTasks_usrID] FOREIGN KEY ([usrID]) REFERENCES [dbo].[dbUser] ([usrID]) NOT FOR REPLICATION
);

GO
CREATE CLUSTERED INDEX [CLX_dbTasks_FileID_tskActive_usrID_feeusrID]
    ON [dbo].[dbTasks](fileID, tskActive, usrID, feeusrID)


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbTasks_rowguid]
    ON [dbo].[dbTasks]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbTasks_FileID]
    ON [dbo].[dbTasks]([fileID] ASC, [Updated] ASC, [Created] ASC) WITH (FILLFACTOR = 95)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbTasks] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbTasks] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbTasks] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbTasks] TO [OMSApplicationRole]
    AS [dbo];

