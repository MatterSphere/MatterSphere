CREATE TABLE dbo.dbFileFolder
(
	fileID BIGINT NOT NULL
	, FolderGuid UNIQUEIDENTIFIER NOT NULL
	, FolderCode NVARCHAR(15) NOT NULL
	, rowguid UNIQUEIDENTIFIER CONSTRAINT DF_dbFileFolder_rowguid DEFAULT (NEWID()) ROWGUIDCOL NOT NULL
	, CONSTRAINT PK_dbFileFolder PRIMARY KEY CLUSTERED (fileID, FolderGuid)
)
GO

CREATE NONCLUSTERED INDEX IX_dbFileFolder_FolderGuid_FolderCode ON dbo.dbFileFolder (FolderGuid, FolderCode)
GO

CREATE NONCLUSTERED INDEX IX_dbFileFolder_FolderCode_FolderGuid ON dbo.dbFileFolder (FolderCode, FolderGuid)
GO

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

GRANT UPDATE
    ON OBJECT::[dbo].[dbFileFolder] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFileFolder] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFileFolder] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFileFolder] TO [OMSApplicationRole]
    AS [dbo];


