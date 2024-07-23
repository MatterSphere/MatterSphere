IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'config.dbDocumentPreview') AND type = 'SN')
BEGIN
	DROP SYNONYM  config.dbDocumentPreview
END

GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'config.dbDocumentPreview') AND type = 'U')
BEGIN
	ALTER SCHEMA config TRANSFER dbo.dbDocumentPreview
END
GO
