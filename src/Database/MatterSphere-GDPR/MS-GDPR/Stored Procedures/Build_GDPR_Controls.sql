CREATE PROCEDURE [dbo].[Build_GDPR_Controls]
AS
/************ Get Tables *********************/
DECLARE @SCHEMA sysname,@TABLE sysname,@Controller bit,@UIHidden bit
DECLARE @VIEW nvarchar(max)
DECLARE @SQL nvarchar(max)
DECLARE @BASESCHEMA sysname
DECLARE @BASECHEMATABLENAME nvarchar(max) 

DECLARE Table_Cursor CURSOR FOR
select distinct [PII_Schema],[PII_TableName],[PII_IScontroller],[PII_UIHidden] from dbo.PII_Tables;


OPEN Table_Cursor;
FETCH NEXT FROM Table_Cursor into @SCHEMA,@TABLE,@Controller,@UIHidden;
WHILE @@FETCH_STATUS = 0
BEGIN
	SELECT @BASESCHEMA=TABLE_SCHEMA FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME= @TABLE AND TABLE_TYPE ='BASE TABLE' AND TABLE_SCHEMA IN ('dbo', 'config');

	set @VIEW = NULL
	/************************** ADD LAST_ACTIVITY COLUMN TO CONTACT ********************************/
	IF @Controller = 1 
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME= @TABLE AND COLUMN_NAME = 'LAST_ACTIVITY')
		BEGIN
		

			SELECT @SQL= 'BEGIN TRANSACTION
		
			ALTER TABLE '+QUOTENAME(@BASESCHEMA)+'.'+QUOTENAME(@TABLE)+' ADD Last_Activity datetime NULL
			;
			COMMIT
			'

			exec (@SQL)
			print (@SQL)

			IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME= @TABLE AND COLUMN_NAME = 'Updated')
			BEGIN
				SELECT @SQL= 'BEGIN TRANSACTION
		
				UPDATE '+QUOTENAME(@BASESCHEMA)+'.'+QUOTENAME(@TABLE)+' SET Last_Activity = Updated 
				;
				COMMIT
				'

				exec (@SQL)
				print (@SQL)
			END
			ELSE
			BEGIN
				SELECT @SQL= 'BEGIN TRANSACTION
		
				UPDATE '+QUOTENAME(@BASESCHEMA)+'.'+QUOTENAME(@TABLE)+' SET Last_Activity = getdate() 
				;
				COMMIT'

				exec (@SQL)
				print (@SQL)

			END


		END
		/********************* Create Table To Hold PII Columns To be Altered **************************/
		IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME= 'PII_CHANGE_'+@TABLE AND TABLE_SCHEMA = @BASESCHEMA)
		BEGIN
		

			SELECT @SQL= 'BEGIN TRANSACTION
		
			CREATE TABLE [CONFIG].[PII_CHANGE_'+@TABLE+'] ([Id] BIGINT NOT NULL PRIMARY KEY)
			;
			COMMIT;
			GRANT ALTER ON [CONFIG].[PII_CHANGE_'+@TABLE+'] TO [OMSAdminRole];
			GRANT ALTER ON [CONFIG].[PII_CHANGE_'+@TABLE+'] TO [OMSROLE];
			GRANT ALTER ON [CONFIG].[PII_CHANGE_'+@TABLE+'] TO [OMSApplicationRole];
			'

			exec (@SQL)
			print (@SQL)
		END

	END


	/************************** ADD IS HIDDEN COLUMN TO BASE TABLE ********************************/
	IF @UIHidden = 1
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME= @TABLE AND COLUMN_NAME = 'ISHIDDEN')
		BEGIN
		
			SELECT @SQL= 'BEGIN TRANSACTION
		
			ALTER TABLE '+QUOTENAME(@BASESCHEMA)+'.'+QUOTENAME(@TABLE)+' ADD 	ISHIDDEN smallint NULL
			;
			ALTER TABLE '+QUOTENAME(@BASESCHEMA)+'.'+QUOTENAME(@TABLE)+' ADD CONSTRAINT DF_'+@TABLE+'_ISHIDDEN DEFAULT 0 FOR ISHIDDEN
			;
			COMMIT
			'

			exec (@SQL)
			print (@SQL)

		END

				/************************** MOVE BASE TABLE TO PII TABLE ********************************/
		IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME= @TABLE)
		BEGIN	
			IF EXISTS (SELECT 1 FROM SYS.synonyms WHERE [SCHEMA_ID]=SCHEMA_ID('config') AND [NAME]= @TABLE)
			BEGIN	
				SELECT @SQL='BEGIN TRANSACTION 
				DROP SYNONYM [CONFIG].'+QUOTENAME(@TABLE)+';
				COMMIT;'
				exec (@SQL)
				print (@SQL)
			END
			SELECT @SQL='BEGIN TRANSACTION 
			ALTER SCHEMA [config] TRANSFER '+QUOTENAME(@SCHEMA)+'.'+QUOTENAME(@TABLE)+';
			COMMIT;'
			exec (@SQL)
			print (@SQL)
			SET @BASESCHEMA = 'CONFIG'
		END


		/******************** BUILD VIEW AGAINST BASE TABLE SYNONYM PII TABLE *****************************/

		BEGIN
			DECLARE @VIEW_SCHEMA NVARCHAR(150)
			DECLARE @VIEW_NAME NVARCHAR(150)
			DECLARE @OP NVARCHAR(100)
			DECLARE @VIEWSELECT1 nvarchar(max)
			DECLARE @VIEWFROM nvarchar(max) = ' (isnull(TBL.ISHIDDEN,0)=0 OR (config.IsPIAdministrator_NS()) = 1)'

			SET @VIEW_SCHEMA = NULL
			SELECT @VIEW_SCHEMA=LEFT(base_object_name,CHARINDEX('.',base_object_name)-1),@VIEW_NAME=RIGHT(base_object_name,LEN(base_object_name)-CHARINDEX('.',base_object_name)) FROM SYS.SYNONYMS WHERE [schema_id] = schema_id(@SCHEMA) and [name]=@TABLE

			DECLARE @DEF NVARCHAR(MAX)
			SET @DEF = NULL
			select @DEF=REPLACE(REPLACE(REPLACE(REPLACE(VIEW_DEFINITION,CHAR(10),' '),CHAR(13),' '),CHAR(9),' '),'--JOIN / APPLY to table valued function or ADO.NET dynamic SQL fails in matter centre ','')  from INFORMATION_SCHEMA.VIEWS where TABLE_SCHEMA= REPLACE(REPLACE(@VIEW_SCHEMA,'[',''),']','') AND TABLE_NAME = REPLACE(REPLACE(@VIEW_NAME,'[',''),']','')

			IF @DEF is not NULL		
			BEGIN
				DECLARE @TID NVARCHAR(10)
				DECLARE @WORKDEF NVARCHAR(MAX)
                
				SELECT @WORKDEF=RIGHT(@DEF,LEN(@DEF)-charindex(' FROM ',@Def))

				DECLARE @POS1 int = charindex(' AS ',@WORKDEF) +4
				DECLARE @POS2 int = charindex(' ',@WORKDEF,@POS1)

				SELECT @TID=SUBSTRING(@WORKDEF,@POS1,@POS2-@POS1)

			
				IF @DEF not like '% '+REPLACE(@VIEWFROM,'(TBL.','('+@TID+'.')+'%'
				BEGIN
					select @VIEW=REPLACE(@DEF, 'CREATE ','ALTER ')
					set @VIEW = @VIEW+' AND '+ REPLACE(@VIEWFROM,'(TBL.','('+@TID+'.')
				END
			END
			ELSE
			BEGIN
				select @VIEW='CREATE VIEW [config].[vw'+@TABLE+'] AS '


				/* BUILD First Select Base Data to Include HIDDEN if ADMIN */
				select @VIEWSELECT1=' SELECT [TBL].*' 
				select @VIEWSELECT1=@VIEWSELECT1+ ' FROM [config].['+@TABLE + '] AS TBL '
				select @VIEWSELECT1=@VIEWSELECT1+ ' WHERE ' + @VIEWFROM 
			

				select @VIEW=@VIEW+@VIEWSELECT1
			END

		END


		/************************** CREATE VIEW ********************************/
		exec (@VIEW)
		print (@VIEW)


		set @VIEWSELECT1 = null
		set @VIEWFROM = null

		/************************** CREATE SYNONYM ********************************/

		IF NOT EXISTS (select 1 from sys.synonyms where [schema_id]=schema_id(@SCHEMA) AND  [Name] = @TABLE)
		BEGIN	
			SELECT @SQL = 'BEGIN TRANSACTION 
			CREATE SYNONYM '+QUOTENAME(@SCHEMA)+'.'+QUOTENAME(@TABLE)+' FOR [config].[vw'+@TABLE+']
			COMMIT;'
			exec (@SQL)
			print (@SQL)
		END
	END

	/************************* SET Change Tracking ************************/

	  BEGIN
		SET @SQL = 'IF ( NOT EXISTS( SELECT 1 FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID('''+QUOTENAME(@BASESCHEMA)+'.'+@TABLE + ''')))'+
				' ALTER TABLE '+QUOTENAME(@BASESCHEMA)+'.'+ QUOTENAME(@TABLE) + ' ENABLE CHANGE_TRACKING' 
	  END
	  PRINT (@SQL)
	  EXEC( @SQL)

/************************** Refresh VIEWs ********************************/	
		SET @BASECHEMATABLENAME = QUOTENAME(@BASESCHEMA) + N'.' + @TABLE
		SET @VIEW = (
				SELECT TOP 1 QUOTENAME(s.name) + N'.' + QUOTENAME(o.name)
				FROM sys.sql_expression_dependencies ed
					INNER JOIN sys.objects o ON o.object_id = ed.referencing_id
					INNER JOIN sys.schemas s ON s.schema_id = o.schema_id
				WHERE ed.referenced_id = OBJECT_ID(@BASECHEMATABLENAME)
					AND o.type_desc = 'VIEW'
				ORDER BY QUOTENAME(s.name) + N'.' + QUOTENAME(o.name)
				)
		WHILE @VIEW IS NOT NULL
		BEGIN
			EXEC sp_refreshview @VIEW;
			PRINT 'sp_refreshview ' + @VIEW;

			SET @VIEW = (
					SELECT TOP 1 QUOTENAME(s.name) + N'.' + QUOTENAME(o.name)
					FROM sys.sql_expression_dependencies ed
						INNER JOIN sys.objects o ON o.object_id = ed.referencing_id
						INNER JOIN sys.schemas s ON s.schema_id = o.schema_id
					WHERE ed.referenced_id = OBJECT_ID(@BASECHEMATABLENAME)
						AND o.type_desc = 'VIEW'
						AND QUOTENAME(s.name) + N'.' + QUOTENAME(o.name) > @VIEW
					ORDER BY QUOTENAME(s.name) + N'.' + QUOTENAME(o.name)
					)

		END

	FETCH NEXT FROM Table_Cursor into  @SCHEMA,@TABLE,@Controller,@UIHidden;
END;
CLOSE Table_Cursor;
DEALLOCATE Table_Cursor;
