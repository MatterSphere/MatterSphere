DECLARE @VIEW nvarchar(MAX)
DECLARE @BASECHEMATABLENAME nvarchar(max) 

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbDocument')
	-- Advanced Security
	SET @BASECHEMATABLENAME = N'config.dbdocument'
ELSE
	-- Non-Advanced Security
	SET @BASECHEMATABLENAME = N'dbo.dbdocument'

EXEC [dbo].[fwbsAddColumn] @BASECHEMATABLENAME , 'docCreationEmailProcessed', 'bit NULL'
EXEC [dbo].[fwbsAddColumn] @BASECHEMATABLENAME , 'docCreationExtEmailProcessed', 'bit NULL'

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
GO



