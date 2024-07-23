CREATE TABLE [dbo].[dbPackageSqlScripts] (
    [sqlID]          INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [sqlName]        NVARCHAR (150)   NOT NULL,
    [sqlDescription] NVARCHAR (500)   NULL,
    [sqlScript]      NVARCHAR (MAX)   NULL,
    [rowguid]        UNIQUEIDENTIFIER CONSTRAINT [DF_dbPackageSqlScripts_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbPackageSqlScripts] PRIMARY KEY CLUSTERED ([sqlID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbPackageSqlScripts_rowguid]
    ON [dbo].[dbPackageSqlScripts]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbPackageSqlScripts] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbPackageSqlScripts] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbPackageSqlScripts] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbPackageSqlScripts] TO [OMSApplicationRole]
    AS [dbo];

