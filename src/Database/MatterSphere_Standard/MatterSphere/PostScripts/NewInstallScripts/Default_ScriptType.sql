Print 'NewInstallScripts\Default_ScriptType.sql'


-- Add default Script types
IF NOT EXISTS ( SELECT scrType FROM dbo.dbScriptType WHERE scrType = 'ENQUIRY' )
BEGIN
	INSERT dbo.dbScriptType ( scrType, scrAssemblyType )
	VALUES ( 'ENQUIRY', 'FWBS.OMS.Script.EnquiryScriptType,OMS.Library' )
END
GO

IF NOT EXISTS ( SELECT scrType FROM dbo.dbScriptType WHERE scrType = 'ENQUIRYFORM' )
BEGIN
	INSERT dbo.dbScriptType ( scrType, scrAssemblyType )
	VALUES ( 'ENQUIRYFORM', 'FWBS.OMS.UI.Windows.Script.EnquiryFormScriptType,OMS.UI' )
END
GO

IF NOT EXISTS ( SELECT scrType FROM dbo.dbScriptType WHERE scrType = 'FILEMANAGEMENT' )
BEGIN
	INSERT dbo.dbScriptType ( scrType, scrAssemblyType )
	VALUES ( 'FILEMANAGEMENT', 'FWBS.OMS.FileManagement.ApplicationScriptType,FWBS.OMS.FileManagement' )
END
GO

IF NOT EXISTS ( SELECT scrType FROM dbo.dbScriptType WHERE scrType = 'MENU' )
BEGIN
	INSERT dbo.dbScriptType ( scrType, scrAssemblyType )
	VALUES ( 'MENU', 'FWBS.OMS.Script.MenuScriptType,OMS.Library' )
END
GO

IF NOT EXISTS ( SELECT scrType FROM dbo.dbScriptType WHERE scrType = 'PRECEDENT' )
BEGIN
	INSERT dbo.dbScriptType ( scrType, scrAssemblyType )
	VALUES ( 'PRECEDENT', 'FWBS.OMS.Script.PrecedentScriptType,OMS.Library' )
END
GO

IF NOT EXISTS ( SELECT scrType FROM dbo.dbScriptType WHERE scrType = 'REPORT' )
BEGIN
	INSERT dbo.dbScriptType ( scrType, scrAssemblyType )
	VALUES ( 'REPORT', 'FWBS.OMS.Script.ReportScriptType,OMS.Library' )
END
GO

IF NOT EXISTS ( SELECT scrType FROM dbo.dbScriptType WHERE scrType = 'SEARCHENG' )
BEGIN
	INSERT dbo.dbScriptType ( scrType, scrAssemblyType )
	VALUES ( 'SEARCHENG', 'FWBS.OMS.Script.SearchListScriptType,OMS.Library' )
END
GO

IF NOT EXISTS ( SELECT scrType FROM dbo.dbScriptType WHERE scrType = 'SEARCHLIST' )
BEGIN
	INSERT dbo.dbScriptType ( scrType, scrAssemblyType )
	VALUES ( 'SEARCHLIST', 'FWBS.OMS.UI.Windows.Script.SearchListScriptType,OMS.UI' )
END
GO

IF NOT EXISTS ( SELECT scrType FROM dbo.dbScriptType WHERE scrType = 'SESSION' )
BEGIN
	INSERT dbo.dbScriptType ( scrType, scrAssemblyType )
	VALUES ( 'SESSION', 'FWBS.OMS.Script.SessionScriptType,OMS.Library' )
END
GO
	
IF NOT EXISTS ( SELECT scrType FROM dbo.dbScriptType WHERE scrType = 'WORKFLOW' )
BEGIN
	INSERT dbo.dbScriptType ( scrType, scrAssemblyType )
	VALUES ( 'WORKFLOW', 'FWBS.OMS.Script.WorkflowScriptType,OMS.Library' )
END
GO
	
IF NOT EXISTS ( SELECT scrType FROM dbo.dbScriptType WHERE scrType = 'SYSTEM' )
BEGIN
	INSERT dbo.dbScriptType ( scrType, scrAssemblyType )
	VALUES('SYSTEM', 'FWBS.OMS.Script.SystemScriptType, OMS.Library')
END
GO
	