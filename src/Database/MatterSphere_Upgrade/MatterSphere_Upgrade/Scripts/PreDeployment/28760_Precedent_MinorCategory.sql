SET NOCOUNT ON;

IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[dbPrecedents]') 
         AND [name] = 'PrecMinorCategory'
)
BEGIN
SELECT 1
	ALTER TABLE [dbo].[dbPrecedents] ADD [PrecMinorCategory] [dbo].[uCodeLookup] NULL

	IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[dbPrecedents]') AND name = N'IX_dbPrecedents_Combi')
	BEGIN
		ALTER TABLE [dbo].[dbPrecedents] DROP CONSTRAINT [IX_dbPrecedents_Combi]
	
		ALTER TABLE [dbo].[dbPrecedents] ADD  CONSTRAINT [IX_dbPrecedents_Combi] UNIQUE NONCLUSTERED 
		(
			[PrecTitle] ASC,
			[PrecType] ASC,
			[PrecLibrary] ASC,
			[PrecCategory] ASC,
			[PrecSubCategory] ASC,
			[PrecLanguage] ASC,
			[PrecMinorCategory] ASC
		)
	END

END
GO

/************************** Refresh VIEWs ********************************/	
DECLARE @VIEW nvarchar(MAX)	
SET @VIEW = (
		SELECT TOP 1 QUOTENAME(s.name) + N'.' + QUOTENAME(o.name)
		FROM sys.sql_expression_dependencies ed
			INNER JOIN sys.objects o ON o.object_id = ed.referencing_id
			INNER JOIN sys.schemas s ON s.schema_id = o.schema_id
		WHERE ed.referenced_id = OBJECT_ID('dbo.dbPrecedents')
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
			WHERE ed.referenced_id = OBJECT_ID('dbo.dbPrecedents')
				AND o.type_desc = 'VIEW'
				AND QUOTENAME(s.name) + N'.' + QUOTENAME(o.name) > @VIEW
			ORDER BY QUOTENAME(s.name) + N'.' + QUOTENAME(o.name)
			)
END
GO

IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[dbUser]') 
         AND [name] = 'usrPrecMinorCat'
)
ALTER TABLE [dbo].[dbUser] ADD [usrPrecMinorCat] [dbo].[uCodeLookup] NULL
GO

/************************* Refresh VIEWs *******************************/	
DECLARE @VIEW nvarchar(MAX)	
SET @VIEW = (
		SELECT TOP 1 QUOTENAME(s.name) + N'.' + QUOTENAME(o.name)
		FROM sys.sql_expression_dependencies ed
			INNER JOIN sys.objects o ON o.object_id = ed.referencing_id
			INNER JOIN sys.schemas s ON s.schema_id = o.schema_id
		WHERE ed.referenced_id = OBJECT_ID('dbo.dbUser')
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
			WHERE ed.referenced_id = OBJECT_ID('dbo.dbUser')
				AND o.type_desc = 'VIEW'
				AND QUOTENAME(s.name) + N'.' + QUOTENAME(o.name) > @VIEW
			ORDER BY QUOTENAME(s.name) + N'.' + QUOTENAME(o.name)
			)
END
GO

IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[dbPrecedentMulti]') 
         AND [name] = 'multiPrecMinorCategory'
)
ALTER TABLE [dbo].[dbPrecedentMulti] ADD [multiPrecMinorCategory] [dbo].[uCodeLookup] NULL
GO

/************************** Refresh VIEWs ********************************/	
DECLARE @VIEW nvarchar(MAX)	
SET @VIEW = (
		SELECT TOP 1 QUOTENAME(s.name) + N'.' + QUOTENAME(o.name)
		FROM sys.sql_expression_dependencies ed
			INNER JOIN sys.objects o ON o.object_id = ed.referencing_id
			INNER JOIN sys.schemas s ON s.schema_id = o.schema_id
		WHERE ed.referenced_id = OBJECT_ID('dbo.dbPrecedentMulti')
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
			WHERE ed.referenced_id = OBJECT_ID('dbo.dbPrecedentMulti')
				AND o.type_desc = 'VIEW'
				AND QUOTENAME(s.name) + N'.' + QUOTENAME(o.name) > @VIEW
			ORDER BY QUOTENAME(s.name) + N'.' + QUOTENAME(o.name)
			)
END
GO

IF EXISTS ( SELECT 'RESOURCE' , 'PRECDUP1' , '{default}', 'A Precedent with the same title, type, library, category, sub-category and language already exists in the system.

Please amend these values (making this Precedent unique) before attempting to save this Precedent.', NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdDesc, cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	UPDATE dbo.dbCodeLookup
	SET cdDesc = 'A Precedent with the same title, type, library, category, sub-category, minor category and language already exists in the system.

Please amend these values (making this Precedent unique) before attempting to save this Precedent.'
	WHERE cdType = 'RESOURCE'
		AND cdCode = 'PRECDUP1'
		AND cdUICultureInfo = '{default}'
		AND cdAddLink IS NULL
END
GO