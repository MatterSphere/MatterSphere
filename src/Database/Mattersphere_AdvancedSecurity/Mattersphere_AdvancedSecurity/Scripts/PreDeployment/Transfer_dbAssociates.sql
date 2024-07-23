IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'config.dbAssociates') AND type = 'SN')
BEGIN
	DROP SYNONYM  config.dbAssociates
END

GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'config.dbAssociates') AND type = 'U')
BEGIN
	ALTER SCHEMA config TRANSFER dbo.dbAssociates
END

GO

