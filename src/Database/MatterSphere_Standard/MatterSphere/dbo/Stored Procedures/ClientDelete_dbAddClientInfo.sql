

CREATE PROCEDURE [dbo].[ClientDelete_dbAddClientInfo]
AS
SET NOCOUNT ON

DECLARE @clID bigint, @contactID bigint, @logID int, @logXML xml, @logString nvarchar(MAX)
DECLARE @logTable table ( ID bigint ) 

SELECT @clID = ClID, @logID = LogID, @logString = CONVERT(nvarchar(MAX), ExecutionXML) FROM dbo.GetClientDeleteRecord() 

	
BEGIN TRY
BEGIN TRANSACTION

	DELETE dbo.dbAddClientInfo OUTPUT deleted.clID INTO @logTable WHERE clID = @clID
	
	IF @@ROWCOUNT > 0
	BEGIN
		SET @logXML = ( SELECT (SELECT ID as clID FROM @logTable FOR XML PATH(''), TYPE ) FOR XML PATH('dbAddClientInfo'))
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
    ON OBJECT::[dbo].[ClientDelete_dbAddClientInfo] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ClientDelete_dbAddClientInfo] TO [OMSAdminRole]
    AS [dbo];

