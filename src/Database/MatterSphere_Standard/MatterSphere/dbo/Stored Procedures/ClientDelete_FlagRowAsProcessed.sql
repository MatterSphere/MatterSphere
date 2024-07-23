

CREATE PROCEDURE [dbo].[ClientDelete_FlagRowAsProcessed]

AS
SET NOCOUNT ON

DECLARE @clID bigint, @logID int
DECLARE @logTable table (ID bigint ) 

SELECT @clID = ClID, @logID = LogID FROM dbo.GetClientDeleteRecord() 


BEGIN TRANSACTION
	
	UPDATE dbo.dbClientDelete SET ExecutionStatus = 5, ExecutionCompletedDate = GETDATE(), ExecutionError = NULL WHERE LogID = @logID
	
COMMIT TRANSACTION	

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ClientDelete_FlagRowAsProcessed] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ClientDelete_FlagRowAsProcessed] TO [OMSAdminRole]
    AS [dbo];

