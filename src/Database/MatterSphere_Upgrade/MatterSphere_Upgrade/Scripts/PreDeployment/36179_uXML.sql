DECLARE  @user_type_id INT
	, @SqlDrop NVARCHAR(MAX) = N''
	, @SqlCreate NVARCHAR(MAX) = N''
	, @Sql NVARCHAR(MAX) = N''
	, @Refresh NVARCHAR(MAX)

DECLARE @Proc TABLE (oId BIGINT, Code NVARCHAR(MAX))
DECLARE @oID BIGINT

SELECT @user_type_id = ut.user_type_id
FROM sys.types ut 
	INNER JOIN sys.types st  ON st.user_type_id = ut.system_type_id
WHERE ut.name = 'uxml' AND ut.schema_id = SCHEMA_ID('dbo') AND st.name = 'ntext'

IF @user_type_id IS NOT NULL
BEGIN
	---- Drop foreign keys
	SET @Sql = (
		SELECT N'ALTER TABLE ' + QUOTENAME(cs.name) + '.' + QUOTENAME(ct.name) + N' DROP CONSTRAINT ' + QUOTENAME(fk.name) + N'
'
		FROM sys.foreign_keys AS fk
			INNER JOIN sys.tables AS rt 	ON fk.referenced_object_id = rt.[object_id] -- referenced table
			INNER JOIN sys.schemas AS rs ON rt.[schema_id] = rs.[schema_id]
			INNER JOIN sys.tables AS ct ON fk.parent_object_id = ct.[object_id] -- constraint table
			INNER JOIN sys.schemas AS cs ON ct.[schema_id] = cs.[schema_id]
		WHERE rt.is_ms_shipped = 0 
			AND ct.is_ms_shipped = 0
			AND EXISTS (SELECT * 
						FROM sys.columns AS c 
							INNER JOIN sys.foreign_key_columns AS fkc ON fkc.referenced_column_id = c.column_id	AND fkc.referenced_object_id = c.[object_id]
						WHERE fkc.constraint_object_id = fk.[object_id]
							AND c.user_type_id = @user_type_id)
		FOR XML PATH(N''), TYPE).value(N'.[1]', N'nvarchar(max)')
	
	IF @Sql IS NOT NULL
	BEGIN
		SET @SqlDrop = @Sql
		---- Create previously dropped foreign keys
		SET @Sql = (
					SELECT N' ALTER TABLE ' + QUOTENAME(cs.name) + N'.' + QUOTENAME(ct.name) + N' ADD CONSTRAINT ' + QUOTENAME(fk.name) + N' FOREIGN KEY (' + STUFF(
						(SELECT ',' + QUOTENAME(c.name)
						-- get all the columns in the constraint table
						FROM sys.columns AS c 
							INNER JOIN sys.foreign_key_columns AS fkc ON fkc.parent_column_id = c.column_id AND fkc.parent_object_id = c.[object_id]
						WHERE fkc.constraint_object_id = fk.[object_id]
						ORDER BY fkc.constraint_column_id 
						FOR XML PATH(N''), TYPE).value(N'.[1]', N'nvarchar(max)')
						, 1, 1, N'')
						+ N') REFERENCES ' + QUOTENAME(rs.name) + '.' + QUOTENAME(rt.name) + '(' + STUFF(
						(SELECT ',' + QUOTENAME(c.name)
						-- get all the referenced columns
						FROM sys.columns AS c 
							INNER JOIN sys.foreign_key_columns AS fkc ON fkc.referenced_column_id = c.column_id	AND fkc.referenced_object_id = c.[object_id]
						WHERE fkc.constraint_object_id = fk.[object_id]
						ORDER BY fkc.constraint_column_id 
						FOR XML PATH(N''), TYPE).value(N'.[1]', N'nvarchar(max)')
						, 1, 1, N'') + N')' + + CASE fk.is_not_for_replication WHEN 1 THEN N' NOT FOR REPLICATION' ELSE N'' END + N';
'
					FROM sys.foreign_keys AS fk
						INNER JOIN sys.tables AS rt ON fk.referenced_object_id = rt.[object_id] -- referenced table
						INNER JOIN sys.schemas AS rs ON rt.[schema_id] = rs.[schema_id]
						INNER JOIN sys.tables AS ct ON fk.parent_object_id = ct.[object_id] -- constraint table
						INNER JOIN sys.schemas AS cs ON ct.[schema_id] = cs.[schema_id]
					WHERE rt.is_ms_shipped = 0 AND ct.is_ms_shipped = 0
						AND EXISTS (SELECT * 
									FROM sys.columns AS c 
										INNER JOIN sys.foreign_key_columns AS fkc ON fkc.referenced_column_id = c.column_id	AND fkc.referenced_object_id = c.[object_id]
									WHERE fkc.constraint_object_id = fk.[object_id]
									AND c.user_type_id = @user_type_id)
					FOR XML PATH(N''), TYPE).value(N'.[1]', N'nvarchar(max)')

		SET @SqlCreate = @Sql
	END

	SET @Sql = (
			SELECT 
				CASE  
					WHEN i.is_primary_key = 1 OR i.is_unique_constraint = 1 THEN N'ALTER TABLE '  + QUOTENAME(s.name) + N'.' + QUOTENAME(t.name) + N' DROP CONSTRAINT ' + QUOTENAME(i.name)
					ELSE N'DROP INDEX '  + QUOTENAME(i.name) + N' ON ' + QUOTENAME(s.name) + N'.' + QUOTENAME(t.name)
				END + N'
'
			FROM sys.indexes AS i  
				INNER JOIN sys.tables t ON t.object_id = i.object_id
				INNER JOIN sys.schemas AS s ON s.schema_id = t.schema_id
			WHERE EXISTS(
				SELECT 1
				FROM sys.index_columns AS ic 
				INNER JOIN sys.columns c ON i.object_id = c.object_id AND ic.column_id = c.column_id
			WHERE c.user_type_id = @user_type_id
				AND i.object_id = ic.object_id AND i.index_id = ic.index_id
			)
			FOR XML PATH(N''), TYPE).value(N'.[1]', N'nvarchar(max)')
	IF @Sql IS NOT NULL
	BEGIN		
		SET @SqlDrop = @SqlDrop + @Sql

		SET @Sql = (
				SELECT
					CASE  
						WHEN i.is_primary_key = 1 
						THEN N'ALTER TABLE '  + QUOTENAME(s.name) + N'.' + QUOTENAME(t.name) + N' ADD CONSTRAINT ' + QUOTENAME(i.name) + N' PRIMARY KEY ' + CASE i.type WHEN 1 THEN N' CLUSTERED ' WHEN 2 THEN N' NONCLUSTERED ' ELSE N' ' END
							+ N' (' + STUFF((SELECT ' , ' + QUOTENAME(c.name) + CASE WHEN ic.is_descending_key = 1 THEN ' DESC ' ELSE ' ASC ' END
									FROM sys.index_columns ic
										JOIN sys.columns c	ON  C.object_id = ic.object_id AND c.column_id = ic.column_id
									WHERE ic.is_included_column = 0
										AND ic.object_id = i.object_id
										AND ic.index_id = i.index_id
									ORDER BY ic.key_ordinal 
									FOR XML PATH('')), 1, 2, '')
							+ N')'
						WHEN i.is_unique_constraint = 1 
						THEN N'ALTER TABLE '  + QUOTENAME(s.name) + N'.' + QUOTENAME(t.name) + N' ADD CONSTRAINT' + QUOTENAME(i.name) + N' UNIQUE' + CASE i.type WHEN 1 THEN N' CLUSTERED ' WHEN 2 THEN N' NONCLUSTERED ' ELSE N' ' END 
							+ N' (' + STUFF((SELECT ' , ' + QUOTENAME(c.name) + CASE WHEN ic.is_descending_key = 1 THEN ' DESC ' ELSE ' ASC ' END
									FROM sys.index_columns ic
										JOIN sys.columns c	ON  C.object_id = ic.object_id AND c.column_id = ic.column_id
									WHERE ic.is_included_column = 0
										AND ic.object_id = i.object_id
										AND ic.index_id = i.index_id
									ORDER BY ic.key_ordinal 
									FOR XML PATH('')), 1, 2, '')
							+ N')'
						ELSE N'CREATE' + CASE i.is_unique WHEN 1 THEN N' UNIQUE ' ELSE N' ' END + CASE i.type WHEN 1 THEN N' CLUSTERED ' WHEN 2 THEN N' NONCLUSTERED ' ELSE N' ' END + N'INDEX ' + QUOTENAME(i.name) + N' ON ' + QUOTENAME(s.name) + N'.' + QUOTENAME(t.name)
							+ N' (' + STUFF((SELECT ' , ' + QUOTENAME(c.name) + CASE WHEN ic.is_descending_key = 1 THEN ' DESC ' ELSE ' ASC ' END
									FROM sys.index_columns ic
										JOIN sys.columns c	ON  C.object_id = ic.object_id AND c.column_id = ic.column_id
									WHERE ic.is_included_column = 0
										AND ic.object_id = i.object_id
										AND ic.index_id = i.index_id
									ORDER BY ic.key_ordinal 
									FOR XML PATH('')), 1, 2, '')
							+ N')'
							+ ISNULL(N' INCLUDE(' + STUFF((SELECT ' , ' + QUOTENAME(c.name)
									FROM sys.index_columns ic
										JOIN sys.columns c	ON  C.object_id = ic.object_id AND c.column_id = ic.column_id
									WHERE ic.is_included_column = 1
										AND ic.object_id = i.object_id
										AND ic.index_id = i.index_id
									ORDER BY ic.key_ordinal 
									FOR XML PATH('')), 1, 2, '')
							+ N')', N'')
					END
					+ ISNULL(N' WHERE  ' + i.filter_definition, '')
					+ N' WITH ( ' +
					+ CASE WHEN i.is_padded = 1 THEN N' PAD_INDEX = ON ' ELSE N' PAD_INDEX = OFF ' END + N',' 
					+ N'FILLFACTOR = ' + CONVERT(NVARCHAR(MAX), CASE WHEN i.fill_factor = 0 THEN 100 ELSE i.fill_factor END ) + N',' 
					+ CASE WHEN i.ignore_dup_key = 1 THEN N' IGNORE_DUP_KEY = ON ' ELSE N' IGNORE_DUP_KEY = OFF ' END + N',' 
					+ CASE WHEN st.no_recompute = 0 THEN N' STATISTICS_NORECOMPUTE = OFF ' ELSE N' STATISTICS_NORECOMPUTE = ON ' END + N',' 
					+ CASE WHEN i.allow_row_locks = 1 THEN N' ALLOW_ROW_LOCKS = ON ' ELSE N' ALLOW_ROW_LOCKS = OFF ' END + N',' 
					+ CASE WHEN i.allow_page_locks = 1 THEN N' ALLOW_PAGE_LOCKS = ON ' ELSE N' ALLOW_PAGE_LOCKS = OFF ' END + N' ) ON ' 
					+ QUOTENAME(DS.name) + N'
'
				FROM sys.indexes AS i  
					INNER JOIN sys.tables t ON t.object_id = i.object_id
					INNER JOIN sys.schemas AS s ON s.schema_id = t.schema_id
					INNER JOIN sys.stats st ON st.object_id = i.object_id AND st.stats_id = i.index_id
					INNER JOIN sys.data_spaces ds ON i.data_space_id = ds.data_space_id
				WHERE EXISTS(
					SELECT 1
					FROM sys.index_columns AS ic 
					INNER JOIN sys.columns c ON i.object_id = c.object_id AND ic.column_id = c.column_id
				WHERE c.user_type_id = @user_type_id
					AND i.object_id = ic.object_id AND i.index_id = ic.index_id
				)
				FOR XML PATH(N''), TYPE).value(N'.[1]', N'nvarchar(max)')

		SET @SqlCreate = @Sql + @SqlCreate
	END

	SET @Sql = (SELECT N'ALTER TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + N' DROP CONSTRAINT ' + QUOTENAME(dc.name) + N'
'
	FROM sys.all_columns c
		INNER JOIN sys.tables t ON c.object_id = t.object_id
		INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
		INNER JOIN sys.default_constraints dc ON c.default_object_id = dc.object_id
	WHERE c.user_type_id = @user_type_id
	FOR XML PATH(N''), TYPE).value(N'.[1]', N'nvarchar(max)')

	IF @Sql IS NOT NULL
	BEGIN		
		SET @SqlDrop = @SqlDrop + @Sql

		SET @Sql = N''

		SELECT N'ALTER TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + N' ADD CONSTRAINT ' + QUOTENAME(dc.name) + N' DEFAULT ' + dc.definition + N' FOR ' + QUOTENAME(c.name) +  N' 
' AS c
		INTO #t 
		FROM sys.all_columns c
			INNER JOIN sys.tables t ON c.object_id = t.object_id
			INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
			INNER JOIN sys.default_constraints dc ON c.default_object_id = dc.object_id 
		WHERE c.user_type_id = @user_type_id

		UPDATE #t
		SET @sql = @sql + c
		DROP TABLE #t

	END

	SET @Sql = (SELECT N'ALTER TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + N' ALTER COLUMN ' + QUOTENAME(c.name) + N' NVARCHAR(MAX)
'
	FROM sys.all_columns c
		INNER JOIN sys.tables t ON c.object_id = t.object_id
		INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
	WHERE c.user_type_id = @user_type_id
	FOR XML PATH(N''), TYPE).value(N'.[1]', N'nvarchar(max)')

	SET @Refresh = (SELECT DISTINCT N'
EXEC sp_refreshview ''' + QUOTENAME(s.name) + N'.' + QUOTENAME(o.name) + ''''
		FROM sys.sql_expression_dependencies ed
			INNER JOIN sys.objects o ON o.object_id = ed.referencing_id
			INNER JOIN sys.schemas s ON s.schema_id = o.schema_id
			INNER JOIN (SELECT QUOTENAME(s.name) + '.' + QUOTENAME(t.name)  AS BASECHEMATABLENAME
	FROM sys.all_columns c
		INNER JOIN sys.tables t ON c.object_id = t.object_id
		INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
	WHERE c.user_type_id = @user_type_id) BT ON OBJECT_ID(BT.BASECHEMATABLENAME) = ed.referenced_id 
		WHERE o.type_desc = 'VIEW'
		FOR XML PATH(N''), TYPE).value(N'.[1]', N'nvarchar(max)')


	IF @Sql IS NOT NULL
	BEGIN
		SET @SqlDrop = @SqlDrop + @Sql + ISNULL(@Refresh, N'')

		SET @Sql = (SELECT N'ALTER TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + N' ALTER COLUMN ' + QUOTENAME(c.name) + N' uXML
'
		FROM sys.all_columns c
			INNER JOIN sys.tables t ON c.object_id = t.object_id
			INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
		WHERE c.user_type_id = @user_type_id
		FOR XML PATH(N''), TYPE).value(N'.[1]', N'nvarchar(max)')

		SET @SqlCreate = @Sql + @SqlCreate + ISNULL(@Refresh, N'')

	END

	INSERT INTO @Proc(oId)
	SELECT DISTINCT object_id
	FROM sys.all_parameters p
	WHERE p.user_type_id = @user_type_id

	IF EXISTS(SELECT 1 FROM @Proc)
	BEGIN
		UPDATE @Proc
		SET Code = Object_definition(oId)

		UPDATE @Proc
		SET Code = STUFF(Code, CHARINDEX('CREATE', Code), 6, 'ALTER')

		SET @oID = (SELECT TOP 1 oID FROM @Proc ORDER BY oID)
		WHILE @oID IS NOT NULL
		BEGIN
			SET @Sql = (SELECT REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(Code, '[dbo].[uxml]', 'NVARCHAR(MAX)'), '[dbo].uxml', 'NVARCHAR(MAX)'), 'dbo.[uxml]', 'NVARCHAR(MAX)'), 'dbo.uxml', 'NVARCHAR(MAX)') , 'uxml', 'NVARCHAR(MAX)') FROM @Proc WHERE oId = @oID)
			EXEC sp_executesql @Sql
			SET @oID = (SELECT TOP 1 oID FROM @Proc WHERE oID > @oID ORDER BY oID)
		END
	END

	IF @SqlDrop <> '' 
	BEGIN
		EXEC sp_executesql @SqlDrop
		EXEC sp_executesql N'DROP TYPE [dbo].[uXML]'
		EXEC sp_executesql N'CREATE TYPE [dbo].[uXML] FROM [nvarchar](MAX) NULL'
		EXEC sp_executesql @SqlCreate
	END

	IF EXISTS(SELECT 1 FROM @Proc)
	BEGIN
		SET @oID = (SELECT TOP 1 oID FROM @Proc ORDER BY oID)
		WHILE @oID IS NOT NULL
		BEGIN
			SET @Sql = (SELECT Code FROM @Proc WHERE oId = @oID)
			EXEC sp_executesql @Sql
			SET @oID = (SELECT TOP 1 oID FROM @Proc WHERE oID > @oID ORDER BY oID)
		END
	END


END

GO
