CREATE TABLE [dbo].[dbExtensibility] (
    [id]        UNIQUEIDENTIFIER    CONSTRAINT [DF_dbExtensibility_id] DEFAULT (newid()) NOT NULL,
    [code]      [dbo].[uCodeLookup] NOT NULL,
    [type]      NVARCHAR (255)      NOT NULL,
    [behaviour] TINYINT             CONSTRAINT [DF_dbExtensibility_startup] DEFAULT ((1)) NOT NULL,
    [rowguid]   UNIQUEIDENTIFIER    CONSTRAINT [DF_dbExtensibility_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbExtensibility] PRIMARY KEY CLUSTERED ([id] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbExtensibility_rowguid]
    ON [dbo].[dbExtensibility]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


CREATE TRIGGER [dbo].[tgrExtensibilityUpdated]
   ON  [dbo].[dbExtensibility] 
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	exec sprTableMonitorUpdate 'dbExtensibility'   
END


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbExtensibility] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbExtensibility] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbExtensibility] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbExtensibility] TO [OMSApplicationRole]
    AS [dbo];

