

CREATE PROCEDURE [dbo].[fwbsColumnChanges]
	 @spTable nvarchar(100)
	,@spColumn nvarchar(100)
	,@spDefinition nvarchar(300)
AS

DECLARE
	@spTempCol nvarchar(110)
SET @spTempCol = @spColumn + 'Temp'

If  (SELECT [replinfo] from sysobjects where [name] =  @spTable) = 128
begin
BEGIN TRANSACTION

	DECLARE
		 @spPublication nvarchar(200)
		,@ErrorSave int

	SET @spPublication = ( SELECT MSp.[publication] FROM [Distribution].[dbo].[MSpublications] MSp 
				JOIN [Distribution].[dbo].[MSarticles] MSa ON MSp.[Publication_id] = MSa.[publication_id] WHERE MSa.[article] = @spTable AND MSp.[publisher_db] = db_name()  )

		EXEC  @ErrorSave = sp_repladdcolumn  @source_object =  @spTable
    		,@column = @spTempCol
     		,@typetext = @spDefinition 
     		,@publication_to_add =  @spPublication
     		,@schema_change_script =  NULL             
     		,@force_invalidate_snapshot =  0          
     		,@force_reinit_subscription =  0
      
		IF @ErrorSave <> 0
   			GOTO ErrorHandler
		
		EXEC ('UPDATE ' + @spTable + ' 
			SET ' + @spTempCol + ' = ' + @spColumn) 

		IF (@@ERROR <> 0)
   			GOTO ErrorHandler

		EXEC  @ErrorSave = sp_replDropColumn
   			 @source_object = @spTable
   		 	,@Column = @spColumn 

		IF @ErrorSave <> 0
   			GOTO ErrorHandler

		EXEC  @ErrorSave = sp_repladdcolumn  @source_object =  @spTable
    		,@column =  @spColumn
     		,@typetext =  @spDefinition
     		,@publication_to_add =  @spPublication
     		,@schema_change_script =  NULL           
     		,@force_invalidate_snapshot =  1           
     		,@force_reinit_subscription =  0            

		IF @ErrorSave <> 0
   			GOTO ErrorHandler

		EXEC ('UPDATE ' + @spTable + ' 
			SET ' + @spColumn + ' = ' + @spTempCol) 

		IF (@@ERROR <> 0)
  			 GOTO ErrorHandler
 
		EXEC  @ErrorSave = sp_replDropColumn
    		 @source_object = @spTable
    		,@Column = @spTempCol
	
		IF @ErrorSave <> 0
   			GOTO ErrorHandler
		
COMMIT TRANSACTION
	RETURN
	Errorhandler:
	ROLLBACK TRANSACTION
	RETURN
end
else
begin
	EXEC ('ALTER TABLE '  + @spTable + ' ALTER COLUMN ' + @spColumn + ' ' + @spDefinition)
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsColumnChanges] TO [OMSAdminRole]
    AS [dbo];

