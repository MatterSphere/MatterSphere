IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbDocument')
BEGIN
	IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbDocument_docDeleted' AND OBJECT_ID('[config].[dbDocument]') = object_id)
		CREATE NONCLUSTERED INDEX [IX_dbDocument_docDeleted]
			ON [config].[dbDocument]([docDeleted] ASC)
			INCLUDE ([docID], [clID], [docFileName]) WITH (FILLFACTOR = 90)
			ON [IndexGroup];
END
ELSE IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'dbDocument')
BEGIN
	IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbDocument_docDeleted' AND OBJECT_ID('[dbo].[dbDocument]') = object_id)
		CREATE NONCLUSTERED INDEX [IX_dbDocument_docDeleted]
			ON [dbo].[dbDocument]([docDeleted] ASC)
			INCLUDE ([docID], [clID], [docFileName]) WITH (FILLFACTOR = 90)
			ON [IndexGroup];
END
GO

IF EXISTS (SELECT * FROM sys.procedures WHERE object_id = OBJECT_ID(N'dbo.VirtualDriveFindFilesInternal'))
	DROP PROCEDURE [dbo].[VirtualDriveFindFilesInternal]
GO