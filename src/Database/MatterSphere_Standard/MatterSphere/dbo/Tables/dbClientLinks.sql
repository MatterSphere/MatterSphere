CREATE TABLE [dbo].[dbClientLinks] (
    [clID]       BIGINT              NOT NULL,
    [clLinkID]   BIGINT              NOT NULL,
    [clLinkCode] [dbo].[uCodeLookup] NOT NULL,
    [Created]    [dbo].[uCreated]    NULL,
    [CreatedBy]  [dbo].[uCreatedBy]  NULL,
    [Active]     BIT                 CONSTRAINT [DF_dbClientLinks_Active] DEFAULT ((1)) NOT NULL,
    [rowguid]    UNIQUEIDENTIFIER    CONSTRAINT [DF_dbClientLinks_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK__dbClientLinks] PRIMARY KEY CLUSTERED ([clID] ASC, [clLinkID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbClientLinks_dbClient] FOREIGN KEY ([clID]) REFERENCES [dbo].[dbClient] ([clID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbClientLinks_dbClient1] FOREIGN KEY ([clLinkID]) REFERENCES [dbo].[dbClient] ([clID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbClientLinks_rowguid]
    ON [dbo].[dbClientLinks]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbClientLinks] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbClientLinks] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbClientLinks] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbClientLinks] TO [OMSApplicationRole]
    AS [dbo];

