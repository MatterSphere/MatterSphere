CREATE TABLE [dbo].[dbAppointments] (
    [appID]           BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [clID]            BIGINT              NOT NULL,
    [fileID]          BIGINT              NOT NULL,
    [assocID]         BIGINT              NULL,
    [docID]           BIGINT              NULL,
    [feeusrID]        INT                 NOT NULL,
    [appRelatedID]    UNIQUEIDENTIFIER    NULL,
    [appType]         [dbo].[uCodeLookup] NULL,
    [appDesc]         NVARCHAR (255)      NULL,
    [appLocation]     NVARCHAR (255)      NULL,
    [appDate]         DATETIME            NOT NULL,
    [appEndDate]      DATETIME            NOT NULL,
    [appAllDay]       BIT                 CONSTRAINT [DF_dbAppointments_appAllDay] DEFAULT ((1)) NOT NULL,
    [appNotes]        NVARCHAR (MAX)      NULL,
    [appAtAssociate]  BIT                 NULL,
    [appReminder]     INT                 CONSTRAINT [DF_dbAppointments_appReminder] DEFAULT ((0)) NOT NULL,
    [appMAPI]         BIT                 CONSTRAINT [DF_dbAppointments_appMAPIStore] DEFAULT ((1)) NOT NULL,
    [appMAPIEntryID]  VARCHAR (300)       NULL,
    [appMAPIFolderID] VARCHAR (300)       NULL,
    [appMAPIStoreID]  VARCHAR (300)       NULL,
    [appDirty]        BIT                 CONSTRAINT [DF_dbAppointments_appDirty] DEFAULT ((1)) NOT NULL,
    [appActive]       BIT                 CONSTRAINT [DF_dbAppointments_appActive] DEFAULT ((1)) NOT NULL,
    [appExternal]     BIT                 CONSTRAINT [DF_dbAppointments_appExternal] DEFAULT ((0)) NOT NULL,
    [Created]         [dbo].[uCreated]    CONSTRAINT [DF_dbAppointments_Created] DEFAULT (getutcdate()) NULL,
    [CreatedBy]       [dbo].[uCreatedBy]  NULL,
    [Updated]         [dbo].[uCreated]    NULL,
    [UpdatedBy]       [dbo].[uCreatedBy]  NULL,
    [rowguid]         UNIQUEIDENTIFIER    CONSTRAINT [DF_dbAppointments_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [appTimeZone]     NVARCHAR (100)      NULL,
    [appExchangeSync] BIT                 CONSTRAINT [DF_dbAppointments_appExchangeSync] DEFAULT ((1)) NOT NULL,
    [appMailbox]      NVARCHAR (200)      NULL,
    [appReminderSet]  BIT                 NULL,
    CONSTRAINT [PK_dbAppointments] PRIMARY KEY CLUSTERED ([appID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbAppointments_dbAssociates] FOREIGN KEY ([assocID]) REFERENCES [dbo].[dbAssociates] ([assocID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbAppointments_dbClient] FOREIGN KEY ([clID]) REFERENCES [dbo].[dbClient] ([clID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbAppointments_dbDocument] FOREIGN KEY ([docID]) REFERENCES [dbo].[dbDocument] ([docID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbAppointments_dbFeeEarner] FOREIGN KEY ([feeusrID]) REFERENCES [dbo].[dbFeeEarner] ([feeusrID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbAppointments_dbFile] FOREIGN KEY ([fileID]) REFERENCES [dbo].[dbFile] ([fileID]) NOT FOR REPLICATION
);




GO
CREATE NONCLUSTERED INDEX [IX_dbAppointments_clid]
    ON [dbo].[dbAppointments]([clID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbAppointments_fileid]
    ON [dbo].[dbAppointments]([fileID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbAppointments_rowguid]
    ON [dbo].[dbAppointments]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbAppointments] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbAppointments] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbAppointments] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbAppointments] TO [OMSApplicationRole]
    AS [dbo];

