

CREATE PROCEDURE [dbo].[ContactMergeDelete_dbContact]
AS
SET NOCOUNT ON

DECLARE @contID bigint, @newContID bigint, @logID int, @logXML xml, @logString nvarchar(MAX)
DECLARE @logTable table ( ID bigint ) 
DECLARE @schema nvarchar(10)

SELECT @contID = OldContID, @newContID = NewContID, @logID = LogID, @logString = CONVERT(nvarchar(MAX), ExecutionXML) FROM dbo.GetContactMergeDeleteRecord() 

SELECT @schema = SCHEMA_NAME(schema_id) FROM sys.tables WHERE name = 'dbContact';

BEGIN TRY
BEGIN TRANSACTION

	IF @schema = 'config'
		DELETE config.dbContact
		OUTPUT deleted.contID INTO @logTable
		WHERE contID = @contID 
	ELSE
		DELETE dbo.dbContact
		OUTPUT deleted.contID INTO @logTable
		WHERE contID = @contID 

	IF @@ROWCOUNT > 0
	BEGIN
		SET @logXML = ( SELECT (SELECT ID as contID FROM @logTable FOR XML PATH(''), TYPE ) FOR XML PATH('dbContact'))
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


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ContactMergeDelete_dbContact] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ContactMergeDelete_dbContact] TO [OMSAdminRole]
    AS [dbo];

