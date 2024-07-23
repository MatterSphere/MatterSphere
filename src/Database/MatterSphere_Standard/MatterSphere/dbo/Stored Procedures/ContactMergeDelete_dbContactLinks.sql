

CREATE PROCEDURE [dbo].[ContactMergeDelete_dbContactLinks]
AS
SET NOCOUNT ON

DECLARE @contID bigint, @newContID bigint, @logID int, @logXML xml, @logString nvarchar(MAX)
DECLARE @logTable table ( ID bigint, Link bigint, Code uCodeLookup ) 

SELECT @contID = OldContID, @logID = LogID, @logString = CONVERT(nvarchar(MAX), ExecutionXML) FROM dbo.GetContactMergeDeleteRecord() 

BEGIN TRY
BEGIN TRANSACTION

	DELETE dbo.dbContactLinks
	OUTPUT deleted.contID, deleted.contLinkID, deleted.contLinkCode INTO @logTable
	WHERE contID = @contID or contLinkID = @contID
	
	IF @@ROWCOUNT > 0
	BEGIN
		SET @logXML = ( SELECT (SELECT ID as '@contID', Link as '@conLink', Code as '@contLinkCode' FROM @logTable FOR XML PATH('row'), TYPE ) FOR XML PATH('dbContactLinks'))
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
    ON OBJECT::[dbo].[ContactMergeDelete_dbContactLinks] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ContactMergeDelete_dbContactLinks] TO [OMSAdminRole]
    AS [dbo];

