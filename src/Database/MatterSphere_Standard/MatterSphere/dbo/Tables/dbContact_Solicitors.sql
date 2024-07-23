CREATE TABLE [dbo].[dbContact_Solicitors] (
    [contID]              BIGINT           NOT NULL,
    [solNumberPartners]   INT              NULL,
    [solNumberFeeEarners] INT              NULL,
    [rowguid]             UNIQUEIDENTIFIER CONSTRAINT [DF_dbContact_Solicitors_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbContact_Solicitors] PRIMARY KEY CLUSTERED ([contID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbContact_Solicitors_dbContact] FOREIGN KEY ([contID]) REFERENCES [dbo].[dbContact] ([contID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbContact_Solicitors_rowguid]
    ON [dbo].[dbContact_Solicitors]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbContact_Solicitors] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbContact_Solicitors] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbContact_Solicitors] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbContact_Solicitors] TO [OMSApplicationRole]
    AS [dbo];

