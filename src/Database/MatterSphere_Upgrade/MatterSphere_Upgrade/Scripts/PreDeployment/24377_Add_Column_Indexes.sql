SET NOCOUNT ON;
DECLARE @VIEW nvarchar(MAX)
DECLARE @BASECHEMATABLENAME nvarchar(max) 


IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbDocument')
	BEGIN
		-- Advanced Security enabled
		EXEC fwbsaddcolumn 'config.dbdocument', 'Opened', 'datetime'
		SET @BASECHEMATABLENAME = N'config.dbdocument'

		IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbDocument_Updated' AND OBJECT_ID('config.dbDocument') = object_id)
			CREATE NONCLUSTERED INDEX [IX_dbDocument_Updated] ON config.[dbDocument] ([Updated] ASC);

		IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbDocument_Opened' AND OBJECT_ID('config.dbDocument') = object_id)
			CREATE NONCLUSTERED INDEX [IX_dbDocument_Opened] ON config.[dbDocument] ([Opened] ASC);

	END
ELSE
	BEGIN
		-- Non-Advanced Security
		EXEC fwbsaddcolumn 'dbdocument', 'Opened', 'datetime'
		SET @BASECHEMATABLENAME = N'dbo.dbdocument'

		IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbDocument_Updated' AND OBJECT_ID('dbo.dbDocument') = object_id)
			CREATE NONCLUSTERED INDEX [IX_dbDocument_Updated] ON [dbo].[dbDocument] ([Updated] ASC);

		IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbDocument_Opened' AND OBJECT_ID('dbo.dbDocument') = object_id)
			CREATE NONCLUSTERED INDEX [IX_dbDocument_Opened] ON [dbo].[dbDocument] ([Opened] ASC);
	END

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

