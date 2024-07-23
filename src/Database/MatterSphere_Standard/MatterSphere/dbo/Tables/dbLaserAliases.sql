CREATE TABLE [dbo].[dbLaserAliases] (
    [frmName]     NVARCHAR (15)    NOT NULL,
    [fldName]     NVARCHAR (100)   NOT NULL,
    [frmAlias]    NVARCHAR (15)    NOT NULL,
    [fldDesc]     NVARCHAR (255)   NULL,
    [fldAlias]    NVARCHAR (255)   NULL,
    [fldConstant] NVARCHAR (255)   NULL,
    [fldPage]     INT              CONSTRAINT [DF__dbLaserAliases_fldPage] DEFAULT ((0)) NOT NULL,
    [rowguid]     UNIQUEIDENTIFIER CONSTRAINT [DF_dbLaserAliases_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbLaserAliases] PRIMARY KEY CLUSTERED ([frmName] ASC, [fldName] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbLaserAliases_rowguid]
    ON [dbo].[dbLaserAliases]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbLaserAliases] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbLaserAliases] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbLaserAliases] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbLaserAliases] TO [OMSApplicationRole]
    AS [dbo];

