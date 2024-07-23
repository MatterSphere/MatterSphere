

CREATE TRIGGER [tgrSchemaChangeNotification]
ON DATABASE
FOR CREATE_PROCEDURE, DROP_PROCEDURE, ALTER_PROCEDURE, CREATE_TABLE, DROP_TABLE, ALTER_TABLE, CREATE_VIEW, DROP_VIEW, ALTER_VIEW
AS
SET NOCOUNT ON;
SET ANSI_PADDING ON;

DECLARE @data XML 
DECLARE @eventtype nvarchar(100) 

SET @data = EVENTDATA() 
SET @eventtype = @data.value('(/EVENT_INSTANCE/EventType)[1]', 'nvarchar(100)')

IF @eventtype = 'ALTER_PROCEDURE' or @eventtype = 'CREATE_PROCEDURE' or @eventtype = 'DROP_PROCEDURE'
BEGIN
	EXEC sprTableMonitorUpdate 'INFORMATION_SCHEMA.ROUTINES'
	EXEC sprTableMonitorUpdate 'INFORMATION_SCHEMA.PARAMETERS' 
END

IF @eventtype = 'ALTER_TABLE' or @eventtype = 'CREATE_TABLE' or @eventtype = 'DROP_TABLE'
BEGIN
	EXEC sprTableMonitorUpdate 'INFORMATION_SCHEMA.TABLES'
	EXEC sprTableMonitorUpdate 'dbFields' 
END
IF @eventtype = 'ALTER_VIEW' or @eventtype = 'CREATE_VIEW' or @eventtype = 'DROP_VIEW'
BEGIN
	EXEC sprTableMonitorUpdate 'INFORMATION_SCHEMA.TABLES'
END

