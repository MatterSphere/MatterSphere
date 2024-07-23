DECLARE
    @BASECHEMATABLENAME NVARCHAR(50),
    @ColLen INT,
    @VIEW NVARCHAR(MAX),
    @SQL NVARCHAR(MAX)

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbAssociates')
	SET @BASECHEMATABLENAME = N'config.dbAssociates'
ELSE
	SET @BASECHEMATABLENAME = N'dbo.dbAssociates'

SET @ColLen = COL_LENGTH(@BASECHEMATABLENAME, 'assocHeading')	-- NVARCHAR(255)

IF @ColLen < 510
BEGIN

	SET @SQL = N'ALTER TABLE ' + @BASECHEMATABLENAME + N' ALTER COLUMN assocHeading NVARCHAR(255) NULL'
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
