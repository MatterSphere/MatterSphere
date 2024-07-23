CREATE TABLE [dbo].[dbDateWizJunction] (
    [grpCode]  [dbo].[uCodeLookup] NOT NULL,
    [typeCode] [dbo].[uCodeLookup] NOT NULL,
    [rowguid]  UNIQUEIDENTIFIER    CONSTRAINT [DF_dbDateWizJunction_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbDateWizJunction] PRIMARY KEY CLUSTERED ([grpCode] ASC, [typeCode] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbDateWizJunction_dbDateWizGroups] FOREIGN KEY ([grpCode]) REFERENCES [dbo].[dbDateWizGroups] ([grpCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbDateWizJunction_dbDateWizTypes] FOREIGN KEY ([typeCode]) REFERENCES [dbo].[dbDateWizTypes] ([typeCode]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDateWizJunction_rowguid]
    ON [dbo].[dbDateWizJunction]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDateWizJunction] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDateWizJunction] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDateWizJunction] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDateWizJunction] TO [OMSApplicationRole]
    AS [dbo];

