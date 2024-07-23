

CREATE PROCEDURE [dbo].[ClientDelete_FlagRowToProcess]


AS
SET NOCOUNT ON

IF (SELECT COUNT(*) FROM dbo.dbClientDelete WHERE ExecutionStatus= 1) > 1
BEGIN
	RAISERROR('Cannot process more than one job at a time. Please review database logs' , 16, 1)
END
ELSE
BEGIN
	IF NOT EXISTS( SELECT * FROM dbo.dbClientDelete WHERE ExecutionStatus = 1)
	BEGIN
		UPDATE TOP (1) dbo.dbClientDelete SET ExecutionStatus = 1, ExecutionStartDate = getdate() WHERE ExecutionStatus = 0
	END
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ClientDelete_FlagRowToProcess] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ClientDelete_FlagRowToProcess] TO [OMSAdminRole]
    AS [dbo];

