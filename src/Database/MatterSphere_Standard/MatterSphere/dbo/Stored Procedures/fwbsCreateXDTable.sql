CREATE PROCEDURE [dbo].[fwbsCreateXDTable]

	@Datatype nvarchar(100) = NULL ,
	@NewColumn nvarchar(15) = NULL ,
	@XDType nvarchar(50) = NULL ,
	@NewTable nvarchar(50) = NULL ,
	@Switch nvarchar(10) = NULL ,
	@DataLength nvarchar(10)= NULL ,
	@ExtendedProperty nvarchar(50) = NULL


AS
SET NOCOUNT ON

DECLARE 
	@Database nvarchar(100) ,
	@SQL nvarchar(1000) ,
	@PrimaryKey nvarchar(30) ,
	@ntextPointer binary(16) ,
	@Parameter nvarchar(500) ,
	@ColDefinition nvarchar(200)

-- Base select query
SELECT SO.[ID] , SO.[name] , CASE WHEN XD.[extCode] IS NOT NULL THEN 'Yes' ELSE 'No' END as [Registered] ,  CASE WHEN XD.[extCode] IS NOT NULL THEN 7 END as [Image]
FROM sysobjects SO LEFT JOIN dbExtendedData XD ON LEFT ( SO.[name] , 15 ) = XD.[extCode] WHERE SO.[type] = 'u'  and SO.[name] like 'ud%'
ORDER BY SO.[name]

-- Set variables
SET @Database = db_name()
SET @PrimaryKey = ( SELECT dbo.[GetCodeLookupDesc] ( 'KEYFIELD' , @XDType , '{default}' ))
SET @Parameter =  '<params parentRequired=""><param name="@' + @PrimaryKey + '" type="bigint" test="1">%' + UPPER ( @PrimaryKey ) + '%</param></params>' 

-- Create the new table
IF @Switch = 'New Table'
BEGIN	
	BEGIN TRANSACTION
		-- Create new table with primary key
		SET @NewTable = 'ud' + @NewTable
		SET @SQL = 'CREATE TABLE [dbo].' + @NewTable + ' ( ' +  @PrimaryKey + ' bigint Constraint PK_' + @NewTable + ' Primary Key ( '+@PrimaryKey + ' ))'
		EXEC sp_executeSQL @SQL , N'@SQL nvarchar(1000)' , @SQL
		IF @@ERROR <> 0
			GOTO ERRORHANDLER
		-- Create the CodeLookup for Extended data
		INSERT dbCodeLookup ( cdType , cdCode , cdDesc )
		VALUES (  'EXTENDEDDATA' , UPPER ( Left ( @NewTable , 15 ) ) , @ExtendedProperty )
		IF @@ERROR <> 0
			GOTO ERRORHANDLER
		-- Create the CodeLookup forOMS Object
		INSERT dbCodeLookup ( cdType , cdCode , cdDesc )
		VALUES (  'OMSOBJECT' , UPPER ( Left ( @NewTable , 15 ) ) , @ExtendedProperty )
		IF @@ERROR <> 0
			GOTO ERRORHANDLER
		-- Create extended data for the new table
		EXEC sp_addextendedproperty @name =  'MS_Description' , @value = @ExtendedProperty , @level0type = 'User' , @level0name = 'dbo' ,  @level1type = 'Table' , @level1name = @NewTable -- , @level2type = 'Column' , @level2name = @NewColumn
		IF @@ERROR <> 0
			GOTO ERRORHANDLER	
		-- register as an OMS object
		SET @XDType = ( SELECT dbo.[GetCodeLookupDesc] (  'EXDATATYPE' , @XDType ,   '{default}' ))
		INSERT dbo.[dbOMSObjects] ( [ObjCode] , [ObjTypeCompatible] , [ObjWinNamespace] , [ObjType] )
		VALUES ( LEFT ( UPPER ( @NewTable ) , 15 ) , 'FWBS.OMS.'+ @XDType , UPPER ( @NewTable )  , 'ExtData' )  
		IF @@ERROR <> 0
			GOTO ERRORHANDLER
		-- register extended data
		INSERT dbo.[dbExtendedData] ( [extCode] , [extSourceType] , [extCall] , [extWhere] , [extSourceLink] , [extDestLink] , [extModes] , [extSystem] )
		VALUES ( LEFT (  UPPER ( @NewTable) , 15 ) , 'OMS' , 'select * from ' + @NewTable , 'where ' + @PrimaryKey + ' = @' + @PrimaryKey , @PrimaryKey , @PrimaryKey ,15 , 0 )
		IF @@ERROR <> 0
			GOTO ERRORHANDLER
		-- Update extFiled column
		Update dbo.[dbExtendedData] set [extParameters] = @Parameter WHERE [extCode] = LEFT ( @NewTable , 15 )
		IF @@ERROR <> 0
			GOTO ERRORHANDLER
		-- Create the rowguid column
		SET @ColDefinition = 'uniqueidentifier ROWGUIDCOL NOT NULL CONSTRAINT DF_' + @NewTable + '_rowguid DEFAULT (newid())'
		EXECUTE fwbsAddColumn @TableName = @NewTable , @ColumnName = 'rowguid' , @ColumnDesc = @ColDefinition
		-- Now Create the unique index
		SET @SQL = 'CREATE UNIQUE NONCLUSTERED INDEX [IX_' + @NewTable + '_rowguid] ON [dbo].[' + @NewTable + '] ([rowguid] ASC )'
		EXECUTE sp_ExecuteSQL @SQL , N'@SQL nvarchar(1000)' , @SQL
	COMMIT TRANSACTION
	SELECT SO.[ID] , SO.[name] , CASE WHEN XD.[extCode] IS NOT NULL THEN 'Yes' ELSE 'No' END as Registered , CASE WHEN SO.[replinfo] = 128 THEN 7 END as [Image]
	FROM sysobjects SO LEFT JOIN dbExtendedData XD ON LEFT ( SO.[name] , 15 ) = XD.[extCode] WHERE SO.[type] = 'u'  and SO.[name] like 'ud%'
	ORDER BY SO.[name]
END

-- Create the new column
IF @Switch = 'New Column'
BEGIN
	IF @DataType = 'bit'
		SET @DataType = 'bit CONSTRAINT DF_' + @NewTable + '_' + @NewColumn + ' Default (0)'
	SET @ColDefinition = @Datatype + coalesce ( @DataLength , '' )
	EXEC fwbsAddColumn @Tablename = @NewTable , @ColumnName = @NewColumn , @ColumnDesc = @ColDefinition
	IF @@ERROR <> 0
			GOTO ERRORHANDLER
	EXEC sp_addextendedproperty @name =  'MS_Description' , @value = @ExtendedProperty , @level0type = 'User' , @level0name = 'dbo' ,  @level1type = 'Table' , @level1name = @NewTable  , @level2type = 'Column' , @level2name = @NewColumn
	IF @@ERROR <> 0
			GOTO ERRORHANDLER
END

RETURN

ERRORHANDLER:
IF @@Trancount <> 0
ROLLBACK TRANSACTION
SET NOCOUNT OFF
RETURN

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsCreateXDTable] TO [OMSAdminRole]
    AS [dbo];

