
-- DISABLE THE TGRLICSYCH TRIGGER ON THE DBREGINFO TABLE. THIS INTERFERES WITH AUDITING 
-- AND IS NOT NECESSARY FOR MODERN INSTALLATIONS OF MATTERSPHERE

declare @triggerName nvarchar(150) = 'tgrLicSych'
declare @tableName nvarchar(150) = 'dbRegInfo'
declare @sql nvarchar(max)

if ((select isnull(is_disabled, 0) from sys.triggers where name = @triggerName) = 0)
begin
	set @sql = 'DISABLE TRIGGER ' + @triggerName + ' ON ' + @tableName 
	EXEC sp_executesql @sql
end


