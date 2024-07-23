CREATE TABLE [dbo].[dbPostingEntryType] (
    [postID]                 INT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [postCode]               [dbo].[uCodeLookup] NULL,
    [postGroup]              [dbo].[uCodeLookup] NULL,
    [postOfficeDebit]        BIT                 CONSTRAINT [DF_dbPostingEntryType_postOfficeDebit] DEFAULT ((0)) NOT NULL,
    [postOfficeCredit]       BIT                 CONSTRAINT [DF_dbPostingEntryType_postOfficeCredit] DEFAULT ((0)) NOT NULL,
    [postClientDebit]        BIT                 CONSTRAINT [DF_dbPostingEntryType_postClientDebit] DEFAULT ((0)) NOT NULL,
    [postClientCredit]       BIT                 CONSTRAINT [DF_dbPostingEntryType_postClientCredit] DEFAULT ((0)) NOT NULL,
    [postDefaultDescription] NVARCHAR (30)       NULL,
    [rowguid]                UNIQUEIDENTIFIER    CONSTRAINT [DF_dbPostingEntryType_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_DBPostingEntryType] PRIMARY KEY CLUSTERED ([postID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbPostingEntryType_rowguid]
    ON [dbo].[dbPostingEntryType]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbPostingEntryType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbPostingEntryType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbPostingEntryType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbPostingEntryType] TO [OMSApplicationRole]
    AS [dbo];

