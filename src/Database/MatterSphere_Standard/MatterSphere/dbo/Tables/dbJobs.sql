CREATE TABLE [dbo].[dbJobs] (
    [usrID]          INT              NOT NULL,
    [jobID]          BIGINT           NOT NULL,
    [jobOrder]       SMALLINT         CONSTRAINT [DF_dbPrecedentMulti_precOrder] DEFAULT ((0)) NOT NULL,
    [precID]         BIGINT           NOT NULL,
    [assocID]        BIGINT           NOT NULL,
    [feeusrID]       INT              NULL,
    [jobSaveMode]    SMALLINT         CONSTRAINT [DF_dbPrecedentMulti_precAutoSave] DEFAULT ((0)) NOT NULL,
    [jobPrintMode]   SMALLINT         CONSTRAINT [DF_dbJobs_jobPrintMode] DEFAULT ((0)) NOT NULL,
    [jobNewTemplate] BIT              CONSTRAINT [DF_dbJobs_jobNew] DEFAULT ((0)) NOT NULL,
    [jobHasError]    BIT              CONSTRAINT [DF_dbJobs_jobError] DEFAULT ((0)) NOT NULL,
    [jobError]       NVARCHAR (100)   NULL,
    [jobStatus]      TINYINT          CONSTRAINT [DF_dbJobs_jobStatus] DEFAULT ((0)) NOT NULL,
    [jobXML]         [dbo].[uXML]     CONSTRAINT [DF_dbJobs_jobXML] DEFAULT (N'<config />') NOT NULL,
    [rowguid]        UNIQUEIDENTIFIER CONSTRAINT [DF_dbJobs_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbJobs] PRIMARY KEY CLUSTERED ([usrID] ASC, [jobID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbJobs_dbAssociates] FOREIGN KEY ([assocID]) REFERENCES [dbo].[dbAssociates] ([assocID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbJobs_dbFeeEarner] FOREIGN KEY ([feeusrID]) REFERENCES [dbo].[dbFeeEarner] ([feeusrID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbJobs_dbPrecedents] FOREIGN KEY ([precID]) REFERENCES [dbo].[dbPrecedents] ([PrecID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbJobs_dbUser] FOREIGN KEY ([usrID]) REFERENCES [dbo].[dbUser] ([usrID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbJobs_rowguid]
    ON [dbo].[dbJobs]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbJobs] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbJobs] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbJobs] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbJobs] TO [OMSApplicationRole]
    AS [dbo];

