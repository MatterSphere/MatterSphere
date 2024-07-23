IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'config.dbFile') AND type = 'SN')
BEGIN
	DROP SYNONYM  config.dbFile
END

GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'config.dbFile') AND type = 'U')
BEGIN
	ALTER SCHEMA config TRANSFER dbo.dbFile
END
GO
