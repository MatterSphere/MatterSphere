CREATE TABLE [dbo].[dbFileLegal] (
    [fileID]            BIGINT              NOT NULL,
    [MatLAContract]     [dbo].[uCodeLookup] NULL,
    [MatLAMatType]      [dbo].[uCodeLookup] NULL,
    [MatLAPartI]        [dbo].[uCodeLookup] NULL,
    [MatLAPartII]       [dbo].[uCodeLookup] NULL,
    [MatEndPoint]       [dbo].[uCodeLookup] NULL,
    [MatLAUFN]          NVARCHAR (50)       NULL,
    [rowguid]           UNIQUEIDENTIFIER    CONSTRAINT [DF_dbFileLegal_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [MatLAConc]         [dbo].[uCodeLookup] NULL,
    [MatLAOutcome]      [dbo].[uCodeLookup] NULL,
    [MatLAStageReached] [dbo].[uCodeLookup] NULL,
    CONSTRAINT [PK_dbFileLegal] PRIMARY KEY CLUSTERED ([fileID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbFileLegal_dbFile] FOREIGN KEY ([fileID]) REFERENCES [dbo].[dbFile] ([fileID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFileLegal_rowguid]
    ON [dbo].[dbFileLegal]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFileLegal] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFileLegal] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFileLegal] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFileLegal] TO [OMSApplicationRole]
    AS [dbo];

