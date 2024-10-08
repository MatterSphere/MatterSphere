

CREATE PROCEDURE [dbo].[ContactMergeDelete_dbAssociates]
AS
SET NOCOUNT ON

DECLARE @contID bigint, @newContID bigint, @logID int, @logXML xml, @logString nvarchar(MAX)
DECLARE @logTable table (ID bigint, rowguid uniqueidentifier)
DECLARE @schema nvarchar(10)

SELECT @contID = OldContID, @newContID = NewContID, @logID = LogID, @logString = CONVERT(nvarchar(MAX), ExecutionXML) FROM dbo.GetContactMergeDeleteRecord() 

SELECT @schema = SCHEMA_NAME(schema_id) FROM sys.tables WHERE name = 'dbAssociates';

BEGIN TRY
BEGIN TRANSACTION

	IF (@newContID IS NOT NULL )
	BEGIN
		IF @schema = 'config'
			UPDATE config.dbAssociates SET contID = @newContID
			OUTPUT deleted.assocID, deleted.rowguid INTO @logTable
			WHERE contID = @contID 
		ELSE
			UPDATE dbo.dbAssociates SET contID = @newContID
			OUTPUT deleted.assocID, deleted.rowguid INTO @logTable
			WHERE contID = @contID 
	END
	ELSE
	BEGIN
		IF @schema = 'config'
			DELETE config.dbAssociates
			OUTPUT deleted.assocID, deleted.rowguid INTO @logTable
			WHERE contID = @contID
		ELSE
			DELETE dbo.dbAssociates
			OUTPUT deleted.assocID, deleted.rowguid INTO @logTable
			WHERE contID = @contID
	END
	
	IF @@ROWCOUNT > 0
	BEGIN
		SET @logXML = ( SELECT ( 
				SELECT rowguid as [@rowguid], (SELECT ID FROM @logTable b WHERE a.rowguid = b.rowguid)
					FROM @logTable a FOR XML PATH('assocID'), TYPE 
			) FOR XML PATH('dbAssociates'))
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
RAISERROR('Failed to merge/delete contact record',16,1)
END CATCH
