

CREATE PROCEDURE [dbo].[MatterMergeDelete_dbDocumentLog]
AS
SET NOCOUNT ON

DECLARE @fileID bigint, @newFileID bigint, @logID int, @logXML xml, @logString nvarchar(MAX)
DECLARE @logTable table ( LogID bigint, docID bigint ) 
DECLARE @schema nvarchar(10)

SELECT @fileID = OldFileID, @logID = LogID, @newFileID = NewFileID, @logString = CONVERT(nvarchar(MAX), ExecutionXML) FROM dbo.GetMatterMergeDeleteRecord() 

SELECT @schema = SCHEMA_NAME(schema_id) FROM sys.tables WHERE name = 'dbDocument';

IF (@newFileID IS NULL)
BEGIN TRY
BEGIN TRANSACTION
	
	IF @schema = 'config'
		DELETE DL 
		OUTPUT deleted.logID, deleted.docID INTO @logTable
		FROM dbo.dbDocumentLog DL JOIN config.dbDocument D ON D.docID = DL.docID
		WHERE D.fileID = @fileID
	ELSE
		DELETE DL 
		OUTPUT deleted.logID, deleted.docID INTO @logTable
		FROM dbo.dbDocumentLog DL JOIN dbo.dbDocument D ON D.docID = DL.docID
		WHERE D.fileID = @fileID
	
	IF @@ROWCOUNT > 0
	BEGIN
		SET @logXML = ( SELECT (SELECT logID as '@logID', docID as '@docID' FROM @logTable FOR XML PATH('row'), TYPE ) FOR XML PATH('dbDocumentLog'))
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
    ON OBJECT::[dbo].[MatterMergeDelete_dbDocumentLog] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MatterMergeDelete_dbDocumentLog] TO [OMSAdminRole]
    AS [dbo];

