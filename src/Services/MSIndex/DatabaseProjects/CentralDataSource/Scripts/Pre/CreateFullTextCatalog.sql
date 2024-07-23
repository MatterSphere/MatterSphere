IF (FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') <> 1)
BEGIN
	RAISERROR ('Full-Text feature is NOT installed on SQL Server.', 18, 1)
END

IF ((SELECT is_fulltext_enabled FROM sys.databases WHERE database_id = DB_ID()) = 0)
BEGIN
	EXEC sp_fulltext_database 'enable';
END

IF NOT EXISTS (SELECT * FROM sys.fulltext_catalogs WHERE name = 'FullTextCatalog')
BEGIN
	CREATE FULLTEXT CATALOG FullTextCatalog AS DEFAULT;
END

GO