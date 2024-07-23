CREATE TABLE [dbo].[dbMessage] (
    [msgID]        INT                 NOT NULL,
    [msgCode]      [dbo].[uCodeLookup] NOT NULL,
    [msgSystem]    BIT                 CONSTRAINT [DF_dbMessage_msgSystem] DEFAULT ((0)) NOT NULL,
    [msgSeverity]  TINYINT             CONSTRAINT [DF_dbMessage_msgSeverity] DEFAULT ((0)) NOT NULL,
    [msgException] NVARCHAR (100)      NULL,
    [msgHelpEnum]  NVARCHAR (50)       NULL,
    [msgNotes]     NVARCHAR (100)      NULL,
    [rowguid]      UNIQUEIDENTIFIER    CONSTRAINT [DF_dbMessage_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbMessage] PRIMARY KEY CLUSTERED ([msgID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbMessage_msgCode]
    ON [dbo].[dbMessage]([msgCode] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbMessage_rowguid]
    ON [dbo].[dbMessage]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbMessage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbMessage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbMessage] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbMessage] TO [OMSApplicationRole]
    AS [dbo];

