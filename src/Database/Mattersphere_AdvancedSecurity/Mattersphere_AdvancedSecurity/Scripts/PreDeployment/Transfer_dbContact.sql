IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'config.dbContact') AND type = 'SN')
BEGIN
	DROP SYNONYM  config.dbContact
END

GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'config.dbContact') AND type = 'U')
BEGIN
	ALTER SCHEMA config TRANSFER dbo.dbContact
END 
GO
