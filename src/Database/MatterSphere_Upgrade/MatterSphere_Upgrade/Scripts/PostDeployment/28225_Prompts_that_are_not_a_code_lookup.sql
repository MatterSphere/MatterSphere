IF NOT EXISTS ( SELECT 'MESSAGE' , 'CLIENTNOTFOUND' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CLIENTNOTFOUND' , '{default}' , '%CLIENT% ''%1%'' cannot be found within the database. Or you have insufficient permission to view the %CLIENT%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CLIENTNOTFNDF' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CLIENTNOTFNDF' , '{default}' , '%CLIENT% ''%1% in Field %2%'' cannot be found within the database. Or you have insufficient permission to view the %CLIENT%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'OMSFILENOTFOUND' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'OMSFILENOTFOUND' , '{default}' , 'The specified %FILE% ''%1%'' cannot be found within the database. Or you have insufficient permission to view the %FILE%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'OMSFILENOTFNDF' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'OMSFILENOTFNDF' , '{default}' , 'The specified %FILE% ''%1% in Field %2%'' cannot be found within the database. Or you have insufficient permission to view the %FILE%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'RESEXCELEDIT' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'RESEXCELEDIT' , '{default}' , 'Excel is currently in edit mode.  The command cannot be executed.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO
