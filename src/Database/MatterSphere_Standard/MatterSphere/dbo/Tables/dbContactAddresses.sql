CREATE TABLE [dbo].[dbContactAddresses] (
    [contID]     BIGINT              NOT NULL,
    [contaddID]  BIGINT              NOT NULL,
    [contCode]   [dbo].[uCodeLookup] NOT NULL,
    [contOrder]  TINYINT             CONSTRAINT [DF_dbContactAddresses_contOrder] DEFAULT ((0)) NOT NULL,
    [contActive] BIT                 CONSTRAINT [DF_dbContactAddresses_contActive] DEFAULT ((1)) NOT NULL,
    [rowguid]    UNIQUEIDENTIFIER    CONSTRAINT [DF_dbContactAddresses_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbContactAddresses_Code] PRIMARY KEY CLUSTERED ([contID] ASC, [contaddID] ASC, [contCode] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbContactAddresses_dbAddress] FOREIGN KEY ([contaddID]) REFERENCES [dbo].[dbAddress] ([addID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbContactAddresses_dbContact] FOREIGN KEY ([contID]) REFERENCES [dbo].[dbContact] ([contID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbContactAddresses_rowguid]
    ON [dbo].[dbContactAddresses]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbContactAddresses] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbContactAddresses] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbContactAddresses] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbContactAddresses] TO [OMSApplicationRole]
    AS [dbo];

