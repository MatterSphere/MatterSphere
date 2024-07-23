

CREATE PROCEDURE [dbo].[MatterMergeDelete_dbAppointments]
AS
SET NOCOUNT ON

DECLARE @fileID bigint, @newFileID bigint, @logID int, @logXML xml, @logString nvarchar(MAX)
DECLARE @logTable table (ID bigint, rowguid uniqueidentifier)

SELECT @fileID = OldFileID, @logID = LogID, @newFileID = NewFileID, @logString = CONVERT(nvarchar(MAX), ExecutionXML) FROM dbo.GetMatterMergeDeleteRecord() 


BEGIN TRY
BEGIN TRANSACTION
	
	IF (@newFileID IS NULL)
	BEGIN
		DELETE dbo.dbAppointments
		OUTPUT deleted.appID, deleted.rowguid INTO @logTable
		WHERE fileID = @fileID
	END
	ELSE
	BEGIN
		UPDATE dbo.dbAppointments SET fileID = @newFileID 
		OUTPUT deleted.appID, deleted.rowguid INTO @logTable
		WHERE fileID = @fileID
	END
	
	
	IF @@ROWCOUNT > 0
	BEGIN
		SET @logXML = ( SELECT ( 
				SELECT rowguid as [@rowguid], (SELECT ID FROM @logTable b WHERE a.rowguid = b.rowguid)
					FROM @logTable a FOR XML PATH('appID'), TYPE 
			) FOR XML PATH('dbAppointments'))
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
    ON OBJECT::[dbo].[MatterMergeDelete_dbAppointments] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MatterMergeDelete_dbAppointments] TO [OMSAdminRole]
    AS [dbo];

