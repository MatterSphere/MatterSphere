

CREATE PROCEDURE [dbo].[fwbsServerConfig]
AS 

DECLARE @status nvarchar(1000)


IF DatabasePropertyEx ( db_name() , 'IsAnsiNullsEnabled' ) = 1
	SET @status =  char (13) + char (10) + 'Ansi Nulls Enabled:' + char(9)  +'ON' + char(9) + char(9)
ELSE
	SET @status =  char (13) + char (10) + 'Ansi Nulls Enabled:'+ char(9) +  'OFF'  + char(9) + char(9)

IF DatabasePropertyEx ( db_name() , 'IsTornPageDetectionEnabled' ) = 1
	SET @status = @status + 'Torn Page Detection Enabled:' + char (9) + char (9) +  'ON' 
ELSE
	SET @status = @status  + 'Torn Page Detection Enabled:' + char(9) + char (9) + 'OFF'

IF DatabasePropertyEx ( db_name() , 'IsAnsiNullDefault' ) = 1
	SET @status = @status + char (13) + char (10) + 'Ansi Null Default:' + char (9)  + 'ON' + char(9)+ char(9) 
ELSE
	SET @status = @status + char (13) + char (10) + 'Ansi Null Default:' + char(9) + 'OFF'  + char(9)+ char(9) 

IF DatabasePropertyEx ( db_name() , 'IsArithmeticAbortEnabled' ) = 1
	SET @status = @status + 'Arithmetic Abort Enabled:' + char (9) + char(9) + 'ON'
ELSE
	SET @status = @status + 'Arithmetic Abort Enabled:' + char(9) + char(9) + 'OFF'

IF DatabasePropertyEx ( db_name() , 'IsLocalCursorsDefault' ) = 1
	SET @status = @status + char (13) + char(10) + 'Local Cursors Default:' + char (9) + 'ON' + char(9) + char(9)
ELSE
	SET @status = @status + char (13) + char(10) +'Local Cursors Default:' + char(9) + 'OFF' + char(9) + char(9)

IF DatabasePropertyEx ( db_name() , 'IsCloseCursorsOnCommitEnabled' ) = 1
	SET @status = @status  + 'Close Cursors On Commit Enabled:' + char (9) + 'ON' + char(9)+ char(9) 
ELSE
	SET @status = @status  + 'Close Cursors On Commit Enabled:' + char(9) +  'OFF' + char(9)+ char(9) 

IF DatabasePropertyEx ( db_name() , 'IsAutoClose' ) = 1
	SET @status = @status + char (13) + char(10) + 'Auto Close:' + char (9) + char (9) + 'ON' + char(9)+ char(9) 
ELSE
	SET @status = @status + char (13) + char(10) + 'Auto Close:' + char(9) + char (9) +  'OFF' + char(9)+ char(9) 

IF DatabasePropertyEx ( db_name() , 'IsQuotedIdentifiersEnabled' ) = 1

	SET @status = @status + 'Quoted Identifiers Enabled:' + char (9) + char(9) + 'ON'
ELSE
	SET @status = @status + 'Quoted Identifiers Enabled:' + char(9) + char(9) + 'OFF'

IF DatabasePropertyEx ( db_name() , 'IsAutoShrink' ) = 1
	SET @status = @status + char (13) + char (10) + 'Auto Shrink:' + char (9) + char (9) + 'ON' + char(9)+ char(9) 
ELSE
	SET @status = @status + char (13) + char (10) + 'Auto Shrink:' + char(9) + char (9) +  'OFF' + char(9)+ char(9) 

IF DatabasePropertyEx ( db_name() , 'IsRecursiveTriggersEnabled' ) = 1
	SET @status = @status + 'Recursive Triggers Enabled:' + char (9) + char(9) +  'ON'
ELSE
	SET @status = @status + 'Recursive Triggers Enabled:' + char(9) +  char(9) + 'OFF'

IF DatabasePropertyEx ( db_name() , 'IsInStandBy' ) = 1
	SET @status = @status + char (13) +  char (10) + 'Stand By:' + char (9) +char (9) +  'ON' + char(9)+ char(9) 
ELSE
	SET @status = @status + char (13) + char (10) + 'Stand By:' + char(9) + char (9) +  'OFF' + char(9)+ char(9) 

IF DatabasePropertyEx ( db_name() , 'IsNumericRoundAbortEnabled' ) = 1
	SET @status = @status + 'Numeric Round Abort Enabled:' + char (9) + char(9) + 'ON'
ELSE
	SET @status = @status + 'Numeric Round Abort Enabled:' + char(9) + char(9) +  'OFF'

IF DatabasePropertyEx ( db_name() , 'IsPublished' ) = 1
	SET @status = @status + char (13) + char (10) + 'Published:' + char (9) + char (9) + 'ON' + char(9)+ char(9) 
ELSE
	SET @status = @status + char (13) + char (10) + 'Published:' + char(9) + char (9) + 'OFF' + char(9)+ char(9) 

IF DatabasePropertyEx ( db_name() , 'IsFullTextEnabled' ) = 1
	SET @status = @status + 'Full Text Enabled:' + char (9) + char (9) + char(9)  +  'ON'
ELSE
	SET @status = @status + 'Full Text Enabled:' + char(9) + char(9) + char(9) +  'OFF'

IF DatabasePropertyEx ( db_name() , 'IsMergePublished' ) = 1
	SET @status = @status + char (13) + char (10) + 'Merge Published:' + char (9) + char(9) + 'ON' + char(9) + char(9) 
ELSE
	SET @status = @status + char (13) + char (10) + 'Merge Published:' + char(9) + char (9) +  'OFF' + char(9) + char(9) 

IF DatabasePropertyEx ( db_name() , 'IsAutoUpdateStatistics' ) = 1
	SET @status = @status + 'Auto Update Statistics:' + char (9) + char(9) + char (9) +  'ON'
ELSE
	SET @status = @status  + 'Auto Update Statistics:' + char(9) +  char(9) + char(9) + 'OFF'

IF DatabasePropertyEx ( db_name() , 'IsSubscribed' ) = 1
	SET @status = @status + char (13) + char (10) + 'Subscribed:' + char (9) + char (9) + 'ON' + char(9)+ char(9) 
ELSE
	SET @status = @status + char (13) +  char (10) + 'Subscribed:' + char(9) + char (9) +  'OFF' + char(9)+ char(9) 

IF DatabasePropertyEx ( db_name() , 'IsAutoCreateStatistics' ) = 1
	SET @status = @status  + 'Auto Create Statistics:' + char (9) + char(9) + char(9) +  'ON'
ELSE
	SET @status = @status  + 'Auto Create Statistics:' + char(9) + char(9) + char(9) + 'OFF'

IF DatabasePropertyEx ( db_name() , 'IsAnsiPaddingEnabled' ) = 1
	SET @status = @status + char (13) + char (10) + 'Ans iPadding Enabled:' + char (9) + 'ON' + char(9)+ char(9) 
ELSE
	SET @status = @status + char (13) + char (10) + 'Ansi Padding Enabled:' + char(9) +  'OFF' + char(9)+ char(9) 

IF DatabasePropertyEx ( db_name() , 'IsAnsiWarningsEnabled' ) = 1
	SET @status = @status + 'Ans iWarnings Enabled:' + char (9) + char(9) + char(9) +   'ON'
ELSE
	SET @status = @status + 'Ansi Warnings Enabled:' + char(9) + char(9) + char(9) + 'OFF'

SET @status = @status + char (13) + char (10) + 'SQL Sort Order:' + char(9) + char (9) + convert(sysname,DatabasePropertyEx ( db_name() , 'SQLSortOrder' ) ) + char(9)+ char(9) 

SET @status =  @status + char (13) + char(10) + 'Status:' + char (9) + char (9) + char (9) + convert ( sysname , DatabasePropertyEx ( db_name() , 'Status' ) ) + char(9)+ char(9) 

SET @status = @status + char (13) + char(10) + 'Recovery model:' + char (9) + char(9) + convert(sysname,DatabasePropertyEx ( db_name() , 'Recovery' ) ) 

SET @status = @status  + char (13) + char(10) + 'Collation:' + char (9) + char (9) + convert ( sysname , DatabasePropertyEx ( db_name() , 'Collation' ) )

IF EXISTS ( SELECT cdCode FROM dbCodeLookup Where cdType = 'SQL' AND cdCode = 'SERVERCONFIG' ) 
BEGIN
	UPDATE dbCodeLookup
	SET cdDesc = @status Where cdType = 'SQL' AND cdCode = 'SERVERCONFIG'
END
ELSE
BEGIN
	INSERT dbCodeLookup ( cdType , cdCode , cdDesc)
	VALUES ( 'SQL' , 'SERVERCONFIG' , @status )
END


SELECT cdDesc FROM dbCodeLookup WHERE cdType = 'SQL' AND cdCode = 'SERVERCONFIG'

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsServerConfig] TO [OMSAdminRole]
    AS [dbo];

