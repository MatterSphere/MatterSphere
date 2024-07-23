CREATE TABLE [dbo].[dbDirectory] (
    [dirID]        SMALLINT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [brID]         INT                 NULL,
    [dirCode]      [dbo].[uCodeLookup] NOT NULL,
    [dirPath]      [dbo].[uFilePath]   NULL,
    [dirSystem]    BIT                 CONSTRAINT [DF_dbDirectory_dirSystem] DEFAULT ((0)) NOT NULL,
    [dirBrowsable] BIT                 CONSTRAINT [DF_dbDirectory_dirBrowsable] DEFAULT ((0)) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER    CONSTRAINT [DF_dbDirectory_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbDirectory] PRIMARY KEY CLUSTERED ([dirID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbDirectory_dbBranch] FOREIGN KEY ([brID]) REFERENCES [dbo].[dbBranch] ([brID]) NOT FOR REPLICATION,
    CONSTRAINT [IX_dbDirectory] UNIQUE NONCLUSTERED ([brID] ASC, [dirCode] ASC) WITH (FILLFACTOR = 90) ON [IndexGroup]
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDirectory_rowguid]
    ON [dbo].[dbDirectory]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDirectory] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDirectory] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDirectory] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDirectory] TO [OMSApplicationRole]
    AS [dbo];

