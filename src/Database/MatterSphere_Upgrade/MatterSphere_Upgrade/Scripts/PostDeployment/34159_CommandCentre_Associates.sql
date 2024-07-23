IF NOT EXISTS ( SELECT 'RESOURCE' , 'ADDASSOCIATE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'ADDASSOCIATE' , '{default}' , 'Add New Associate' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'RESOURCE' , 'NEWASSOCIATE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'NEWASSOCIATE' , '{default}' , 'New Associate' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'RESOURCE' , 'NEWCLIENT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'NEWCLIENT' , '{default}' , 'New %CLIENT%' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'RESOURCE' , 'NEWCONTACT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'NEWCONTACT' , '{default}' , 'New Contact' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'RESOURCE' , 'NEWMATTER', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'NEWMATTER' , '{default}' , 'New Matter' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'RESOURCE' , 'NEWPRECLIENT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'NEWPRECLIENT' , '{default}' , 'New Pre-%CLIENT%' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'SLPBUTTON' , 'FLGDEFAULT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'SLPBUTTON' , 'FLGDEFAULT' , '{default}' , 'Flag as Default' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'SLPBUTTON' , 'COPYTO', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'SLPBUTTON' , 'COPYTO' , '{default}' , 'Copy To' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'ENQQUESTION' , 'THEIRREFER', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'ENQQUESTION' , 'THEIRREFER' , '{default}' , 'Their Reference' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'ENQQUESTION' , 'ASSOCADDR', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'ENQQUESTION' , 'ASSOCADDR' , '{default}' , 'Associate Address' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'ENQQUESTCUETXT' , 'ADDSALUT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'ENQQUESTCUETXT' , 'ADDSALUT' , '{default}' , 'Add Salutation' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'ENQQUESTCUETXT' , 'ADDREF', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'ENQQUESTCUETXT' , 'ADDREF' , '{default}' , 'Add Reference' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'ENQQUESTCUETXT' , 'ADDHEAD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'ENQQUESTCUETXT' , 'ADDHEAD' , '{default}' , 'Add Heading' , 0 , 1 , NULL , NULL , NULL , 0)
END

-- Hide cancel/save buttons on Key Date tab in Command Center
DECLARE @xml XML;

DECLARE @T TABLE (rn INT PRIMARY KEY, lookup NVARCHAR(100), hidebuttons NVARCHAR(5))

DECLARE @rn INT
	, @lookup NVARCHAR(100)
	, @hidebuttons NVARCHAR(5)
	, @updated BIT = 0

SET @xml = (SELECT typeXML  FROM dbo.dbFileType WHERE typeCode = 'TEMPLATE')

INSERT INTO @T (rn, lookup, hidebuttons)
SELECT  ROW_NUMBER() OVER ( ORDER BY node ) AS rn,
    node.value('@lookup', 'NVARCHAR(100)') AS lookup,
    node.value('@hidebuttons', 'NVARCHAR(5)') AS hidebuttons
FROM    @xml.nodes('/Config/Dialog/Tabs/Tab') x ( node )

DELETE @T WHERE lookup NOT IN ('CLASSOCIATES')

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