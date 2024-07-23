CREATE PROCEDURE [dbo].[sprAudit_SetTriggerOnActiveConfigs]  
AS


declare @AUDITING_ENABLED bit = (select isnull(regAuditingEnabled, 0) from dbRegInfo) -- Get Enabled flag from dbRegInfo
declare @SQL nvarchar(max) = ''


Create table #ActiveAuditConfiguration ( 
											TableName nvarchar(256)
											, TriggerName nvarchar(100)
											, isAuditingEnabled bit 
										)


insert into #ActiveAuditConfiguration 
(
	TableName
	, TriggerName
	, isAuditingEnabled
)
select
	right(left([acTableName],charindex('.',[acTableName],charindex('.',[acTableName])+1)-1), charindex('.',reverse(left([acTableName],charindex('.',[acTableName],charindex('.',[acTableName])+1)-1)))-1) -- Schema
	+ '.' + left(right(ac.acTableName, charindex('.', REVERSE(ac.acTableName)) - 1), charIndex(']', right(ac.acTableName, charindex('.', REVERSE(ac.acTableName)) - 1))) as [TableName]
	, t.name as [TriggerName]
	, @AUDITING_ENABLED as [isAuditingEnabled]
from 
	Audit.Configuration ac
left join
	sys.triggers t on t.object_id = ac.acSourceTableTriggerID
where 
	acActive = 1



declare	@TableName nvarchar(256)
		, @TriggerName nvarchar(100)
		, @isAuditingEnabled bit


DECLARE cur CURSOR FOR SELECT TableName, TriggerName, isAuditingEnabled FROM #ActiveAuditConfiguration
OPEN cur

FETCH NEXT FROM cur INTO @TableName, @TriggerName, @isAuditingEnabled

WHILE @@FETCH_STATUS = 0 BEGIN
    
	if (@TriggerName != '' AND @TriggerName is not null)
	begin
		set @SQL = ''
		
		if (@isAuditingEnabled = 1)
		begin
			-- enable trigger
			set @SQL = 'ENABLE TRIGGER ' + @TriggerName + ' ON ' + @TableName 
		end

		if (@isAuditingEnabled = 0)
		begin
			-- disable trigger
			set @SQL = 'DISABLE TRIGGER ' + @triggerName + ' ON ' + @tableName 
		end

		exec(@SQL)
	end	

    FETCH NEXT FROM cur INTO @TableName, @TriggerName, @isAuditingEnabled
END

CLOSE cur    
DEALLOCATE cur

drop table #ActiveAuditConfiguration
