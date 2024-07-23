CREATE PROCEDURE [dbo].[PII_DataRemoval] AS

DECLARE @SQL nvarchar(max)
DECLARE @FIELDSQL nvarchar(max)
DECLARE @T1 int
DECLARE @TN1 nvarchar(250)
DECLARE @S2 nvarchar(250)
DECLARE @T2 nvarchar(250)
DECLARE @JF2 nvarchar(250)
DECLARE @JT2 nvarchar(250)
DECLARE @JA2 nvarchar(250)
CREATE TABLE  #T (ID nvarchar(MAX))

DECLARE  Control_Cursor CURSOR FOR 
select distinct ID,[PII_TABLENAME] from [dbo].[PII_Tables] WHERE [PII_ISController] = 1
OPEN Control_Cursor;
/******************* FIRST A CURSOR To POPULATE FROM FOR MAIN TABLES CAUSES AND STRUCTUES FOR LATTER IN PROCEDURE **************/
FETCH NEXT FROM Control_Cursor into @T1,@TN1;
WHILE @@FETCH_STATUS = 0
BEGIN

	DECLARE  Table_Cursor CURSOR FOR 
	select distinct F.[PII_Schema],F.[PII_TableName],J.[FR_JOIN_COLUMN],J.[TO_JOIN_COLUMN],J.[Audit_Removal_link] from [dbo].[PII_Fields] F 
	JOIN [dbo].[PII_Table_join] J ON F.PII_TABLENAME = J.TO_TABLE_NAME
	WHERE F.PII_ControlTableID = @T1
	OPEN Table_Cursor;
	/******************* FIRST A CURSOR To POPULATE FROM FOR MAIN TABLES CAUSES AND STRUCTUES FOR LATTER IN PROCEDURE **************/
	FETCH NEXT FROM Table_Cursor into @S2,@T2,@JF2,@JT2,@JA2;
	WHILE @@FETCH_STATUS = 0
	BEGIN

		/**************************************** Populate temp table with link ID **************************************************/
		IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Configuration' and TABLE_SCHEMA = 'audit')
		BEGIN
		TRUNCATE TABLE #T
		
		select @SQL = 'INSERT INTO #T SELECT DISTINCT D.'+QUOTENAME(@JA2)+'  FROM '+AC.acAuditTableName+' D ' 
			+ ' JOIN '+AC.acTableName+' A ON D.'+QUOTENAME(PTJ.Audit_Removal_Link)+' = A.'+QUOTENAME(FR_Join_column)
			 from [dbo].[PII_Fields] PF
			JOIN [dbo].[PII_Table_join] PTJ ON PTJ.TO_Table_Name = PF.PII_TableName 
			JOIN [audit].[Configuration] AC ON PTJ.TO_Table_Name = replace(replace(reverse(left(reverse([acTableName]),charindex('[',reverse([acTableName])))),'[',''),']','')
			CROSS JOIN [dbo].PII_Tables PT 
			WHERE PT.ID = PTJ.FR_Table_Name
			AND PF.PII_Type = 0
			AND PF.PII_ControlTableID = PT.ID
			AND PF.PII_TableName = @T2
			AND PTJ.FR_Table_Name =@T1
			AND PTJ.Audit_Removal_Link <> ''

		print (@SQL)
		exec sp_executesql @SQL
		END
		/************************ Start Delete **************************/


		set @SQL = null

		select @SQL='Delete D '
		 from [dbo].[PII_Fields] PF
		JOIN [dbo].[PII_Table_join] PTJ ON PTJ.TO_Table_Name = PF.PII_TableName 
		CROSS JOIN [dbo].PII_Tables PT 
		WHERE PT.ID = PTJ.FR_Table_Name
		AND PF.PII_ControlTableID = PT.ID
		AND PF.[PII_Type] = 0
		AND PF.PII_TableName = @T2
		AND PTJ.FR_Table_Name =@T1


		select @SQL=@SQL+' FROM '+QUOTENAME(@S2)+'.'+QUOTENAME(@T2)+' D JOIN [Config].[PII_CHANGE_'+@TN1+ '] O ON O.ID = D.'+QUOTENAME(@JT2)

		BEGIN TRANSACTION
		print (@SQL)
		exec (@SQL)
		COMMIT;

		/*********************** AUDIT Information REMOVEAL ********************************/
		IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Configuration' and TABLE_SCHEMA = 'audit')
		BEGIN
			set @SQL = ''

			select @SQL= 'Delete D  FROM '+AC.acAuditTableName+' D '			
			+ ' JOIN #T O  ON O.ID = D.'+QUOTENAME(@JA2)
		
		
			 from [dbo].[PII_Fields] PF
			JOIN [dbo].[PII_Table_join] PTJ ON PTJ.TO_Table_Name = PF.PII_TableName 
			JOIN [audit].[Configuration] AC ON PTJ.TO_Table_Name = replace(replace(reverse(left(reverse([acTableName]),charindex('[',reverse([acTableName])))),'[',''),']','')
			CROSS JOIN [dbo].PII_Tables PT 
			WHERE PT.ID = PTJ.FR_Table_Name
			AND PF.PII_Type = 0
			AND PF.PII_ControlTableID = PT.ID
			AND PF.PII_TableName = @T2
			AND PTJ.FR_Table_Name =@T1
			AND PTJ.Audit_Removal_Link <> ''

			BEGIN TRANSACTION
				print (@SQL)
				execute as login = 'MSAUDITOR';
				exec sp_executesql @SQL;
				REVERT;
			COMMIT;
		END		
		/************************** END OF AUDIT REMOVAL ****************************/

		

		set @SQL = null

		select @SQL=CASE WHEN @SQL is null then 'Update '+QUOTENAME(@S2)+'.'+QUOTENAME(@T2)+' SET ' else @SQL+' ,' end+ QUOTENAME(PF.[PII_ColumnName]) + ' = ' +CASE WHEN PF.[PII_Obfuscation] IS NULL THEN 'NULL' ELSE ''''+PF.[PII_Obfuscation]+'''' END +''
		 from [dbo].[PII_Fields] PF
		JOIN [dbo].[PII_Table_join] PTJ ON PTJ.TO_Table_Name = PF.PII_TableName 
		CROSS JOIN [dbo].PII_Tables PT 
		WHERE PT.ID = PTJ.FR_Table_Name
		AND PF.PII_ControlTableID = PT.ID
		AND PF.[PII_Type] = 1
		AND PF.PII_TableName = @T2
		AND PTJ.FR_Table_Name =@T1

		select @SQL=@SQL+' FROM '+QUOTENAME(@S2)+'.'+QUOTENAME(@T2)+' JOIN [Config].[PII_CHANGE_'+@TN1+ '] O ON O.ID = '+QUOTENAME(@T2)+'.'+QUOTENAME(@JT2)
		BEGIN TRANSACTION
		print (@SQL)
		exec (@SQL)
		COMMIT;

		set @SQL = null

		select @SQL=CASE WHEN @SQL is null then 'Update '+QUOTENAME(@S2)+'.'+QUOTENAME(@T2)+' SET ' else @SQL+' ,' end+ QUOTENAME(PF.[PII_ColumnName]) + ' = ' +CAST(PF.[PII_RELINKID] as nvarchar(10)) +''
		 from [dbo].[PII_Fields] PF
		JOIN [dbo].[PII_Table_join] PTJ ON PTJ.TO_Table_Name = PF.PII_TableName 
		CROSS JOIN [dbo].PII_Tables PT 
		WHERE PT.ID = PTJ.FR_Table_Name
		AND PF.PII_ControlTableID = PT.ID
		AND PF.[PII_Type] = 2
		AND PF.PII_TableName = @T2
		AND PTJ.FR_Table_Name =@T1

		select @SQL=@SQL+' FROM '+QUOTENAME(@S2)+'.'+QUOTENAME(@T2)+' JOIN [Config].[PII_CHANGE_'+@TN1+ '] O ON O.ID = '+QUOTENAME(@T2)+'.'+QUOTENAME(@JT2)
		BEGIN TRANSACTION
		print (@SQL)
		exec (@SQL)
		COMMIT;

				/*********************** AUDIT Information REMOVEAL ********************************/
		IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Configuration' and TABLE_SCHEMA = 'audit')
		BEGIN
			set @SQL = NULL
			set @FIELDSQL = ''

			select @SQL= 'Delete D  FROM '+AC.acAuditTableName+' D ' 
			+ ' JOIN '+AC.acTableName+' A ON D.'+QUOTENAME(PTJ.Audit_Removal_Link)+' = A.'+QUOTENAME(FR_Join_column)
			+ ' JOIN [Config].[PII_CHANGE_'+@TN1+ '] O  ON O.ID = A.'+QUOTENAME(@JA2)
			+ ' WHERE D.FieldName in (',
			@FIELDSQL=@FIELDSQL+','''+PF.PII_ColumnName+''''
		
			 from [dbo].[PII_Fields] PF
			JOIN [dbo].[PII_Table_join] PTJ ON PTJ.TO_Table_Name = PF.PII_TableName 
			JOIN [audit].[Configuration] AC ON PTJ.TO_Table_Name = replace(replace(reverse(left(reverse([acTableName]),charindex('[',reverse([acTableName])))),'[',''),']','')
			CROSS JOIN [dbo].PII_Tables PT 
			WHERE PT.ID = PTJ.FR_Table_Name
			AND PF.PII_Type between 1 and 2
			AND PF.PII_ControlTableID = PT.ID
			AND PF.PII_TableName = @T2
			AND PTJ.FR_Table_Name =@T1
			AND PTJ.Audit_Removal_Link <> ''

			set @SQL = @SQL+(Stuff(@FieldSQL,1,1,''))+')'

			BEGIN TRANSACTION
				print (@SQL)
				execute as login = 'MSAUDITOR';
				exec (@SQL);
				REVERT;
			COMMIT;
		END		
		/************************** END OF AUDIT REMOVAL ****************************/

		FETCH NEXT FROM Table_Cursor into @S2,@T2,@JF2,@JT2,@JA2;
	END;

	CLOSE Table_Cursor;
	DEALLOCATE Table_Cursor;

	select @SQL='DELETE FROM [Config].[PII_CHANGE_'+@TN1+ '] '
	BEGIN TRANSACTION
	print (@SQL)
	exec (@SQL)
	COMMIT;

	FETCH NEXT FROM Control_Cursor into @T1,@TN1;
END;

CLOSE Control_Cursor;
DEALLOCATE Control_Cursor;

RETURN 0