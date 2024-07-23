DECLARE @DBNAME SYSNAME
DECLARE @SQL nvarchar(max)
DECLARE @CREATESQL nvarchar(max)

SET @DBNAME = DB_NAME () + '_Audit'
set @CREATESQL = 'CREATE SCHEMA [audit]'
SET @sql = N'IF NOT EXISTS (SELECT 1 FROM '+ QUOTENAME(@DBNAME) + '.sys.schemas WHERE name = ''audit'') EXEC '+ QUOTENAME(@DBNAME) + '..sp_executesql @CREATESQL;'
EXEC sp_executesql @sql, N'@CREATESQL NVARCHAR(MAX)', @CREATESQL