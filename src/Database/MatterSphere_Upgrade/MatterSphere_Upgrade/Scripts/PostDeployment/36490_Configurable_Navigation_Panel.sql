IF NOT EXISTS ( SELECT 'RESOURCE' , 'ACTIONSHEADER', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'ACTIONSHEADER' , '{default}' , 'Actions' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'INFPANEL', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'INFPANEL' , '{default}' , 'Information Panel' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO