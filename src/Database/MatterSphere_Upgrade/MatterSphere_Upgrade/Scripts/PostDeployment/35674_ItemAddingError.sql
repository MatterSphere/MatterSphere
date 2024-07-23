IF NOT EXISTS ( SELECT 'RESOURCE' , 'ITEMNOTADDED', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'ITEMNOTADDED' , '{default}' , 'Item ''%1%'' could not be added. Please contact your System Administrator.' , 0 , 1 , NULL , NULL , NULL , 0)
END