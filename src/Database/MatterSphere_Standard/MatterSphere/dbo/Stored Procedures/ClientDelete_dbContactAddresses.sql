

CREATE PROCEDURE [dbo].[ClientDelete_dbContactAddresses]


AS
SET NOCOUNT ON

DECLARE @clID bigint, @contactID bigint, @logID int, @logXML xml, @logString nvarchar(MAX)
DECLARE @logTable table ( ID bigint , AddID bigint) 

SELECT @clID = ClID, @logID = LogID, @contactID = DefaultContactID, @logString = CONVERT(nvarchar(MAX), ExecutionXML) FROM dbo.GetClientDeleteRecord() 

IF (@contactID IS NULL)
	RETURN
	
BEGIN TRY
BEGIN TRANSACTION

	DELETE dbo.dbContactAddresses OUTPUT deleted.contID, deleted.contaddID INTO @logTable WHERE contID = @contactID
	
	IF @@ROWCOUNT > 0
	BEGIN
		SET @logXML = ( SELECT (SELECT ID as '@contID', AddID as '@addID' FROM @logTable FOR XML PATH('row'), TYPE ) FOR XML PATH('dbContactAddresses'))
		IF (@logString = '<log/>')
			UPDATE dbo.dbClientDelete SET ExecutionXML = '<log>' + CONVERT(nvarchar(MAX), @logXML) + '</log>' WHERE logID = @logID
		ELSE
			UPDATE dbo.dbClientDelete SET ExecutionXML = REPLACE (Convert(nvarchar(MAX) , ExecutionXML) , '</log>' , Convert(nvarchar(MAX) , @logXML) + '</log>') WHERE logID = @logID
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
UPDATE dbo.dbClientDelete SET ExecutionStatus = 2, ExecutionError = @err WHERE LogID = @logID
RAISERROR('Failed to delete client record',16,1)
END CATCH

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ClientDelete_dbContactAddresses] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ClientDelete_dbContactAddresses] TO [OMSAdminRole]
    AS [dbo];

