CREATE TABLE [dbo].[dbSecurityRole] (
    [roleCode]   [dbo].[uCodeLookup] NOT NULL,
    [roleLevel]  TINYINT             CONSTRAINT [DF_dbRole_rolLevel] DEFAULT ((0)) NOT NULL,
    [roleSystem] BIT                 CONSTRAINT [DF_dbRole_roleSystem] DEFAULT ((0)) NOT NULL,
    [rowguid]    UNIQUEIDENTIFIER    CONSTRAINT [DF_dbSecurityRole_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbRole] PRIMARY KEY CLUSTERED ([roleCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbSecurityRole_rowguid]
    ON [dbo].[dbSecurityRole]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbSecurityRole] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbSecurityRole] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbSecurityRole] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbSecurityRole] TO [OMSApplicationRole]
    AS [dbo];

