

CREATE PROCEDURE [dbo].[MatterMergeDelete_FlagRowAsProcessed]
AS
SET NOCOUNT ON

DECLARE @fileID bigint, @newFileID bigint, @logID int, @logXML xml
DECLARE @logTable table (ID bigint ) 

SELECT @fileID = OldFileID, @logID = LogID FROM dbo.GetMatterMergeDeleteRecord() 



BEGIN TRANSACTION
	
	UPDATE dbo.dbMatterMergeDelete SET ExecutionStatus = 5, ExecutionCompletedDate = GETDATE(), ExecutionError = NULL WHERE LogID = @logID
	
COMMIT TRANSACTION	

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MatterMergeDelete_FlagRowAsProcessed] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MatterMergeDelete_FlagRowAsProcessed] TO [OMSAdminRole]
    AS [dbo];

