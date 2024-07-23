IF NOT EXISTS ( SELECT 'ENQQUESTCUETXT' , 'CHOOSETEAM', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'ENQQUESTCUETXT' , 'CHOOSETEAM' , '{default}' , 'Choose Team' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO