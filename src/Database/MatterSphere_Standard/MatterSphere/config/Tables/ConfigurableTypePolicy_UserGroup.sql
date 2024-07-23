CREATE TABLE [config].[ConfigurableTypePolicy_UserGroup] (
    [UserGroupID]       UNIQUEIDENTIFIER    NOT NULL,
    [PolicyID]          UNIQUEIDENTIFIER    NOT NULL,
    [SecurableType]     [dbo].[uCodeLookup] NOT NULL,
    [SecurableTypeCode] [dbo].[uCodeLookup] NOT NULL,
    [rowguid]           UNIQUEIDENTIFIER    CONSTRAINT [DF_ConfigurableTypePolicy_UserGroup_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_ConfigurableTypePolicy_UserGroup_1] PRIMARY KEY CLUSTERED ([UserGroupID] ASC, [PolicyID] ASC, [SecurableType] ASC, [SecurableTypeCode] ASC),
    CONSTRAINT [FK_ConfigurableTypePolicy_UserGroup_ObjectPolicy] FOREIGN KEY ([PolicyID]) REFERENCES [config].[ObjectPolicy] ([ID]) ON DELETE CASCADE
);




GO
GRANT UPDATE
    ON OBJECT::[config].[ConfigurableTypePolicy_UserGroup] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[ConfigurableTypePolicy_UserGroup] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[ConfigurableTypePolicy_UserGroup] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[ConfigurableTypePolicy_UserGroup] TO [OMSApplicationRole]
    AS [dbo];

