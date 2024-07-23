

CREATE PROCEDURE [dbo].[fwbsDropColumn]
	@Tablename nvarchar(100) ,   			-- Name of Table from which to drop column 
	@ColumnName nvarchar(100) 		 		-- Column Name
 	
AS
DECLARE 
	 @Script nvarchar(400),    				-- SQL Script string variable
	 @Error int								-- Holds any error code
	 
	 
If  (SELECT [replinfo] from sysobjects where [name] =  @Tablename) = 128
begin
	BEGIN TRANSACTION
	EXEC @Error = sp_repldropcolumn  @source_object =  @tablename
   		,@column =  @columnname
     	,@schema_change_script =  NULL            -- Default.   Path to SQL Script
     	,@force_invalidate_snapshot =  1            -- Default.   Invalidates Snapshot
     	,@force_reinit_subscription =  0  	          -- Default.   Does not re-initialise subscription
	IF @ERROR <> 0
	begin
		ROLLBACK TRANSACTION
		RETURN
	end
	COMMIT TRANSACTION
	RETURN 
end
else
begin
  SET @script = 'ALTER TABLE ' + @tablename + ' DROP  COLUMN ' + @columnname 
  EXEC sp_executesql @script
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsDropColumn] TO [OMSAdminRole]
    AS [dbo];

