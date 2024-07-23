CREATE TABLE [dbo].[dbDateWizGroups] (
    [grpCode]      [dbo].[uCodeLookup] NOT NULL,
    [grpInstalled] BIT                 CONSTRAINT [DF_dbDateWizGroups_grpInstalled] DEFAULT ((1)) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER    CONSTRAINT [DF_dbDateWizGroups_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbDateWizGroups] PRIMARY KEY CLUSTERED ([grpCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDateWizGroups_rowguid]
    ON [dbo].[dbDateWizGroups]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDateWizGroups] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDateWizGroups] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDateWizGroups] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDateWizGroups] TO [OMSApplicationRole]
    AS [dbo];

