

CREATE PROCEDURE [dbo].[ContactMergeDelete_dbFile]
AS
SET NOCOUNT ON

DECLARE @contID bigint, @newContID bigint, @logID int, @logXML xml, @logString nvarchar(MAX)
DECLARE @logTable table ( fileID bigint, contID bigint ) 
DECLARE @schema nvarchar(10)

SELECT @contID = OldContID, @logID = LogID, @logString = CONVERT(nvarchar(MAX), ExecutionXML)  FROM dbo.GetContactMergeDeleteRecord() 

SELECT @schema = SCHEMA_NAME(schema_id) FROM sys.tables WHERE name = 'dbFile';

BEGIN TRY
BEGIN TRANSACTION
	
	IF @schema = 'config'
		UPDATE config.dbFile SET fileSourceContact = @newContID
		OUTPUT deleted.fileID, deleted.fileSourceContact INTO @logTable
		WHERE fileSourceContact = @contID
	ELSE
		UPDATE dbo.dbFile SET fileSourceContact = @newContID
		OUTPUT deleted.fileID, deleted.fileSourceContact INTO @logTable
		WHERE fileSourceContact = @contID

	IF @@ROWCOUNT > 0
	BEGIN
		SET @logXML = ( SELECT (SELECT fileID as '@fileID', contID as '@contID' FROM @logTable FOR XML PATH('row'), TYPE ) FOR XML PATH('dbFile'))
		IF (@logString = '<log/>')
			UPDATE dbo.dbContactMergeDelete SET ExecutionXML = '<log>' + CONVERT(nvarchar(MAX), @logXML) + '</log>' WHERE logID = @logID
		ELSE
			UPDATE dbo.dbContactMergeDelete SET ExecutionXML = REPLACE (Convert(nvarchar(MAX) , ExecutionXML) , '</log>' , Convert(nvarchar(MAX) , @logXML) + '</log>') WHERE logID = @logID
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
UPDATE dbo.dbContactMergeDelete SET ExecutionStatus = 2, ExecutionError = @err WHERE LogID = @logID
RAISERROR('Failed to merge - delete record', 16,1)
END CATCH

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ContactMergeDelete_dbFile] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ContactMergeDelete_dbFile] TO [OMSAdminRole]
    AS [dbo];

