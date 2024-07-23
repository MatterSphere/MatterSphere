CREATE PROCEDURE [dbo].[sprAudit_SetActiveStatusAndTrigger] 
(
	@auditConfigID int = NULL
	, @tableName nvarchar(50)
	, @sourceTableTriggerID bigint = 0
	, @isActive bit
) AS

declare @isTriggerDisabled bit
declare @triggerName nvarchar(75)
declare @sql nvarchar(max)
declare @isAuditingEnabled bit = (select isnull(regAuditingEnabled, 0) from dbRegInfo)

if (@auditConfigID != 0)
begin
	-- Set the Active flag on the audit config
	update Audit.Configuration
	set acActive = @isActive
	where acID = @auditConfigID

	if (@sourceTableTriggerID != 0)
	begin
		-- Enable/Disable the trigger using the supplied ID in @sourceTableTriggerID
		
		set @isTriggerDisabled = (select is_disabled from sys.triggers where object_id = @sourceTableTriggerID)
		set @triggerName = (select name from sys.triggers where object_id = @sourceTableTriggerID)

		if (@isAuditingEnabled = 1)
		begin
			if (@isTriggerDisabled = 1 and @isActive = 1)
			begin
				-- enable trigger
				set @sql = 'ENABLE TRIGGER ' + @triggerName + ' ON ' + @tableName 
			end
		end

		if (@isTriggerDisabled = 0 and @isActive = 0)
		begin
			-- disable trigger
			set @sql = 'DISABLE TRIGGER ' + @triggerName + ' ON ' + @tableName 
		end

		exec(@sql)
	end
end

select 0, 0