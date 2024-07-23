CREATE PROCEDURE [dbo].[fwbsGrantToRole]  @RoleName nvarchar(100) 
AS
IF NOT EXISTS ( SELECT [name] FROM sysusers WHERE [name] = @RoleName )
BEGIN
	DECLARE @ERR nvarchar(100)
	set @ERR = 'The role '+@RoleName+' does not exist'
	RAISERROR ( @ERR , 10 , 1)
	RETURN
END

DECLARE @procedure nvarchar(250) , @sql nvarchar (500) , @table nvarchar(250) , @tabletype nvarchar(300)

IF ( SELECT LEFT (CONVERT(char(20), SERVERPROPERTY('productversion')),1 ) )= '8'
BEGIN
	RAISERROR ( 'This procedure does not function on this version of SQL Server' , 16 , 1 )
END

ELSE

BEGIN

	-- Apply permissions to tables and views
	DECLARE access_cursor Insensitive Cursor for
	SELECT '[' + Table_Schema + '].[' + Table_Name + ']'  FROM INFORMATION_SCHEMA.TABLES
	UNION
	SELECT '[' + S.[Name] + '].[' + O.[Name] + ']' FROM sysobjects O JOIN sys.schemas S ON s.schema_id = O.uid WHERE [type] = 'IF'
	UNION
	SELECT '[' + S.[Name] + '].[' + O.[Name] + ']' FROM sysobjects O JOIN sys.schemas S ON s.schema_id = O.uid WHERE [type] = 'SN'
	OPEN access_cursor 
	FETCH NEXT FROM access_cursor INTO @table
	WHILE @@fetch_status = 0
		BEGIN 
		SET @sql = 'GRANT SELECT, UPDATE, DELETE, INSERT on ' + @table + ' to '+@RoleName
		EXEC sp_executesql @sql
		FETCH NEXT FROM access_cursor INTO @table
	END

	CLOSE access_cursor
	DEALLOCATE access_cursor



	-- Apply permissions to procs and functions
	IF @RoleName = 'OMSAdminRole'
	BEGIN
		DECLARE access_cursor INSENSITIVE Cursor FOR
			SELECT '[' + U.[Name] + '].[' + O.[Name] + ']' FROM sysobjects O JOIN sys.Schemas U ON U.schema_id = O.uid WHERE (( O.[type] = 'p') OR (O.[type] = 'fn')) AND O.[Category] = 0 ORDER BY 1
		OPEN access_cursor 
		FETCH NEXT FROM access_cursor INTO @Procedure
		WHILE @@fetch_status = 0
		BEGIN 
			SET @sql = 'GRANT execute on ' + @Procedure + ' to '+@RoleName
			EXEC sp_executesql @sql
			FETCH NEXT FROM access_cursor INTO @procedure
		END
	END
	ELSE
	BEGIN
		DECLARE access_cursor INSENSITIVE Cursor FOR
			SELECT '[' + U.[Name] + '].[' + O.[Name] + ']' FROM sysobjects O JOIN sys.Schemas U ON U.schema_id = O.uid WHERE (( O.[type] = 'p'AND O.[name] NOT LIKE 'fwbs%') OR (O.[type] = 'fn')) AND O.[Category] = 0 ORDER BY 1
		OPEN access_cursor 
		FETCH NEXT FROM access_cursor INTO @Procedure
		WHILE @@fetch_status = 0
		BEGIN 
			SET @sql = 'GRANT execute on ' + @Procedure + ' to '+@RoleName
			EXEC sp_executesql @sql
			FETCH NEXT FROM access_cursor INTO @procedure
		END
	END
	CLOSE access_cursor
	DEALLOCATE access_cursor

	-- Apply Permissions to Views
	DECLARE access_cursor Insensitive Cursor for
	SELECT '[' + U.[Name] + '].[' + O.[Name] + ']' FROM sysobjects O JOIN sys.Schemas U ON U.schema_id = O.uid WHERE ( O.[type] = 'v' ) 
	OPEN access_cursor 
	FETCH NEXT FROM access_cursor INTO @table
	WHILE @@fetch_status = 0
		BEGIN 
		SET @sql = 'GRANT SELECT on ' + @table + ' to '+@RoleName
		EXEC sp_executesql @sql
		FETCH NEXT FROM access_cursor INTO @table
	END

	CLOSE access_cursor
	DEALLOCATE access_cursor


	IF EXISTS ( SELECT  1 WHERE CONVERT(int,SERVERPROPERTY('ProductMajorVersion')) >= 11)
	BEGIN
		-- Apply Permissions to Table Functions (split out @ 2012 onwards)
		DECLARE access_cursor Insensitive Cursor for
		SELECT '[' + U.[Name] + '].[' + O.[Name] + ']' FROM sysobjects O JOIN sys.Schemas U ON U.schema_id = O.uid WHERE ( O.[type] = 'tf' ) 
		OPEN access_cursor 
		FETCH NEXT FROM access_cursor INTO @table
		WHILE @@fetch_status = 0
			BEGIN 
			SET @sql = 'GRANT SELECT on ' + @table + ' to '+@RoleName
			EXEC sp_executesql @sql
			FETCH NEXT FROM access_cursor INTO @table
		END

		CLOSE access_cursor
		DEALLOCATE access_cursor
	END

	-- Apply Permissions to TableType

	DECLARE access_cursor INSENSITIVE Cursor FOR
		SELECT '[' + U.[Name] + '].[' + t.[Name] + ']' FROM sys.table_types t JOIN sys.Schemas U ON U.schema_id = t.schema_id  ORDER BY 1
	OPEN access_cursor 
	FETCH NEXT FROM access_cursor INTO @TableType
	WHILE @@fetch_status = 0
	BEGIN 
		SET @sql = 'GRANT execute on TYPE::' + @TableType + ' to '+@RoleName
		EXEC sp_executesql @sql
		FETCH NEXT FROM access_cursor INTO @TableType
	END

	CLOSE access_cursor
	DEALLOCATE access_cursor

END