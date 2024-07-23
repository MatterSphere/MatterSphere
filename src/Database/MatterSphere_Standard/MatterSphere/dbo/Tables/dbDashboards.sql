CREATE TABLE [dbo].[dbDashboards]
(
    [dshObjCode] [dbo].[uCodeLookup] NOT NULL,
    [dshSystem] BIT NOT NULL,
    [dshConfig] [dbo].[uXML] NOT NULL, 
    [dshActive] BIT NOT NULL DEFAULT 1, 
    [dshTypeCompatible] TINYINT NOT NULL DEFAULT 0,
    [rowguid] UNIQUEIDENTIFIER CONSTRAINT [DF_dbDashboards_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbDashboards] PRIMARY KEY CLUSTERED ([dshObjCode] ASC) WITH (FILLFACTOR = 90)
)

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDashboards_rowguid]
    ON [dbo].[dbDashboards]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDashboards] TO [OMSApplicationRole]
    AS [dbo];
GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDashboards] TO [OMSApplicationRole]
    AS [dbo];
GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDashboards] TO [OMSApplicationRole]
    AS [dbo];
GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDashboards] TO [OMSApplicationRole]
    AS [dbo];