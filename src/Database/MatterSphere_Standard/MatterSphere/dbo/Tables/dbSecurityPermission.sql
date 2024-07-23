CREATE TABLE [dbo].[dbSecurityPermission] (
    [permID]      BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [permRole]    [dbo].[uCodeLookup] NULL,
    [usrID]       INT                 NULL,
    [permType]    [dbo].[uCodeLookup] NOT NULL,
    [permObjectS] NVARCHAR (50)       NULL,
    [permObjectN] BIGINT              NULL,
    [permCode]    [dbo].[uCodeLookup] NOT NULL,
    [permValue]   BIT                 CONSTRAINT [DF_dbSecurityPermission_permValue] DEFAULT ((0)) NULL,
    [rowguid]     UNIQUEIDENTIFIER    CONSTRAINT [DF_dbSecurityPermission_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbRoleItems] PRIMARY KEY CLUSTERED ([permID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbSecurityPermission_dbSecurityPermissionType] FOREIGN KEY ([permType]) REFERENCES [dbo].[dbSecurityPermissionType] ([permCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbSecurityPermission_dbSecurityRole] FOREIGN KEY ([permRole]) REFERENCES [dbo].[dbSecurityRole] ([roleCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbSecurityPermission_dbUser] FOREIGN KEY ([usrID]) REFERENCES [dbo].[dbUser] ([usrID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbSecurityPermission_rowguid]
    ON [dbo].[dbSecurityPermission]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbSecurityPermission] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbSecurityPermission] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbSecurityPermission] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbSecurityPermission] TO [OMSApplicationRole]
    AS [dbo];

