CREATE PROCEDURE [dbo].[sprAudit_SaveConfiguration] 
(
	 @ID bigint = 0
	 , @TABLENAME nvarchar(max) 
	 , @FRIENDLYTABLENAME nvarchar(max) 
	 , @AUDITTABLENAME nvarchar(max) 
	 , @DELETE bit = 1
	 , @UPDATE bit = 1
	 , @INSERT bit = 1
	 , @RETENTIONPERIOD int = 0
	 , @GROUP nvarchar(max) = null
	 , @ACTIVE bit = 1
	 , @CREATED datetime
	 , @CREATEDBY bigint
	 , @UPDATED datetime
	 , @UPDATEDBY bigint 
) AS

if (@ID = 0) -- Tables are being added for the first time
begin
	if (@TABLENAME is not null)
	begin
		insert into audit.Configuration
		(
			acTableName
			, acTableFriendlyName
			, acAuditTableName
			, acDelete
			, acUpdate
			, acInsert
			, acRetentionPeriod
			, acGroup
			, acActive
			, Created
			, CreatedBy
			, Updated
			, UpdatedBy
		)
		values
		(
			@TABLENAME
			, @FRIENDLYTABLENAME
			, @AUDITTABLENAME
			, @DELETE
			, @UPDATE
			, @INSERT
			, @RETENTIONPERIOD
			, @GROUP
			, @ACTIVE
			, @CREATED
			, @CREATEDBY
			, @UPDATED
			, @UPDATEDBY
		)
	end
end
else -- Table already has an acID and is therefore being edited
begin
	update audit.Configuration
	set 
		acTableFriendlyName = @FRIENDLYTABLENAME
		, acDelete = @DELETE
		, acUpdate = @UPDATE
		, acInsert = @INSERT
		, acRetentionPeriod = @RETENTIONPERIOD
		, acGroup = @GROUP
		, acActive = @ACTIVE
		, Updated = @UPDATED
		, UpdatedBy = @UPDATEDBY
	where
		acID = @ID
end

select 0, 0
GO

