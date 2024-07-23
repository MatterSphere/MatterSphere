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


