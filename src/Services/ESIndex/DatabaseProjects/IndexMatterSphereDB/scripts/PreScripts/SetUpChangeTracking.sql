DECLARE @trackedtables TABLE (tablename NVARCHAR(256))
DECLARE @tablename NVARCHAR(256)
	, @sql NVARCHAR(MAX) = N'ALTER DATABASE ' + QUOTENAME(DB_NAME()) + N' SET CHANGE_TRACKING = ON (CHANGE_RETENTION = 3 DAYS, AUTO_CLEANUP = ON)'

IF CHANGE_TRACKING_CURRENT_VERSION() IS NULL
BEGIN
	PRINT @sql
	EXEC sp_executesql @sql
END

IF EXISTS (SELECT TOP 1 1 FROM sys.synonyms WHERE [schema_id] = schema_id('dbo') AND base_object_name LIKE '_config_.%')
BEGIN
	INSERT INTO @trackedtables (tablename)
	VALUES
		('config.dbAssociates'),
		('config.dbClient'),
		('config.dbContact'),
		('config.dbDocument'),
		('config.dbFile'),
		('dbo.dbUser'),
		('dbo.dbAddress'),
		('dbo.dbPrecedents'),
		('dbo.dbCodeLookup'),
		('dbo.dbClientContacts'),
		('dbo.dbAppointments'),
		('dbo.dbTasks'),
		('relationship.UserGroup_Contact'),
		('relationship.UserGroup_Client'),
		('relationship.UserGroup_Document'),
		('relationship.UserGroup_file')
END
ELSE
BEGIN
	INSERT INTO @trackedtables (tablename)
	VALUES
		('dbo.dbAssociates'),
		('dbo.dbClient'),
		('dbo.dbContact'),
		('dbo.dbDocument'),
		('dbo.dbFile'),
		('dbo.dbUser'),
		('dbo.dbAddress'),
		('dbo.dbPrecedents'),
		('dbo.dbCodeLookup'),
		('dbo.dbClientContacts'),
		('dbo.dbAppointments'),
		('dbo.dbTasks'),
		('relationship.UserGroup_Contact'),
		('relationship.UserGroup_Client'),
		('relationship.UserGroup_Document'),
		('relationship.UserGroup_file')
END

SET @tablename = (SELECT TOP 1 tablename FROM @trackedtables ORDER BY tablename)

WHILE @tablename IS NOT NULL
BEGIN
	SET @sql = N'IF NOT EXISTS(SELECT 1 FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'''+ @tablename + N''')) ALTER TABLE ' + @tablename + N' ENABLE CHANGE_TRACKING'
	PRINT @sql
	EXEC sp_executesql @sql
	SET @tablename = (SELECT TOP 1 tablename FROM @trackedtables WHERE tablename > @tablename ORDER BY tablename)	
END

-- Turn on Column tracking for selected tables
IF EXISTS (SELECT TOP 1 1 FROM sys.synonyms WHERE [schema_id] = schema_id('dbo') AND base_object_name LIKE '_config_.%')
BEGIN
	ALTER TABLE config.dbFile DISABLE CHANGE_TRACKING 
	ALTER TABLE config.dbFile ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED=ON)
	ALTER TABLE config.dbClient DISABLE CHANGE_TRACKING 
	ALTER TABLE config.dbClient ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED=ON)
END
ELSE
BEGIN
	ALTER TABLE dbo.dbFile DISABLE CHANGE_TRACKING 
	ALTER TABLE dbo.dbFile ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED=ON)
	ALTER TABLE dbo.dbClient DISABLE CHANGE_TRACKING 
	ALTER TABLE dbo.dbClient ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED=ON)
END