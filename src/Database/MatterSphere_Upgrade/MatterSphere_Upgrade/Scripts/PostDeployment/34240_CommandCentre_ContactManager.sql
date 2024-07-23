IF NOT EXISTS ( SELECT 'SLPBUTTON' , 'VIEWDETAILS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'SLPBUTTON' , 'VIEWDETAILS' , '{default}' , 'View Details' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DLGGROUPCAPTION' , 'NAVGRPCLIENTS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'DLGGROUPCAPTION' , 'NAVGRPCLIENTS' , '{default}' , 'Clients' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DLGGROUPCAPTION' , 'CMNDCTRCAPTION', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'DLGGROUPCAPTION' , 'CMNDCTRCAPTION' , '{default}' , 'Command Centre' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DLGGROUPCAPTION' , 'DASHBRDCAPTION', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'DLGGROUPCAPTION' , 'DASHBRDCAPTION' , '{default}' , 'Dashboard' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DLGTABCAPTION' , 'CONTACTLIST', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'DLGTABCAPTION' , 'CONTACTLIST' , '{default}' , 'Contact List' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'RESOURCE' , 'ADDCONTACT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'ADDCONTACT' , '{default}' , 'Add New Contact' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'RESOURCE' , 'CREATEENTITY', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'CREATEENTITY' , '{default}' , 'Create %1%' , 0 , 1 , NULL , NULL , NULL , 0)
END

DECLARE @xml XML
	, @updated BIT = 0

SET @xml = (SELECT CAST(typeXML AS XML) FROM dbo.dbCommandCentreType WHERE typeCode = 'STANDARD')

IF NOT EXISTS(SELECT 1 FROM @xml.nodes('/Config/Dialog/Tabs/Tab') x ( node ) WHERE node.value('@lookup', 'NVARCHAR(MAX)') = 'CONTACTLIST')
BEGIN
	SET @xml.modify('insert <Tab lookup="CONTACTLIST" source="SCHCONTLIST" tabtype="List" group="NAVGRPCLIENTS" hidebuttons="True" glyph="39" conditional="Ispackageinstalled(&quot;CommandCentre&quot;)" /> as first into (/Config/Dialog/Tabs)[1]')
	SET @updated = 1
END

IF @updated = 1
	UPDATE dbo.dbCommandCentreType
		SET typeXML = CAST(@xml AS NVARCHAR(MAX))
	WHERE typeCode = 'STANDARD'
