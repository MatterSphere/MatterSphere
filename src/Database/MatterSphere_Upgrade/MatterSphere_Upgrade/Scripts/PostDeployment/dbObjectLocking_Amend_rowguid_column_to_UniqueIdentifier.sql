
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dbObjectLocking]') AND type in (N'U'))
BEGIN
	IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_dbObjectLocking_rowguid]') AND type = 'D')
	BEGIN
		ALTER TABLE dbObjectLocking DROP CONSTRAINT [DF_dbObjectLocking_rowguid]
	END

	ALTER TABLE dbObjectLocking ALTER COLUMN rowguid uniqueidentifier not null
	ALTER TABLE dbObjectLocking ADD  CONSTRAINT [DF_dbObjectLocking_rowguid]  DEFAULT (newid()) FOR [rowguid]
END
