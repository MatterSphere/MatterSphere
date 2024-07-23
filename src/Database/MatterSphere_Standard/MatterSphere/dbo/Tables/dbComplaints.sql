CREATE TABLE [dbo].[dbComplaints] (
    [compID]             INT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [fileID]             BIGINT              NULL,
    [clID]               BIGINT              NOT NULL,
    [compRef]            NVARCHAR (30)       NULL,
    [compDesc]           NVARCHAR (50)       NOT NULL,
    [compfeeID]          INT                 CONSTRAINT [DF_dbComplaints_compfeeID] DEFAULT ((0)) NOT NULL,
    [compNote]           NVARCHAR (MAX)      NULL,
    [compCompleted]      DATETIME            NULL,
    [compCompletedusrID] INT                 NULL,
    [compEstCompDate]    DATETIME            NULL,
    [compType]           [dbo].[uCodeLookup] NULL,
    [compActive]         BIT                 CONSTRAINT [DF_dbComplaints_compActive] DEFAULT ((1)) NOT NULL,
    [Created]            [dbo].[uCreated]    NULL,
    [CreatedBy]          [dbo].[uCreatedBy]  NULL,
    [rowguid]            UNIQUEIDENTIFIER    CONSTRAINT [DF_dbComplaints_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbComplaints] PRIMARY KEY CLUSTERED ([compID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbComplaints_dbClient] FOREIGN KEY ([clID]) REFERENCES [dbo].[dbClient] ([clID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbComplaints_dbFeeEarner] FOREIGN KEY ([compfeeID]) REFERENCES [dbo].[dbFeeEarner] ([feeusrID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbComplaints_dbFile] FOREIGN KEY ([fileID]) REFERENCES [dbo].[dbFile] ([fileID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbComplaints_rowguid]
    ON [dbo].[dbComplaints]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbComplaints] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbComplaints] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbComplaints] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbComplaints] TO [OMSApplicationRole]
    AS [dbo];

