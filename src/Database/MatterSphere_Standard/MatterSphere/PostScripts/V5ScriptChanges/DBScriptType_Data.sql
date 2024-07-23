
print 'Starting V5ScriptChanges\DBScriptType_Data.sql'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO