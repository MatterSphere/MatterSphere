IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'dbFileType' AND COLUMN_NAME = 'filePrecMinorCategory')
BEGIN

	EXEC dbo.fwbsaddcolumn @tablename = 'dbo.dbFileType' , @columnName = 'filePrecMinorCategory' , @columnDesc = 'dbo.uCodeLookup NULL'

	DECLARE @VIEW NVARCHAR(MAX)
	DECLARE @BASECHEMATABLENAME NVARCHAR(max) = N'dbo.dbFileType'

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
	DECLARE @sql NVARCHAR(MAX) = 'UPDATE dbo.dbFileType SET filePrecMinorCategory = ''TEMPLATE'' WHERE typeCode = ''TEMPLATE'''
	exec sp_executesql @sql
END
