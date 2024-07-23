USE master
GO

DECLARE @sql NVARCHAR(MAX) 
	, @dbName SYSNAME 

SET @dbName = N'$(DatabaseName)'
SET @dbName = QUOTENAME(@dbName)
PRINT @dbName
SET @sql = N'ALTER AUTHORIZATION ON DATABASE::' + @dbName + N'TO sa;
ALTER DATABASE ' + @dbName + N' SET TRUSTWORTHY ON;
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name =  N''$(DatabaseName)'' AND is_broker_enabled = 1)
	ALTER DATABASE ' + @dbName + N' SET ENABLE_BROKER;
'

PRINT @sql
EXEC sp_executesql @sql

GO
USE  [$(DatabaseName)]
GO
