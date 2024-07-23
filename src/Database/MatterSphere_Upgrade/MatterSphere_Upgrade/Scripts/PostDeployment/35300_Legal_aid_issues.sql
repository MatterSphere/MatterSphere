SET NOCOUNT ON;
DECLARE @VIEW NVARCHAR(MAX)
	, @BASECHEMATABLENAME NVARCHAR(MAX) 
	, @ColType SYSNAME
	, @SQL NVARCHAR(MAX)


IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbFile')
	SET @BASECHEMATABLENAME = N'config.dbFile'
ELSE
	SET @BASECHEMATABLENAME = N'dbo.dbFile'

SET @ColType = (SELECT t.name FROM sys.columns sc INNER JOIN sys.types t ON t.system_type_id = sc.system_type_id WHERE sc.object_id = OBJECT_ID(@BASECHEMATABLENAME) AND sc.name = 'fileLACategory' )

IF @ColType <> 'smallint'
BEGIN

	SET @SQL = N'ALTER TABLE ' + @BASECHEMATABLENAME + N' ALTER COLUMN fileLACategory SMALLINT NULL
ALTER TABLE ' + @BASECHEMATABLENAME + N' ADD CONSTRAINT [FK_dbFile_dbLegalAidCategory] FOREIGN KEY ([fileLACategory]) REFERENCES [dbo].[dbLegalAidCategory] ([LegAidCategory]) ON UPDATE CASCADE NOT FOR REPLICATION
CREATE NONCLUSTERED INDEX [IX_dbFile_fileLACategory] ON ' + @BASECHEMATABLENAME + N'([fileLACategory] ASC) WITH (FILLFACTOR = 90)'

	EXEC sp_executesql @SQL

/************************** Refresh VIEWs ********************************/		
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
END

IF EXISTS(SELECT * FROM sys.foreign_keys WHERE name = 'FK_dbLegalAidActivities_dbLegalAidCategory' AND update_referential_action <> 1)
BEGIN
	ALTER TABLE dbo.dbLegalAidActivities DROP CONSTRAINT [FK_dbLegalAidActivities_dbLegalAidCategory]
	ALTER TABLE dbo.dbLegalAidActivities ADD CONSTRAINT [FK_dbLegalAidActivities_dbLegalAidCategory] FOREIGN KEY ([ActivityLegalAidCat]) REFERENCES [dbo].[dbLegalAidCategory] ([LegAidCategory]) ON UPDATE CASCADE NOT FOR REPLICATION
END