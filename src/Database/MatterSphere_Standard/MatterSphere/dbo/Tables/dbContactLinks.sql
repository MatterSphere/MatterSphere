CREATE TABLE [dbo].[dbContactLinks] (
    [contID]       BIGINT              NOT NULL,
    [contLinkID]   BIGINT              NOT NULL,
    [contLinkCode] [dbo].[uCodeLookup] NOT NULL,
    [Created]      [dbo].[uCreated]    NULL,
    [CreatedBy]    [dbo].[uCreatedBy]  NULL,
    [rowguid]      UNIQUEIDENTIFIER    CONSTRAINT [DF_dbContactLinks_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbContactLinks] PRIMARY KEY CLUSTERED ([contID] ASC, [contLinkID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbContactLinks_dbContact] FOREIGN KEY ([contID]) REFERENCES [dbo].[dbContact] ([contID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbContactLinks_dbContact1] FOREIGN KEY ([contLinkID]) REFERENCES [dbo].[dbContact] ([contID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbContactLinks_rowguid]
    ON [dbo].[dbContactLinks]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbContactLinks] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbContactLinks] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbContactLinks] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbContactLinks] TO [OMSApplicationRole]
    AS [dbo];

