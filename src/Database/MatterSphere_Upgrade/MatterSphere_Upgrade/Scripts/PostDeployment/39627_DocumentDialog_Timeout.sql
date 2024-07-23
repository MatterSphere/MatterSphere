SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.GetDocAll') AND type in (N'IF', N'TF' , N'FN'))
	DROP FUNCTION [dbo].[GetDocAll]
GO


IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbDocument')
BEGIN
	IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbDocument_SearchDocAll_1' AND OBJECT_ID('[config].[dbDocument]') = object_id)
		CREATE NONCLUSTERED INDEX [IX_dbDocument_SearchDocAll_1]
			ON [config].[dbDocument] ([Createdby], [UpdatedBy], [docDeleted], [docID], [Created])
			INCLUDE ([fileID], [docType], [docWallet], [Updated], [Opened], [docFolderGUID])
			ON [IndexGroup];

	IF EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbDocument_SearchDocAll_2' AND OBJECT_ID('[config].[dbDocument]') = object_id)
		DROP INDEX [IX_dbDocument_SearchDocAll_2] ON [config].[dbDocument]

		CREATE NONCLUSTERED INDEX [IX_dbDocument_SearchDocAll_2]
			ON [config].[dbDocument] ([UpdatedBy], [docDeleted], [docID])
			INCLUDE ([fileID], [assocID], [docType], [docWallet], [docAuthored], [Createdby], [Updated])
			ON [IndexGroup];

	IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbDocument_SearchDocAll_3' AND OBJECT_ID('[config].[dbDocument]') = object_id)
		CREATE NONCLUSTERED INDEX [IX_dbDocument_SearchDocAll_3]
			ON [config].[dbDocument] ([UpdatedBy], [docDeleted], [Created])
			INCLUDE ([docID], [fileID], [assocID], [Createdby], [Updated], [docFolderGUID])
			ON [IndexGroup];
END
ELSE IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'dbDocument')
BEGIN
	IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbDocument_SearchDocAll_1' AND OBJECT_ID('[dbo].[dbDocument]') = object_id)
		CREATE NONCLUSTERED INDEX [IX_dbDocument_SearchDocAll_1]
			ON [dbo].[dbDocument] ([Createdby], [UpdatedBy], [docDeleted], [docID], [Created])
			INCLUDE ([fileID], [docType], [docWallet], [Updated], [Opened], [docFolderGUID])
			ON [IndexGroup];

	IF EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbDocument_SearchDocAll_2' AND OBJECT_ID('[dbo].[dbDocument]') = object_id)
		DROP INDEX [IX_dbDocument_SearchDocAll_2] ON [dbo].[dbDocument]
		
		CREATE NONCLUSTERED INDEX [IX_dbDocument_SearchDocAll_2]
			ON [dbo].[dbDocument] ([UpdatedBy], [docDeleted], [docID])
			INCLUDE ([fileID], [assocID], [docType], [docWallet], [docAuthored], [Createdby], [Updated])
			ON [IndexGroup];

	IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbDocument_SearchDocAll_3' AND OBJECT_ID('[dbo].[dbDocument]') = object_id)
		CREATE NONCLUSTERED INDEX [IX_dbDocument_SearchDocAll_3]
			ON [dbo].[dbDocument] ([UpdatedBy], [docDeleted], [Created])
			INCLUDE ([docID], [fileID], [assocID], [Createdby], [Updated], [docFolderGUID])
			ON [IndexGroup];
END

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbFile')
BEGIN
	IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbFile_filePrincipleID_fileDepartment' AND OBJECT_ID('[config].[dbFile]') = object_id)
		CREATE NONCLUSTERED INDEX [IX_dbFile_filePrincipleID_fileDepartment]
			ON [config].[dbFile] ([filePrincipleID], [fileDepartment])
			INCLUDE ([fileID], [clID], [fileNo], [fileDesc], [fileType])
			ON [IndexGroup];
END
ELSE IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'dbFile')
BEGIN
	IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbFile_filePrincipleID_fileDepartment' AND OBJECT_ID('[dbo].[dbFile]') = object_id)
		CREATE NONCLUSTERED INDEX [IX_dbFile_filePrincipleID_fileDepartment]
			ON [dbo].[dbFile] ([filePrincipleID], [fileDepartment])
			INCLUDE ([fileID], [clID], [fileNo], [fileDesc], [fileType])
			ON [IndexGroup];
END
