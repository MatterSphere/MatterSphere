CREATE TABLE [dbo].[dbOMSObjects] (
    [ObjCode]           [dbo].[uCodeLookup] NOT NULL,
    [ObjTypeCompatible] NVARCHAR (100)      NOT NULL,
    [ObjSigned]         NVARCHAR (100)      NULL,
    [ObjWinNamespace]   NVARCHAR (100)      NULL,
    [ObjDefaultTabName] [dbo].[uCodeLookup] NULL,
    [ObjWebNameSpace]   NVARCHAR (100)      CONSTRAINT [DF_dbOMSObjects_ObjWebCompatible] DEFAULT ((0)) NULL,
    [ObjPDANameSpace]   NVARCHAR (100)      CONSTRAINT [DF_dbOMSObjects_ObjPDACompatible] DEFAULT ((0)) NULL,
    [ObjType]           NVARCHAR (50)       CONSTRAINT [DF_dbOMSObjects_ObjType] DEFAULT (N'ENQ') NOT NULL,
    [rowguid]           UNIQUEIDENTIFIER    CONSTRAINT [DF_dbOMSObjects_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [ObjAddinFileName]  NVARCHAR (150)      NULL,
    [TileParams]  NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_dbOMSObjects] PRIMARY KEY CLUSTERED ([ObjCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE NONCLUSTERED INDEX [IX_dbOMSObjects_ObjCompatible]
    ON [dbo].[dbOMSObjects]([ObjTypeCompatible] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbOMSObjects_rowguid]
    ON [dbo].[dbOMSObjects]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


CREATE TRIGGER [dbo].[tgrOMSObjectsUpdated]
   ON  [dbo].[dbOMSObjects] 
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

	exec sprTableMonitorUpdate 'dbOMSObjects'   

END


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbOMSObjects] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbOMSObjects] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbOMSObjects] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbOMSObjects] TO [OMSApplicationRole]
    AS [dbo];

