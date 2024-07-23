CREATE TABLE [dbo].[dbContactSite] (
    [contID]          BIGINT              NOT NULL,
    [contCode]        [dbo].[uCodeLookup] NOT NULL,
    [contSite]        NVARCHAR (255)      NOT NULL,
    [contDescription] NVARCHAR (255)      NULL,
    [contOrder]       TINYINT             NOT NULL,
    [contActive]      BIT                 CONSTRAINT [DF_dbContactSite_contActive] DEFAULT ((1)) NOT NULL,
    [rowguid]         UNIQUEIDENTIFIER    CONSTRAINT [DF_dbContactSite_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbContactSite] PRIMARY KEY CLUSTERED ([contID] ASC),
    CONSTRAINT [FK_dbContactSite_dbContact] FOREIGN KEY ([contID]) REFERENCES [dbo].[dbContact] ([contID])
);




GO
CREATE NONCLUSTERED INDEX [IX_dbContactSite_ContID]
    ON [dbo].[dbContactSite]([contID] ASC)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbContactSite_Rowguid]
    ON [dbo].[dbContactSite]([rowguid] ASC)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbContactSite] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbContactSite] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbContactSite] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbContactSite] TO [OMSApplicationRole]
    AS [dbo];

