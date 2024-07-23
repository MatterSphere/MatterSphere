Print 'NewInstallScripts\Default_SecurityRole.sql'


-- Add default security roles
IF NOT EXISTS ( SELECT roleCode , roleLevel , roleSystem FROM dbo.dbSecurityRole WHERE roleCode = 'SYSTEM' )
BEGIN
	INSERT dbo.dbSecurityRole ( roleCode , roleLevel , roleSystem )
	VALUES ( 'SYSTEM' , 255 , 1 )
END
GO

IF NOT EXISTS ( SELECT roleCode , roleLevel , roleSystem FROM dbo.dbSecurityRole WHERE roleCode = 'USER' )
BEGIN
	INSERT dbo.dbSecurityRole ( roleCode , roleLevel , roleSystem )
	VALUES ( 'USER' , 0 , 1 )
END
GO


