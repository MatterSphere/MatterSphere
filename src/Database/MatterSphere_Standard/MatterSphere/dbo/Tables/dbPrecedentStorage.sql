CREATE TABLE [dbo].[dbPrecedentStorage] (
    [precID]   BIGINT           NOT NULL,
    [precBLOB] VARBINARY (MAX)  NOT NULL,
    [rowguid]  UNIQUEIDENTIFIER CONSTRAINT [DF_dbPrecedentStorage_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbPrecedentStorage] PRIMARY KEY CLUSTERED ([precID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbPrecedentStorage_dbPrecedents] FOREIGN KEY ([precID]) REFERENCES [dbo].[dbPrecedents] ([PrecID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbPrecedentStorage_rowguid]
    ON [dbo].[dbPrecedentStorage]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbPrecedentStorage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbPrecedentStorage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbPrecedentStorage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbPrecedentStorage] TO [OMSApplicationRole]
    AS [dbo];

