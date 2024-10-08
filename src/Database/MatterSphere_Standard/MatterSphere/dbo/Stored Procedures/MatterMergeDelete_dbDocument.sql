

CREATE PROCEDURE [dbo].[MatterMergeDelete_dbDocument]
AS
SET NOCOUNT ON

DECLARE @fileID bigint, @newFileID bigint, @logID int, @logXML xml, @logString nvarchar(MAX)
DECLARE @logTable table (ID bigint, rowguid uniqueidentifier)
DECLARE @schema nvarchar(10)

SELECT @fileID = OldFileID, @logID = LogID, @newFileID = NewFileID, @logString = CONVERT(nvarchar(MAX), ExecutionXML) FROM dbo.GetMatterMergeDeleteRecord() 

SELECT @schema = SCHEMA_NAME(schema_id) FROM sys.tables WHERE name = 'dbDocument';

BEGIN TRY
BEGIN TRANSACTION
	
	IF (@newFileID IS NULL)
	BEGIN
		IF @schema = 'config'
			DELETE config.dbDocument
			OUTPUT deleted.docID, deleted.rowguid INTO @logTable
			WHERE fileID = @fileID
		ELSE
			DELETE dbo.dbDocument
			OUTPUT deleted.docID, deleted.rowguid INTO @logTable
			WHERE fileID = @fileID
	END
	ELSE
	BEGIN
		IF @schema = 'config'
			UPDATE config.dbDocument SET fileID = @newFileID 
			OUTPUT deleted.docID, deleted.rowguid INTO @logTable
			WHERE fileID = @fileID
		ELSE
			UPDATE dbo.dbDocument SET fileID = @newFileID 
			OUTPUT deleted.docID, deleted.rowguid INTO @logTable
			WHERE fileID = @fileID
	END
	
	
	IF @@ROWCOUNT > 0
	BEGIN
		SET @logXML = ( SELECT ( 
				SELECT rowguid as [@rowguid], (SELECT ID FROM @logTable b WHERE a.rowguid = b.rowguid)
					FROM @logTable a FOR XML PATH('docID'), TYPE 
			) FOR XML PATH('dbDocument'))
		IF (@logString = '<log/>')
			UPDATE dbo.dbMatterMergeDelete SET ExecutionXML = '<log>' + CONVERT(nvarchar(MAX), @logXML) + '</log>' WHERE logID = @logID
		ELSE
			UPDATE dbo.dbMatterMergeDelete SET ExecutionXML = REPLACE (Convert(nvarchar(MAX) , ExecutionXML) , '</log>' , Convert(nvarchar(MAX) , @logXML) + '</log>') WHERE logID = @logID
	END
	
COMMIT TRANSACTION	
END TRY

BEGIN CATCH
IF @@Trancount <> 0
BEGIN
	ROLLBACK TRANSACTION
END
DECLARE @err nvarchar(MAX) 
SET @err = ERROR_MESSAGE()
UPDATE dbo.dbMatterMergeDelete SET ExecutionStatus = 2, ExecutionError = @err WHERE LogID = @logID
RAISERROR('Failed to merge - delete record', 16,1)
END CATCH

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MatterMergeDelete_dbDocument] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MatterMergeDelete_dbDocument] TO [OMSAdminRole]
    AS [dbo];

