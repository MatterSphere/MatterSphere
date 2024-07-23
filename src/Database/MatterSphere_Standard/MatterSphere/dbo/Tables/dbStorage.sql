CREATE TABLE [dbo].[dbStorage] (
    [token]   UNIQUEIDENTIFIER CONSTRAINT [DF_dbStorage_token] DEFAULT (newid()) NOT NULL,
    [data]    VARBINARY (MAX)  NOT NULL,
    [rowguid] UNIQUEIDENTIFIER CONSTRAINT [DF_dbStorage_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbStorage] PRIMARY KEY NONCLUSTERED ([token] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbStorage_rowguid]
    ON [dbo].[dbStorage]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbStorage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbStorage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbStorage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbStorage] TO [OMSApplicationRole]
    AS [dbo];

