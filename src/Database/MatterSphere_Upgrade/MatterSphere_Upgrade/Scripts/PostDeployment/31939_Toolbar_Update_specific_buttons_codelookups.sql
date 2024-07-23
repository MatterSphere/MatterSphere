IF NOT EXISTS ( SELECT 'CBCCAPTIONS' , 'VIEWOMSTASKS2' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'CBCCAPTIONS' , 'VIEWOMSTASKS2' , '{default}' , '%APPNAME% Tasks' , 0 , 1 , NULL , 'Views %APPNAME% related tasks.' , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'CBCCAPTIONS' , 'VIEWMYTASKS' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'CBCCAPTIONS' , 'VIEWMYTASKS' , '{default}' , 'My Tasks' , 0 , 1 , NULL , 'Views the currently logged in user''s tasks list.' , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'CBCCAPTIONS' , 'VIEWFEETASKS2' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'CBCCAPTIONS' , 'VIEWFEETASKS2' , '{default}' , '%FEEEARNER% Tasks' , 0 , 1 , NULL , 'Views the current %FEEEARNERS% task list.' , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'CBCCAPTIONS' , 'VIEWOTHTASKS2' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'CBCCAPTIONS' , 'VIEWOTHTASKS2' , '{default}' , 'Other Tasks' , 0 , 1 , NULL , 'Views another person''s task list.' , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'CBCCAPTIONS' , 'TASKSGROUP' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'CBCCAPTIONS' , 'TASKSGROUP' , '{default}' , 'Tasks' , 0 , 1 , NULL , 'Task management.' , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'CBCCAPTIONS' , 'ADDTASK2' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'CBCCAPTIONS' , 'ADDTASK2' , '{default}' , 'Add New Task' , 0 , 1 , NULL , 'Creates a new task.' , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'CBCCAPTIONS' , 'CALENDARGROUP' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'CBCCAPTIONS' , 'CALENDARGROUP' , '{default}' , 'Calendar' , 0 , 1 , NULL , 'Views a calendar within Outlook.' , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'CBCCAPTIONS' , 'MYCALENDAR' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'CBCCAPTIONS' , 'MYCALENDAR' , '{default}' , 'My Calendar' , 0 , 1 , NULL , 'Views the currently logged in calendar.' , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'CBCCAPTIONS' , 'FEECALENDAR' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'CBCCAPTIONS' , 'FEECALENDAR' , '{default}' , '%FEEEARNER% Calendar' , 0 , 1 , NULL , 'View the current %FEEEARNERS% calendar.' , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'CBCCAPTIONS' , 'OTHCALENDAR' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'CBCCAPTIONS' , 'OTHCALENDAR' , '{default}' , 'Other Calendar' , 0 , 1 , NULL , 'Views another person''s calendar.' , NULL , 0)
END
GO
