CREATE TABLE [dbo].[dbTracking] (
    [trackID]           BIGINT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [logID]             INT              NULL,
    [logType]           NVARCHAR (15)    NULL,
    [trackCheckedOut]   DATETIME         NULL,
    [trackCheckedOutBy] INT              NULL,
    [trackIssuedTo]     BIGINT           NULL,
    [trackIssueExpiry]  DATETIME         NULL,
    [trackCheckedIn]    DATETIME         NULL,
    [trackCheckedInBy]  INT              NULL,
    [trackLocReference] NVARCHAR (50)    NULL,
    [rowguid]           UNIQUEIDENTIFIER CONSTRAINT [DF_dbTracking_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
	[trackNotes]        NVARCHAR (200) NULL,
    CONSTRAINT [PK_dbArchiveEvents] PRIMARY KEY CLUSTERED ([trackID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbTracking_rowguid]
    ON [dbo].[dbTracking]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];

GO
CREATE NONCLUSTERED INDEX IX_dbTracking_logID
    ON dbo.dbTracking(logID, trackCheckedIn) WITH (FILLFACTOR = 90)
    ON IndexGroup;

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbTracking] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbTracking] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbTracking] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbTracking] TO [OMSApplicationRole]
    AS [dbo];

