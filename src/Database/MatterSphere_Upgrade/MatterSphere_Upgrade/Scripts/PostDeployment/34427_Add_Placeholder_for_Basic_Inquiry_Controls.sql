IF NOT EXISTS ( SELECT 'RESOURCE' , 'CUETEXT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'CUETEXT' , '{default}' , 'Cue Text' , 1 , 1 , NULL , NULL , NULL , 0)
END
GO