IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.dbTasks') AND name = 'PK_dbTasks' AND type_desc = 'CLUSTERED')
BEGIN
	ALTER TABLE dbo.dbTasks DROP CONSTRAINT PK_dbTasks

	CREATE CLUSTERED INDEX [CLX_dbTasks_FileID_tskActive_usrID_feeusrID]
    ON dbo.dbTasks(fileID, tskActive, usrID, feeusrID)


	ALTER TABLE dbo.dbTasks ADD  CONSTRAINT PK_dbTasks PRIMARY KEY NONCLUSTERED (tskID)
END
GO
