IF NOT EXISTS ( SELECT 'MESSAGE' , 'COLUMNCELLTYPE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'COLUMNCELLTYPE' , '{default}' , 'Cell must be a ''%1%'' type' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'SLBUTTON' , 'FILTERS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'SLBUTTON' , 'FILTERS' , '{default}' , 'Filters' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO
IF NOT EXISTS ( SELECT 'SLBUTTON' , 'RESETALL', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'SLBUTTON' , 'RESETALL' , '{default}' , 'Reset All' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO