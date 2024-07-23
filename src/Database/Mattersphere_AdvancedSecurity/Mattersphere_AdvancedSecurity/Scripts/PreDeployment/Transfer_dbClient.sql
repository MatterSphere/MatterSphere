IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'config.dbclient') AND type = 'SN')
BEGIN
	DROP SYNONYM  config.dbclient
END

GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'config.dbClient') AND type = 'U')
BEGIN
	ALTER SCHEMA config TRANSFER dbo.dbClient
END

GO