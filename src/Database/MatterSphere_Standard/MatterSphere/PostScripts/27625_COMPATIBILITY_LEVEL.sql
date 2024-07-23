DECLARE @cl tinyint
	, @dbname sysname = DB_NAME()
	, @sql nvarchar(max)

SET @cl = (SELECT compatibility_level FROM sys.databases WHERE name = @dbname)
IF @cl > 130 
BEGIN
	SET @sql = N'ALTER DATABASE ' + QUOTENAME(@dbname) + N' SET COMPATIBILITY_LEVEL = 130'
	EXEC sp_executesql @sql
END
