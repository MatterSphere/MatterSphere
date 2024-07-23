Print 'Starting dbCaptainsLogType_LOGIN_LOGOFF.sql'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT [typeCode] FROM dbo.[dbCaptainsLogType] WHERE typeID = 1 AND typeCode = 'LOGIN' )
BEGIN
	INSERT INTO [dbo].[dbCaptainsLogType] ([typeID], [typeCode], [typeHelpURL], [typeSeverity], [typeGroup], [typeSystem]) 
	VALUES (1,N'LOGIN', NULL, 1, N'SESSION', 1)
END

IF NOT EXISTS ( SELECT [typeCode] FROM dbo.[dbCaptainsLogType] WHERE typeID = 2 AND typeCode = 'LOGOFF' )
BEGIN
	INSERT INTO [dbo].[dbCaptainsLogType] ([typeID], [typeCode], [typeHelpURL], [typeSeverity], [typeGroup], [typeSystem]) 
	VALUES (2,N'LOGOFF', NULL, 1, N'SESSION', 1)
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO