IF NOT EXISTS ( SELECT 'RESOURCE' , 'CMDSEARCH', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
	BEGIN
		INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
		VALUES ( 'RESOURCE' , 'CMDSEARCH' , '{default}' , 'Search' , 0 , 1 , NULL , NULL , NULL , 0)
	END
ELSE
	BEGIN
		UPDATE dbo.dbCodeLookup 
			SET cdDesc = 'Search'
		WHERE cdType =  'RESOURCE' AND  cdCode = 'CMDSEARCH' AND  cdUICultureInfo = '{default}' AND cdAddLink IS NULL
	END
GO