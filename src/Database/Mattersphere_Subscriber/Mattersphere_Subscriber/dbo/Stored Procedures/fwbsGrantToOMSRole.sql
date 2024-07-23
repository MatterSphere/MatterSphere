

CREATE PROCEDURE [dbo].[fwbsGrantToOMSRole]
AS
IF NOT EXISTS ( SELECT [name] FROM sysusers WHERE [name] = 'OMSRole' )
BEGIN
	RAISERROR ( 'The role OMSRole does not exist' , 10 , 1)
	RETURN
END

DECLARE @procedure nvarchar(250) , @sql nvarchar (500) , @table nvarchar(250)

IF ( SELECT LEFT (CONVERT(char(20), SERVERPROPERTY('productversion')),1 ) )= '8'
BEGIN
	RAISERROR ( 'This procedure does not function on this version of SQL Server' , 16 , 1 )
	---- Apply permissions to procs and functions
	--DECLARE access_cursor INSENSITIVE Cursor FOR
		--SELECT '[' + U.[Name] + '].[' + O.[Name] + ']' FROM sysobjects O JOIN sysUsers U ON U.uid = O.uid WHERE (( O.[type] = 'p'AND O.[name] NOT LIKE 'fwbs%') OR (O.[type] = 'fn')) AND O.[uid] = 1 AND O.[Category] = 0 ORDER BY 1
	--OPEN access_cursor 
	--FETCH NEXT FROM access_cursor INTO @Procedure
	--WHILE @@fetch_status = 0
	--BEGIN 
		--SET @sql = 'GRANT execute on ' + @Procedure + ' to OMSRole'
		--EXEC sp_executesql @sql
		--FETCH NEXT FROM access_cursor INTO @procedure
	--END

	--CLOSE access_cursor
	--DEALLOCATE access_cursor

	---- Apply Permissions to Views
	--DECLARE access_cursor Insensitive Cursor for
	--SELECT '[' + U.[Name] + '].[' + O.[Name] + ']' FROM sysobjects O JOIN sysUsers U ON U.uid = O.uid WHERE ( O.[type] = 'v' ) AND O.[uid] = 1
	--OPEN access_cursor 
	--FETCH NEXT FROM access_cursor INTO @table
	--WHILE @@fetch_status = 0
		--BEGIN 
		--SET @sql = 'GRANT SELECT on ' + @table + ' to OMSRole'
		--EXEC sp_executesql @sql
		--FETCH NEXT FROM access_cursor INTO @table
	--END

	--CLOSE access_cursor
	--DEALLOCATE access_cursor
	--RETURN
END

ELSE

BEGIN
	-- Apply permissions to procs and functions
	DECLARE access_cursor INSENSITIVE Cursor FOR
		SELECT '[' + U.[Name] + '].[' + O.[Name] + ']' FROM sysobjects O JOIN sys.Schemas U ON U.schema_id = O.uid WHERE (( O.[type] = 'p'AND O.[name] NOT LIKE 'fwbs%') OR (O.[type] = 'fn')) AND O.[Category] = 0 ORDER BY 1
	OPEN access_cursor 
	FETCH NEXT FROM access_cursor INTO @Procedure
	WHILE @@fetch_status = 0
	BEGIN 
		SET @sql = 'GRANT execute on ' + @Procedure + ' to OMSRole'
		EXEC sp_executesql @sql
		FETCH NEXT FROM access_cursor INTO @procedure
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
		SET @sql = 'GRANT SELECT on ' + @table + ' to OMSRole'
		EXEC sp_executesql @sql
		FETCH NEXT FROM access_cursor INTO @table
	END

	CLOSE access_cursor
	DEALLOCATE access_cursor
END