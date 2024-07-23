IF NOT EXISTS ( SELECT 'RESOURCE' , 'ADDAPPOINTMENT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'ADDAPPOINTMENT' , '{default}' , 'Add New Appointment' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'NEWAPPOINTMENT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'NEWAPPOINTMENT' , '{default}' , 'New Appointment' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'EDITAPPOINTMENT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'EDITAPPOINTMENT' , '{default}' , 'Edit Appointment' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'BTNDONE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'BTNDONE' , '{default}' , 'Done' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'BTNCANCEL', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'BTNCANCEL' , '{default}' , 'Cancel' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'ENQQUESTCUETXT' , 'ADD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'ENQQUESTCUETXT' , 'ADD' , '{default}' , 'Add...' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'ENQQUESTCUETXT' , 'CHOOSE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'ENQQUESTCUETXT' , 'CHOOSE' , '{default}' , 'Choose...' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'ENQQUESTCUETXT' , 'FILTERBY', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'ENQQUESTCUETXT' , 'FILTERBY' , '{default}' , 'Filter by...' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'HIDEBUTTONS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'HIDEBUTTONS' , '{default}' , 'Hide Cancel/Save Buttons' , 0 , 1 , NULL , NULL , NULL , 0)
END