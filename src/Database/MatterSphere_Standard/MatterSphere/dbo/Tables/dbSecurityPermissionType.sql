CREATE TABLE [dbo].[dbSecurityPermissionType] (
    [permCode] [dbo].[uCodeLookup] NOT NULL,
    [permType] NVARCHAR (100)      NOT NULL,
    [rowguid]  UNIQUEIDENTIFIER    CONSTRAINT [DF_dbSecurityPermissionType_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbSecurityPermissionType] PRIMARY KEY CLUSTERED ([permCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbSecurityPermissionType_rowguid]
    ON [dbo].[dbSecurityPermissionType]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbSecurityPermissionType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbSecurityPermissionType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbSecurityPermissionType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbSecurityPermissionType] TO [OMSApplicationRole]
    AS [dbo];

