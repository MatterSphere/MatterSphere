IF NOT EXISTS ( SELECT ctrlID FROM [dbo].[dbEnquiryControl] WHERE ctrlID = 108 )
	INSERT [dbo].[dbEnquiryControl] ( ctrlID , ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType )
	VALUES (108, 'SearchConflicts', 'OMS', 0, 'FWBS.OMS.UI.Windows.ucSearchConflicts,OMS.UI', NULL, NULL)
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'OPENBUT' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'RESOURCE' , 'OPENBUT' , '{default}' , 'Open' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'MESSAGE' , 'SLERROR' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'MESSAGE' , 'SLERROR' , '{default}' , 'Unexpected error in Search List ''%1%''. Please contact support.' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'RESOURCE' , 'CONFCSEARCH' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'RESOURCE' , 'CONFCSEARCH' , '{default}' , 'Searching...' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'RESOURCE' , 'CONFFOUND' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'RESOURCE' , 'CONFFOUND' , '{default}' , 'Found' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'MESSAGE' , 'SLNOTSUPP' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'MESSAGE' , 'SLNOTSUPP' , '{default}' , 'Searchlist ''%1%'' is not supported.' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'MESSAGE' , 'IDNOTVALID' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'MESSAGE' , 'IDNOTVALID' , '{default}' , '''%1%'' is not a valid ID.' , 0 , 1 , NULL , NULL , NULL , 0)
END

----
IF NOT EXISTS ( SELECT 'RESOURCE' , 'NOLOCATIONFOUND' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'RESOURCE' , 'NOLOCATIONFOUND' , '{default}' , 'No Location found' , 0 , 1 , NULL , NULL , NULL , 0)
END


IF NOT EXISTS ( SELECT 'RESOURCE' , 'NOORGFOUND' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'RESOURCE' , 'NOORGFOUND' , '{default}' , 'No Organization found' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'RESOURCE' , 'NOMATTERSFOUND' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'RESOURCE' , 'NOMATTERSFOUND' , '{default}' , 'No Matters found' , 0 , 1 , NULL , NULL , NULL , 0)
END