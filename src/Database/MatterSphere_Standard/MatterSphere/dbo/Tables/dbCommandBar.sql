CREATE TABLE [dbo].[dbCommandBar] (
    [cbCode]     [dbo].[uCodeLookup] NOT NULL,
    [cbVersion]  BIGINT              CONSTRAINT [DF_dbCommandBar_cbVersion] DEFAULT ((0)) NOT NULL,
    [cbPosition] VARCHAR (30)        CONSTRAINT [DF_dbCommandBar_cbPosition] DEFAULT ('msoBarTop') NOT NULL,
    [cbScript]   [dbo].[uCodeLookup] NULL,
    [rowguid]    UNIQUEIDENTIFIER    CONSTRAINT [DF_dbCommandBar_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbCommandBar] PRIMARY KEY CLUSTERED ([cbCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbCommandBar_rowguid]
    ON [dbo].[dbCommandBar]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbCommandBar] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbCommandBar] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbCommandBar] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbCommandBar] TO [OMSApplicationRole]
    AS [dbo];

