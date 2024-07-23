CREATE TABLE [dbo].[dbFileEvents] (
    [evID]       BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [fileID]     BIGINT              NOT NULL,
    [evType]     [dbo].[uCodeLookup] NOT NULL,
    [evDesc]     NVARCHAR (100)      NULL,
    [evExtended] NVARCHAR (MAX)      NULL,
    [evusrID]    INT                 NULL,
    [evWhen]     DATETIME            CONSTRAINT [DF_dbFileEvents_evWhen] DEFAULT (getutcdate()) NULL,
    [rowguid]    UNIQUEIDENTIFIER    CONSTRAINT [DF_dbFileEvents_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbFileEvents] PRIMARY KEY CLUSTERED ([evID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbFileEvents_dbFile] FOREIGN KEY ([fileID]) REFERENCES [dbo].[dbFile] ([fileID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFileEvents_rowguid]
    ON [dbo].[dbFileEvents]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbFileEvents_FileID]
    ON [dbo].[dbFileEvents]([fileID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFileEvents] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFileEvents] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFileEvents] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFileEvents] TO [OMSApplicationRole]
    AS [dbo];

