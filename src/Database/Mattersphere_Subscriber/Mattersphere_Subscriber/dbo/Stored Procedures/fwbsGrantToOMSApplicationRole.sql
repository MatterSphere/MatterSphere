

CREATE PROCEDURE [dbo].[fwbsGrantToOMSApplicationRole]
AS

DECLARE  @procedure nvarchar(250) , @sql nvarchar (1000) , @table nvarchar(300)
	
IF NOT EXISTS ( SELECT [name] FROM sysusers WHERE [name] = 'OMSApplicationRole' )
BEGIN
	RAISERROR ( 'The role OMSApplicationRole does not exist' , 10 , 1)
	RETURN
END

-- SQL2000
IF ( SELECT LEFT (CONVERT(char(20), SERVERPROPERTY('productversion')),1 ) )= '8'
BEGIN
	RAISERROR ('This procedure will not function on this version of SQL Server' , 16 , 1)
	---- Apply permissions to stored procedures and functions
	--DECLARE  @procedure nvarchar(250) , @sql nvarchar (1000) , @table nvarchar(300)
	--DECLARE access_cursor INSENSITIVE Cursor FOR
	--SELECT [name] FROM sysobjects WHERE (( [type] = 'p' ) OR ( [type] = 'fn' )) AND [uid] = 1 AND [Category] = 0 order by 1
	--OPEN access_cursor 
	--FETCH NEXT FROM access_cursor INTO @procedure
	--WHILE @@fetch_status = 0
	--BEGIN 
		--SET @sql = 'GRANT execute on ' + @procedure + ' to OMSApplicationRole'
		--EXEC sp_executesql @sql
		--FETCH NEXT FROM access_cursor INTO @procedure
	--END

	--CLOSE access_cursor
	--DEALLOCATE access_cursor
END

ELSE

BEGIN
	SET @SQL = 'GRANT EXECUTE TO OMSApplicationRole'
	EXEC sp_executesql @SQL


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
		SET @sql = 'GRANT SELECT, UPDATE, DELETE, INSERT on ' + @table + ' to OMSApplicationRole'
		EXEC sp_executesql @sql
		FETCH NEXT FROM access_cursor INTO @table
	END

	CLOSE access_cursor
	DEALLOCATE access_cursor

END