CREATE TABLE [dbo].[dbContactNumbers] (
    [contID]           BIGINT              NOT NULL,
    [contCode]         [dbo].[uCodeLookup] NOT NULL,
    [contNumber]       [dbo].[uTelephone]  NOT NULL,
    [contExtraCode]    [dbo].[uCodeLookup] NOT NULL,
    [contOrder]        TINYINT             CONSTRAINT [DF_dbNumber_telOrder] DEFAULT ((0)) NOT NULL,
    [contActive]       BIT                 CONSTRAINT [DF_dbNumber_telActive] DEFAULT ((1)) NOT NULL,
    [rowguid]          UNIQUEIDENTIFIER    CONSTRAINT [DF_dbContactNumbers_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [contDefaultOrder] TINYINT             DEFAULT ((0)) NULL,
    CONSTRAINT [PK_dbContactNumbers] PRIMARY KEY CLUSTERED ([contID] ASC, [contCode] ASC, [contNumber] ASC, [contExtraCode] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbContactNumbers_dbContact] FOREIGN KEY ([contID]) REFERENCES [dbo].[dbContact] ([contID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbContactNumbers_rowguid]
    ON [dbo].[dbContactNumbers]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbContactNumbers] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbContactNumbers] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbContactNumbers] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbContactNumbers] TO [OMSApplicationRole]
    AS [dbo];

