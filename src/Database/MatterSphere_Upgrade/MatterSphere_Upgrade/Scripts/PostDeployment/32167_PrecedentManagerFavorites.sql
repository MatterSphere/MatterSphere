IF NOT EXISTS ( SELECT 'SLBUTTON' , 'PRECADDTOFAV', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'SLBUTTON' , 'PRECADDTOFAV' , '{default}' , 'Add To Favourites' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'SLBUTTON' , 'PRECREMOVEFAV', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'SLBUTTON' , 'PRECREMOVEFAV' , '{default}' , 'Remove From Favourites' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO