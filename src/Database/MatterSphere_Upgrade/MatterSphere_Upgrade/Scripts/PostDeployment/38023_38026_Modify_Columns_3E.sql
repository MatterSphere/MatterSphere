-- dbContact
DECLARE
    @BASECHEMATABLENAME NVARCHAR(MAX),
    @ColLen1 INT, @ColLen2 INT, @ColLen3 INT,
    @VIEW NVARCHAR(MAX),
    @SQL NVARCHAR(MAX)

IF EXISTS (SELECT 1 
             FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_SCHEMA = 'config'
              AND TABLE_NAME = 'dbContact')
	SET @BASECHEMATABLENAME = N'config.dbContact'
ELSE
	SET @BASECHEMATABLENAME = N'dbo.dbContact'

SET @ColLen1 = COL_LENGTH(@BASECHEMATABLENAME, 'contExtTxtID')	-- NVARCHAR(36)
SET @ColLen2 = COL_LENGTH(@BASECHEMATABLENAME, 'contName')		-- NVARCHAR(128)
SET @ColLen3 = COL_LENGTH(@BASECHEMATABLENAME, 'contNotes')		-- NVARCHAR(4000)

IF @ColLen1 < 72 OR @ColLen2 < 256 OR @ColLen3 < 8000
BEGIN

	SET @SQL = N'DROP INDEX [IX_dbContact_contExtTxtID] ON ' +  @BASECHEMATABLENAME + N'; ' +
		N'ALTER TABLE ' + @BASECHEMATABLENAME + N' ALTER COLUMN contExtTxtID NVARCHAR(36) NULL; ' +
		N'ALTER TABLE ' + @BASECHEMATABLENAME + N' ALTER COLUMN contName NVARCHAR(128) NOT NULL; ' +
		N'ALTER TABLE ' + @BASECHEMATABLENAME + N' ALTER COLUMN contNotes NVARCHAR(4000) NULL; ' +
		N'CREATE NONCLUSTERED INDEX [IX_dbContact_contExtTxtID] ON ' + @BASECHEMATABLENAME + N'([contExtTxtID] ASC) ON [IndexGroup]'

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
GO

-- dbClient
DECLARE
    @BASECHEMATABLENAME NVARCHAR(MAX),
    @ColLen1 INT, @ColLen2 INT,
    @VIEW NVARCHAR(MAX),
    @SQL NVARCHAR(MAX)

IF EXISTS (SELECT 1 
             FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_SCHEMA = 'config'
              AND TABLE_NAME = 'dbClient')
	SET @BASECHEMATABLENAME = N'config.dbClient'
ELSE
	SET @BASECHEMATABLENAME = N'dbo.dbClient'

SET @ColLen1 = COL_LENGTH(@BASECHEMATABLENAME, 'clexttxtID')	-- NVARCHAR(36)
SET @ColLen2 = COL_LENGTH(@BASECHEMATABLENAME, 'clName')		-- NVARCHAR(128)

IF @ColLen1 < 72 OR @ColLen2 < 256
BEGIN

	SET @SQL = N'ALTER TABLE ' + @BASECHEMATABLENAME + N' ALTER COLUMN clexttxtID NVARCHAR(36) NULL; ' +
		N'ALTER TABLE ' + @BASECHEMATABLENAME + N' ALTER COLUMN clName NVARCHAR(128) NOT NULL'

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

SET @BASECHEMATABLENAME = N'dbo.dbDocumentArchiveTempList'
SET @ColLen2 = COL_LENGTH(@BASECHEMATABLENAME, 'clName')		-- NVARCHAR(128)

IF @ColLen2 < 256
BEGIN
	SET @SQL = N'ALTER TABLE ' + @BASECHEMATABLENAME + N' ALTER COLUMN clName NVARCHAR(128) NULL'
	EXEC sp_executesql @SQL
END
GO

-- dbFile
DECLARE
    @BASECHEMATABLENAME NVARCHAR(MAX),
    @ColLen INT,
    @VIEW NVARCHAR(MAX),
    @SQL NVARCHAR(MAX)

IF EXISTS (SELECT 1 
             FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_SCHEMA = 'config'
              AND TABLE_NAME = 'dbFile')
	SET @BASECHEMATABLENAME = N'config.dbFile'
ELSE
	SET @BASECHEMATABLENAME = N'dbo.dbFile'

SET @ColLen = COL_LENGTH(@BASECHEMATABLENAME, 'fileExtLinkTxtID')	-- NVARCHAR(36)

IF @ColLen < 72
BEGIN

	SET @SQL = N'ALTER TABLE ' + @BASECHEMATABLENAME + N' ALTER COLUMN fileExtLinkTxtID NVARCHAR(36) NULL'

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
GO

-- dbActivities
DECLARE
    @BASECHEMATABLENAME NVARCHAR(MAX),
    @ColLen INT,
    @VIEW NVARCHAR(MAX),
    @SQL NVARCHAR(MAX)

SET @BASECHEMATABLENAME = N'dbo.dbActivities'
SET @ColLen = COL_LENGTH(@BASECHEMATABLENAME, 'actAccCode')	-- NVARCHAR(16)

IF @ColLen < 32
BEGIN

	SET @SQL = N'ALTER TABLE ' + @BASECHEMATABLENAME + N' ALTER COLUMN actAccCode NVARCHAR(16) NULL'

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
GO

-- dbDepartment, dbBranch
DECLARE
    @BASECHEMATABLENAME NVARCHAR(MAX),
    @ColLen INT,
    @VIEW NVARCHAR(MAX),
    @SQL NVARCHAR(MAX)

SET @BASECHEMATABLENAME = N'dbo.dbDepartment'
SET @ColLen = COL_LENGTH(@BASECHEMATABLENAME, 'deptAccCode')	-- NVARCHAR(16)

IF @ColLen < 32
BEGIN
	SET @SQL = N'ALTER TABLE ' + @BASECHEMATABLENAME + N' ALTER COLUMN deptAccCode NVARCHAR(16) NULL'
	EXEC sp_executesql @SQL
END

SET @BASECHEMATABLENAME = N'dbo.dbBranch'
SET @ColLen = COL_LENGTH(@BASECHEMATABLENAME, 'brCode')			-- NVARCHAR(16)

IF @ColLen < 32
BEGIN

	SET @SQL = N'ALTER TABLE ' + @BASECHEMATABLENAME + N' ALTER COLUMN brCode NVARCHAR(16) NOT NULL'
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
GO
