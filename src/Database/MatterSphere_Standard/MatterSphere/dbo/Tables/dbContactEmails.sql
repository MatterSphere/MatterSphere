CREATE TABLE [dbo].[dbContactEmails] (
    [contID]           BIGINT              NOT NULL,
    [contEmail]        [dbo].[uEmail]      NOT NULL,
    [contCode]         [dbo].[uCodeLookup] NOT NULL,
    [contOrder]        TINYINT             CONSTRAINT [DF_dbContactEmails_emailOrder] DEFAULT ((0)) NOT NULL,
    [contActive]       BIT                 CONSTRAINT [DF_dbContactEmails_contActive] DEFAULT ((1)) NOT NULL,
    [rowguid]          UNIQUEIDENTIFIER    CONSTRAINT [DF_dbContactEmails_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [contDefaultOrder] TINYINT             DEFAULT ((0)) NULL,
    CONSTRAINT [PK_dbContactEmails] PRIMARY KEY CLUSTERED ([contID] ASC, [contEmail] ASC, [contCode] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbContactEmails_dbContact] FOREIGN KEY ([contID]) REFERENCES [dbo].[dbContact] ([contID]) NOT FOR REPLICATION
);




GO
CREATE NONCLUSTERED INDEX [IX_dbContactEmails_contEmail]
    ON [dbo].[dbContactEmails]([contEmail] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbContactEmails_rowguid]
    ON [dbo].[dbContactEmails]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbContactEmails] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbContactEmails] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbContactEmails] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbContactEmails] TO [OMSApplicationRole]
    AS [dbo];

