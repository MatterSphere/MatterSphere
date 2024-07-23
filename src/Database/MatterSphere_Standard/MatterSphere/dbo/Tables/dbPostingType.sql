CREATE TABLE [dbo].[dbPostingType] (
    [postID]           INT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [postCode]         [dbo].[uCodeLookup] NULL,
    [postAccCode]      NVARCHAR (30)       NULL,
    [postOfficeDebit]  BIT                 CONSTRAINT [DF_dbPostingType_postOfficeDebit] DEFAULT ((0)) NOT NULL,
    [postOfficeCredit] BIT                 CONSTRAINT [DF_dbPostingType_postOfficeCredit] DEFAULT ((0)) NOT NULL,
    [postClientDebit]  BIT                 CONSTRAINT [DF_dbPostingType_postClientDebit] DEFAULT ((0)) NOT NULL,
    [postClientCredit] BIT                 CONSTRAINT [DF_dbPostingType_postClientCredit] DEFAULT ((0)) NOT NULL,
    [postBankAccNum]   NVARCHAR (50)       NULL,
    [postBackColor]    INT                 NULL,
    [postForeColor]    INT                 NULL,
    [rowguid]          UNIQUEIDENTIFIER    CONSTRAINT [DF_dbPostingType_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbPostingType] PRIMARY KEY CLUSTERED ([postID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbPostingType_rowguid]
    ON [dbo].[dbPostingType]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbPostingType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbPostingType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbPostingType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbPostingType] TO [OMSApplicationRole]
    AS [dbo];

