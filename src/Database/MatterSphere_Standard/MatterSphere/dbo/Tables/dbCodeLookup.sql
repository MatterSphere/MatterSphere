CREATE TABLE [dbo].[dbCodeLookup] (
    [cdID]            INT                    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [cdType]          [dbo].[uCodeLookup]    CONSTRAINT [DF_dbCodeLookup_cdType] DEFAULT ('') NOT NULL,
    [cdCode]          [dbo].[uCodeLookup]    NOT NULL,
    [cdUICultureInfo] [dbo].[uUICultureInfo] CONSTRAINT [DF_dbCodeLookup_cdUICultureInfo] DEFAULT ('{default}') NOT NULL,
    [cdDesc]          NVARCHAR (1000)        NULL,
    [cdSystem]        BIT                    CONSTRAINT [DF_dbCodeLookup_cdSystem] DEFAULT ((0)) NOT NULL,
    [cdDeletable]     BIT                    CONSTRAINT [DF_dbCodeLookup_cdDeletable] DEFAULT ((1)) NOT NULL,
    [cdAddLink]       [dbo].[uCodeLookup]    NULL,
    [cdHelp]          NVARCHAR (500)         NULL,
    [cdNotes]         NVARCHAR (500)         NULL,
    [cdGroup]         BIT                    CONSTRAINT [DF_dbCodeLookup_cdGroup] DEFAULT ((0)) NOT NULL,
    [rowguid]         UNIQUEIDENTIFIER       CONSTRAINT [DF_dbCodeLookup_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbCodeLookup] PRIMARY KEY CLUSTERED ([cdID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [CK_dbCodeLookup] CHECK ([cdAddlink]<>''),
    CONSTRAINT [IX_dbCodeLookup] UNIQUE NONCLUSTERED ([cdType] ASC, [cdCode] ASC, [cdUICultureInfo] ASC, [cdAddLink] ASC) WITH (FILLFACTOR = 90) ON [IndexGroup]
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbCodeLookup_rowguid]
    ON [dbo].[dbCodeLookup]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


CREATE TRIGGER [dbo].[tgrCodeLookupUpdated]
   ON  [dbo].[dbCodeLookup] 
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	DELETE dbTableMonitor FROM inserted i JOIN dbTableMonitor tm on i.cdType = tm.Category and tm.TableName = 'dbCodeLookup'
	DELETE dbTableMonitor FROM deleted i JOIN dbTableMonitor tm on i.cdType = tm.Category and tm.TableName = 'dbCodeLookup'

	DECLARE @UTCDATE DATETIME
	SET @UTCDATE = GETUTCDATE()
	
	INSERT dbTableMonitor (TableName, Category, LastUpdated) SELECT DISTINCT 'dbCodeLookup' as TableName, cdType, @UTCDATE as Modified FROM inserted
	INSERT dbTableMonitor (TableName, Category, LastUpdated) SELECT DISTINCT 'dbCodeLookup', cdType, @UTCDATE FROM deleted WHERE NOT EXISTS (SELECT 0 FROM dbTableMonitor WHERE TableName = 'dbCodeLookup' AND Category = deleted.cdType)

END


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbCodeLookup] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbCodeLookup] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbCodeLookup] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbCodeLookup] TO [OMSApplicationRole]
    AS [dbo];

