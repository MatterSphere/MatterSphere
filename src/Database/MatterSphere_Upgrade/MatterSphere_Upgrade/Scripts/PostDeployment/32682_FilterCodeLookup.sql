IF NOT EXISTS ( SELECT 'RESOURCE' , 'FILTERBY2', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'FILTERBY2' , '{default}' , '*Filter By...' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO