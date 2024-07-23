IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbClient')
	IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbClient_cltypeCode' AND OBJECT_ID('[config].[dbClient]') = object_id)
		CREATE NONCLUSTERED INDEX [IX_dbClient_cltypeCode] ON config.dbClient
		(
			cltypeCode ASC
		) WITH (FILLFACTOR = 90)
			ON [IndexGroup];

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'dbClient')
	IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbClient_cltypeCode' AND OBJECT_ID('[dbo].[dbClient]') = object_id)
		CREATE NONCLUSTERED INDEX [IX_dbClient_cltypeCode] ON dbo.dbClient
		(
			cltypeCode ASC
		) WITH (FILLFACTOR = 90)
			ON [IndexGroup];

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbfile')
	IF NOT EXISTS(SELECT c.* 
			FROM sys.indexes i
				INNER JOIN sys.index_columns ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id
				INNER JOIN sys.columns c ON c.object_id = i.object_id AND c.column_id = ic.column_id
			WHERE i.object_id = OBJECT_ID('config.dbFile') 
				AND i.name = 'IX_dbFile_clID_fileNo' 
				AND c.name = 'fileStatus')
	BEGIN
		DROP INDEX [IX_dbFile_clID_fileNo]  ON [config].[dbFile]
		CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFile_clID_fileNo]  ON [config].[dbFile]([clID] ASC, [fileNo] ASC) INCLUDE (filestatus)
	END

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'dbfile')
	IF NOT EXISTS(SELECT c.* 
			FROM sys.indexes i
				INNER JOIN sys.index_columns ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id
				INNER JOIN sys.columns c ON c.object_id = i.object_id AND c.column_id = ic.column_id
			WHERE i.object_id = OBJECT_ID('dbo.dbFile') 
				AND i.name = 'IX_dbFile_clID_fileNo' 
				AND c.name = 'fileStatus')
	BEGIN
		DROP INDEX [IX_dbFile_clID_fileNo]  ON [dbo].[dbFile]
		CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFile_clID_fileNo]  ON [dbo].[dbFile]([clID] ASC, [fileNo] ASC) INCLUDE (filestatus)
	END

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbFile')
	IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbFile_clID_Created' AND OBJECT_ID('[config].[dbFile]') = object_id)
		CREATE NONCLUSTERED INDEX [IX_dbFile_clID_Created]
			ON [config].[dbFile]([clID], [Created] DESC)
			INCLUDE ([fileStatus], [fileDesc], [fileType], [phID], [fileAlertLevel], [fileAlertMessage]);

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'dbFile')
	IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbFile_clID_Created' AND OBJECT_ID('[dbo].[dbFile]') = object_id)
		CREATE NONCLUSTERED INDEX [IX_dbFile_clID_Created]
			ON [dbo].[dbFile]([clID], [Created] DESC)
			INCLUDE ([fileStatus], [fileDesc], [fileType], [phID], [fileAlertLevel], [fileAlertMessage]);
