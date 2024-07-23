CREATE TABLE [dbo].[dbDateWizTypes] (
    [typeCode]      [dbo].[uCodeLookup] NOT NULL,
    [typeEnquiry]   [dbo].[uCodeLookup] NULL,
    [typeSystem]    BIT                 CONSTRAINT [DF_dbDateWizTypes_TypeBuiltIn] DEFAULT ((0)) NOT NULL,
    [typeInstalled] BIT                 CONSTRAINT [DF_dbDateWizTypes_typeInstalled] DEFAULT ((1)) NOT NULL,
    [rowguid]       UNIQUEIDENTIFIER    CONSTRAINT [DF_dbDateWizTypes_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbDateWizTypes] PRIMARY KEY CLUSTERED ([typeCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDateWizTypes_rowguid]
    ON [dbo].[dbDateWizTypes]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbDateWizTypes] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbDateWizTypes] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbDateWizTypes] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbDateWizTypes] TO [OMSApplicationRole]
    AS [dbo];

