CREATE TABLE [dbo].[dbAdminMenu] (
    [admnuID]               INT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [admnuName]             NVARCHAR (50)       NOT NULL,
    [admnuParent]           INT                 NULL,
    [admnuCode]             [dbo].[uCodeLookup] NOT NULL,
    [admnuImageIndex]       SMALLINT            CONSTRAINT [DF_dbAdminMenu_admnuImageIndex] DEFAULT ((12)) NOT NULL,
    [admnuSearchListCode]   NVARCHAR (150)      NULL,
    [admnuVisibleInSideBar] BIT                 CONSTRAINT [DF_dbAdminMenu_admnuVisibleInSideBar] DEFAULT ((0)) NOT NULL,
    [admnuIncFav]           BIT                 CONSTRAINT [DF_dbAdminMenu_admnuIncFav] DEFAULT ((0)) NOT NULL,
    [admnuOrder]            TINYINT             CONSTRAINT [DF_dbAdminMenu_admnuOrder] DEFAULT ((0)) NOT NULL,
    [admnuRoles]            NVARCHAR (150)      NULL,
    [rowguid]               UNIQUEIDENTIFIER    CONSTRAINT [DF_dbAdminMenu_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [admnuSystem]           BIT                 CONSTRAINT [DF_dbAdminMenu_SystemValue] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_dbAdminMenu] PRIMARY KEY CLUSTERED ([admnuID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbAdminMenu_rowguid]
    ON [dbo].[dbAdminMenu]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbAdminMenu] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbAdminMenu] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbAdminMenu] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbAdminMenu] TO [OMSApplicationRole]
    AS [dbo];

