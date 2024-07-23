CREATE TABLE [dbo].[dbOyezAliases] (
    [frmName]     NVARCHAR (15)    NOT NULL,
    [fldPage]     INT              NOT NULL,
    [fldName]     NVARCHAR (100)   NOT NULL,
    [frmAlias]    NVARCHAR (15)    NOT NULL,
    [fldDesc]     NVARCHAR (255)   NULL,
    [fldAlias]    NVARCHAR (255)   NULL,
    [fldConstant] NVARCHAR (255)   NULL,
    [rowguid]     UNIQUEIDENTIFIER CONSTRAINT [DF_dbOyezAliases_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbOyezAliases] PRIMARY KEY CLUSTERED ([frmName] ASC, [fldPage] ASC, [fldName] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbOyezAliases_rowguid]
    ON [dbo].[dbOyezAliases]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbOyezAliases] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbOyezAliases] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbOyezAliases] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbOyezAliases] TO [OMSApplicationRole]
    AS [dbo];

