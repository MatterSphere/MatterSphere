

CREATE PROCEDURE [dbo].[MatterMergeDelete_dbDocumentStorage]
AS
SET NOCOUNT ON

DECLARE @fileID bigint, @newFileID bigint, @logID int, @logXML xml, @logString nvarchar(MAX)
DECLARE @logTable table ( ID bigint ) 
DECLARE @schema nvarchar(10)

SELECT @fileID = OldFileID, @logID = LogID, @newFileID = NewFileID, @logString = CONVERT(nvarchar(MAX), ExecutionXML) FROM dbo.GetMatterMergeDeleteRecord() 

SELECT @schema = SCHEMA_NAME(schema_id) FROM sys.tables WHERE name = 'dbDocument';

IF (@newFileID IS NULL)
BEGIN TRY
BEGIN TRANSACTION
	
	IF @schema = 'config'
		DELETE DS 
		OUTPUT deleted.docID INTO @logTable
		FROM dbo.dbDocumentStorage DS JOIN config.dbDocument D ON D.docID = DS.docID
		WHERE D.fileID = @fileID
	ELSE
		DELETE DS 
		OUTPUT deleted.docID INTO @logTable
		FROM dbo.dbDocumentStorage DS JOIN dbo.dbDocument D ON D.docID = DS.docID
		WHERE D.fileID = @fileID

	IF @@ROWCOUNT > 0
	BEGIN
		SET @logXML = ( SELECT (SELECT ID as docID FROM @logTable FOR XML PATH(''), TYPE ) FOR XML PATH('dbDocumentStorage'))
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
    ON OBJECT::[dbo].[MatterMergeDelete_dbDocumentStorage] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MatterMergeDelete_dbDocumentStorage] TO [OMSAdminRole]
    AS [dbo];

