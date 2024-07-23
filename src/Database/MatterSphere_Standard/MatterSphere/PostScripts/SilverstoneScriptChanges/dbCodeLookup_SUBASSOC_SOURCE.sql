
Print 'SilverstoneScriptChanges\dbCodeLookup_SUBASSOC_SOURCE.sql'

IF NOT EXISTS ( SELECT 'SUBASSOC' , 'SOURCE' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'SUBASSOC' , 'SOURCE' , '{default}' , 'Source Of Business' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO



