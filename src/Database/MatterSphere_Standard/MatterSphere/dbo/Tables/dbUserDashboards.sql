CREATE TABLE [dbo].[dbUserDashboards]
(
    [usrID] INT NOT NULL,
    [dshObjCode] [dbo].[uCodeLookup] NOT NULL, 
    [dshConfig] [dbo].[uXML] NOT NULL, 
    [rowguid] UNIQUEIDENTIFIER CONSTRAINT [DF_dbUserDashboards_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbUserDashboards] PRIMARY KEY CLUSTERED ([usrID], [dshObjCode] ASC) WITH (FILLFACTOR = 90)
)

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbUserDashboards_rowguid]
    ON [dbo].[dbUserDashboards]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbUserDashboards] TO [OMSApplicationRole]
    AS [dbo];

GO
GRANT SELECT
    ON OBJECT::[dbo].[dbUserDashboards] TO [OMSApplicationRole]
    AS [dbo];

GO
GRANT INSERT
    ON OBJECT::[dbo].[dbUserDashboards] TO [OMSApplicationRole]
    AS [dbo];

GO
GRANT DELETE
    ON OBJECT::[dbo].[dbUserDashboards] TO [OMSApplicationRole]
    AS [dbo];