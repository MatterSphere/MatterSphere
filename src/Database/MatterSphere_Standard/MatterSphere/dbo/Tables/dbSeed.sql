CREATE TABLE [dbo].[dbSeed] (
    [brID]         INT                 NOT NULL,
    [seedType]     [dbo].[uCodeLookup] NOT NULL,
    [seedAuto]     BIT                 CONSTRAINT [DF_dbSeed_seedAuto] DEFAULT ((1)) NOT NULL,
    [seedPrefix]   NVARCHAR (20)       NULL,
    [seedSuffix]   NVARCHAR (20)       NULL,
    [seedLastUsed] BIGINT              CONSTRAINT [DF_dbSeed_seedLastUsed] DEFAULT ((1)) NOT NULL,
    [seedSQL]      NVARCHAR (500)      NULL,
    [rowguid]      UNIQUEIDENTIFIER    CONSTRAINT [DF_dbSeed_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbSeed] PRIMARY KEY CLUSTERED ([brID] ASC, [seedType] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbSeed_dbBranch] FOREIGN KEY ([brID]) REFERENCES [dbo].[dbBranch] ([brID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbSeed_rowguid]
    ON [dbo].[dbSeed]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbSeed] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbSeed] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbSeed] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbSeed] TO [OMSApplicationRole]
    AS [dbo];

