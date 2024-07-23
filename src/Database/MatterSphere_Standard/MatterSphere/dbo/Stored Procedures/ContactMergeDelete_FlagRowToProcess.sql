

CREATE PROCEDURE [dbo].[ContactMergeDelete_FlagRowToProcess]
AS
SET NOCOUNT ON

IF (SELECT COUNT(*) FROM dbo.dbContactMergeDelete WHERE ExecutionStatus= 1) > 1
BEGIN
	RAISERROR('Cannot process more than one job at a time. Please review database logs' , 16, 1)
END
ELSE
BEGIN
	IF NOT EXISTS( SELECT * FROM dbo.dbContactMergeDelete WHERE ExecutionStatus = 1)
	BEGIN
		UPDATE TOP (1) dbo.dbContactMergeDelete SET ExecutionStatus = 1, ExecutionStartDate = getdate() WHERE ExecutionStatus = 0
	END
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ContactMergeDelete_FlagRowToProcess] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ContactMergeDelete_FlagRowToProcess] TO [OMSAdminRole]
    AS [dbo];

