CREATE TABLE [dbo].[dbStorageProvider] (
    [spID]    SMALLINT            NOT NULL,
    [spCode]  [dbo].[uCodeLookup] NOT NULL,
    [spType]  NVARCHAR (100)      NOT NULL,
    [spGUID]  UNIQUEIDENTIFIER    NOT NULL,
    [rowguid] UNIQUEIDENTIFIER    CONSTRAINT [DF_dbStorageProvider_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [spType2] NVARCHAR (255)      NULL,
    CONSTRAINT [PK_dbStorageProvider] PRIMARY KEY CLUSTERED ([spID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbStorageProvider_rowguid]
    ON [dbo].[dbStorageProvider]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


CREATE TRIGGER [dbo].[tgrStorageProviderUpdated]
   ON  [dbo].[dbStorageProvider] 
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	exec sprTableMonitorUpdate 'dbStorageProvider'    
END



GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbStorageProvider] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbStorageProvider] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbStorageProvider] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbStorageProvider] TO [OMSApplicationRole]
    AS [dbo];

