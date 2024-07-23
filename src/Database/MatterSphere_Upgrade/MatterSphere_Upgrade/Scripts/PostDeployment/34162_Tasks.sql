IF NOT EXISTS ( SELECT 'SLBUTTON' , 'GENREPORT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'SLBUTTON' , 'GENREPORT' , '{default}' , 'Generate Report' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'SLBUTTON' , 'ADDNEW', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'SLBUTTON' , 'ADDNEW' , '{default}' , 'Add New' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'RESOURCE' , 'ADDTASK', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'ADDTASK' , '{default}' , 'Add New Task' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'NEWTASK', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'NEWTASK' , '{default}' , 'New Task' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'EDITTASK', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'EDITTASK' , '{default}' , 'Edit Task' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'ENQQUESTION' , 'APPTDESC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'ENQQUESTION' , 'APPTDESC' , '{default}' , 'Appointment Description' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'ENQQUESTION' , 'SYNCFEECALEND', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'ENQQUESTION' , 'SYNCFEECALEND' , '{default}' , 'Synchronize with the Fee Earners Calendar' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

-- Hide cancel/save buttons on Task tabs in Command Center
DECLARE @xml XML;

DECLARE @T TABLE (rn INT PRIMARY KEY, lookup NVARCHAR(100), hidebuttons NVARCHAR(5))

DECLARE @rn INT
	, @lookup NVARCHAR(100)
	, @hidebuttons NVARCHAR(5)
	, @updated BIT = 0

SET @xml = (SELECT typeXML  FROM dbo.dbCommandCentreType WHERE typeCode = 'STANDARD')

INSERT INTO @T (rn, lookup, hidebuttons)
SELECT  ROW_NUMBER() OVER ( ORDER BY node ) AS rn,
    node.value('@lookup', 'NVARCHAR(100)') AS lookup,
    node.value('@hidebuttons', 'NVARCHAR(5)') AS hidebuttons
FROM    @xml.nodes('/Config/Dialog/Tabs/Tab') x ( node )


SELECT @rn = rn, @hidebuttons = hidebuttons FROM @T WHERE lookup = '1788437295'

IF @rn IS NOT NULL
	IF @hidebuttons IS NULL
	BEGIN
		SET @xml.modify('insert attribute hidebuttons {"True"} into (/Config/Dialog/Tabs/Tab[sql:variable("@rn")])[1]')
		SET @updated = 1
	END

IF @updated = 1
	UPDATE dbo.dbCommandCentreType
		SET typeXML = CAST(@xml AS NVARCHAR(MAX))
	WHERE typeCode = 'STANDARD'


DELETE @T
SET @updated = 0

SET @xml = (SELECT typeXML  FROM dbo.dbFileType WHERE typeCode = 'TEMPLATE')

INSERT INTO @T (rn, lookup, hidebuttons)
SELECT  ROW_NUMBER() OVER ( ORDER BY node ) AS rn,
    node.value('@lookup', 'NVARCHAR(100)') AS lookup,
    node.value('@hidebuttons', 'NVARCHAR(5)') AS hidebuttons
FROM    @xml.nodes('/Config/Dialog/Tabs/Tab') x ( node )

DELETE @T WHERE lookup NOT IN ('227574651', '1788437295')

SET @rn = (SELECT TOP 1 rn FROM @T ORDER BY rn)
WHILE @rn IS NOT NULL
BEGIN
	SET @hidebuttons = (SELECT hidebuttons FROM @T WHERE rn = @rn)
	IF @hidebuttons IS NULL
	BEGIN
		SET @xml.modify('insert attribute hidebuttons {"True"} into (/Config/Dialog/Tabs/Tab[sql:variable("@rn")])[1]')
		SET @updated = 1
	END
	SET @rn = (SELECT TOP 1 rn FROM @T WHERE rn > @rn ORDER BY rn)
END

IF @updated = 1
	UPDATE dbo.dbFileType
		SET typeXML = CAST(@xml AS NVARCHAR(MAX))
	WHERE typeCode = 'TEMPLATE'