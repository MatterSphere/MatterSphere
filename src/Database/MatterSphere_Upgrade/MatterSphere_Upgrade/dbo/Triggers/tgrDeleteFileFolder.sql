CREATE TRIGGER [dbo].[tgrDeleteFileFolder] ON [dbo].[dbFileFolder]
AFTER DELETE NOT FOR REPLICATION
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbDocument')
		UPDATE D SET docFolderGUID = NULL
		FROM config.dbDocument D INNER JOIN deleted ON D.fileID = deleted.fileID AND D.docFolderGUID = deleted.FolderGuid
	ELSE
		UPDATE D SET docFolderGUID = NULL
		FROM dbo.dbDocument D INNER JOIN deleted ON D.fileID = deleted.fileID AND D.docFolderGUID = deleted.FolderGuid
END
GO