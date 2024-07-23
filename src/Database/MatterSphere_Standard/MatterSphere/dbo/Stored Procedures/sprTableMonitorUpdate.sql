

CREATE PROCEDURE [dbo].[sprTableMonitorUpdate] @TableName NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS(SELECT TableName from dbTableMonitor WHERE TableName = @TableName)
	BEGIN
		UPDATE dbTableMonitor SET TableName = @TableName, LastUpdated = GETUTCDATE() WHERE TableName = @TableName
	END
	ELSE
	BEGIN
		INSERT dbTableMonitor (TableName, Category, LastUpdated) VALUES (@TableName, '', GETUTCDATE())
	END
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprTableMonitorUpdate] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprTableMonitorUpdate] TO [OMSAdminRole]
    AS [dbo];

