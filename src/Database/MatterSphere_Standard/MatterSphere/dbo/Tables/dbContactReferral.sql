CREATE TABLE [dbo].[dbContactReferral] (
    [refID]      INT                 NOT NULL,
    [contID]     BIGINT              NOT NULL,
    [refContact] BIGINT              NOT NULL,
    [refDate]    SMALLDATETIME       NULL,
    [refDesc]    NVARCHAR (500)      NULL,
    [refBy]      [dbo].[uCodeLookup] NULL,
    [rowguid]    UNIQUEIDENTIFIER    CONSTRAINT [DF_dbContactReferral_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbContactReferall] PRIMARY KEY CLUSTERED ([refID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbContactReferral_dbContact] FOREIGN KEY ([contID]) REFERENCES [dbo].[dbContact] ([contID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbContactReferral_dbContact1] FOREIGN KEY ([refContact]) REFERENCES [dbo].[dbContact] ([contID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbContactReferral_rowguid]
    ON [dbo].[dbContactReferral]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbContactReferral] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbContactReferral] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbContactReferral] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbContactReferral] TO [OMSApplicationRole]
    AS [dbo];

