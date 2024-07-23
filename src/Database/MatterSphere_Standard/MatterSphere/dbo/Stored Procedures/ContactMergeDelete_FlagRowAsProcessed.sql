

CREATE PROCEDURE [dbo].[ContactMergeDelete_FlagRowAsProcessed]
AS
SET NOCOUNT ON

DECLARE @logID int
DECLARE @logTable table (ID bigint ) 

SELECT @logID = LogID FROM dbo.GetContactMergeDeleteRecord() 


BEGIN TRANSACTION
	
	UPDATE dbo.dbContactMergeDelete SET ExecutionStatus = 5, ExecutionCompletedDate = GETDATE(), ExecutionError = NULL WHERE LogID = @logID
	
COMMIT TRANSACTION	

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ContactMergeDelete_FlagRowAsProcessed] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ContactMergeDelete_FlagRowAsProcessed] TO [OMSAdminRole]
    AS [dbo];

