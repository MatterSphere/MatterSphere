	SET NOCOUNT ON;
	DECLARE @VIEW nvarchar(MAX)
	DECLARE @BASECHEMATABLENAME nvarchar(max) 


	if (exists (select *
				from INFORMATION_SCHEMA.TABLES
				where TABLE_SCHEMA = 'config'
				and TABLE_NAME = 'dbDocument'))
		begin
			-- Advanced Security enabled
			exec fwbsaddcolumn 'config.dbdocument', 'docFolderGUID', 'uniqueidentifier'
			SET @BASECHEMATABLENAME = N'config.dbdocument'
		end
	else
		begin
			-- Non-Advanced Security
			exec fwbsaddcolumn 'dbdocument', 'docFolderGUID', 'uniqueidentifier'
			SET @BASECHEMATABLENAME = N'dbo.dbdocument'
		end

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
	
	