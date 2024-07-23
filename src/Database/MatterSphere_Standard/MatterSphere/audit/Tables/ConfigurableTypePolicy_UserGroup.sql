CREATE TABLE [audit].[ConfigurableTypePolicy_UserGroup] (
    [AuditID]           UNIQUEIDENTIFIER    CONSTRAINT [DF_ConfigurableTypePolicy_UserGroups_AuditID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [AuditDate]         DATETIME            NOT NULL,
    [AuditUserID]       INT                 NOT NULL,
    [AuditEvent]        [dbo].[uCodeLookup] NOT NULL,
    [UserGroupID]       UNIQUEIDENTIFIER    NOT NULL,
    [PolicyID]          UNIQUEIDENTIFIER    NOT NULL,
    [SecurableType]     [dbo].[uCodeLookup] NOT NULL,
    [SecurableTypeCode] [dbo].[uCodeLookup] NOT NULL,
    [rowguid]           UNIQUEIDENTIFIER    NOT NULL,
    CONSTRAINT [PK_ConfigurableTypePolicy_UserGroup] PRIMARY KEY NONCLUSTERED ([AuditID] ASC) ON [IndexGroup]
);




GO
GRANT UPDATE
    ON OBJECT::[audit].[ConfigurableTypePolicy_UserGroup] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[audit].[ConfigurableTypePolicy_UserGroup] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[audit].[ConfigurableTypePolicy_UserGroup] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[audit].[ConfigurableTypePolicy_UserGroup] TO [OMSApplicationRole]
    AS [dbo];

