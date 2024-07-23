Print 'STarting NewInstallScripts\Default_CommandBar.sql'


-- Add Default Command Bar records
IF NOT EXISTS ( SELECT cbCode FROM dbo.dbCommandBar WHERE cbCode = 'DOCUMENT' )
BEGIN
	INSERT dbo.dbCommandBar (cbCode, cbVersion, cbPosition, cbScript)
	VALUES ('DOCUMENT', 61, 'msoBarTop', NULL)
END
GO

IF NOT EXISTS ( SELECT cbCode FROM dbo.dbCommandBar WHERE cbCode = 'MAIN' )
BEGIN
	INSERT dbo.dbCommandBar (cbCode, cbVersion, cbPosition, cbScript)
	VALUES ('MAIN', 1310, 'msoBarTop', 'MAINCB')
END
GO

IF NOT EXISTS ( SELECT cbCode FROM dbo.dbCommandBar WHERE cbCode = 'OUTLOOKITEM' )
BEGIN
	INSERT dbo.dbCommandBar (cbCode, cbVersion, cbPosition, cbScript)
	VALUES ('OUTLOOKITEM', 251, 'msoBarTop', NULL)
END
GO

IF NOT EXISTS ( SELECT cbCode FROM dbo.dbCommandBar WHERE cbCode = 'PRECEDENT' )
BEGIN
	INSERT dbo.dbCommandBar (cbCode, cbVersion, cbPosition, cbScript)
	VALUES ('PRECEDENT', 86, 'msoBarTop', NULL)
END
GO

IF NOT EXISTS ( SELECT cbCode FROM dbo.dbCommandBar WHERE cbCode = 'REPORTS' )
BEGIN
	INSERT dbo.dbCommandBar (cbCode, cbVersion, cbPosition, cbScript)
	VALUES ('REPORTS', 6, 'None', NULL)
END
GO

IF NOT EXISTS ( SELECT cbCode FROM dbo.dbCommandBar WHERE cbCode = 'TIMEREC' )
BEGIN
	INSERT dbo.dbCommandBar (cbCode, cbVersion, cbPosition, cbScript)
	VALUES ('TIMEREC', 40, 'msoBarRight', NULL)
END
GO
