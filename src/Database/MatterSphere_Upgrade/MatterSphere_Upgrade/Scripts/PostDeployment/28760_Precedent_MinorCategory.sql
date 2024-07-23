
IF NOT EXISTS ( SELECT 'PRECGROUP' , 'PRECMINORCAT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'PRECGROUP' , 'PRECMINORCAT' , '{default}' , 'Precedent Minor Categories' , 1 , 1 , NULL , NULL , NULL , 1)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'PRECMINORCAT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'PRECMINORCAT' , '{default}' , '5. Minor Category' , 1 , 0 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'DOCUMENTS' , 'PRECMINORCAT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'DOCUMENTS' , 'PRECMINORCAT' , '{default}' , '%PRECEDENT% Minor Category' , 1 , 0 , NULL , NULL , NULL , 1)
END
GO

IF NOT EXISTS ( SELECT 'ENQQUESTION' , 'PRECMINORCAT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'ENQQUESTION' , 'PRECMINORCAT' , '{default}' , '%PRECEDENT% Minor Category' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'DOCUMENTS' , 'PRECMINORCAT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'DOCUMENTS' , 'PRECMINORCAT' , '{default}' , '%PRECEDENT% Minor Category' , 1 , 0 , NULL , NULL , NULL , 1)
END
GO