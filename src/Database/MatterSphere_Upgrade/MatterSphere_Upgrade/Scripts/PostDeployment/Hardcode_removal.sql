IF NOT EXISTS ( SELECT 'RESOURCE' , 'CODEEXISTS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'CODEEXISTS' , '{default}' , 'The Search List Code [''%1%''] already exists..' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'SPECIFYCODE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'SPECIFYCODE' , '{default}' , 'Please specify a new Code for this New Search List?' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'SLSCRIPTEXISTS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'SLSCRIPTEXISTS' , '{default}' , 'This Search List has a Script. What would you like to do?' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CODEGR15', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CODEGR15' , '{default}' , 'Group Code cannot be greater than 15 characters' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'OBJNOTVALID', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'OBJNOTVALID' , '{default}' , '''%1%'' is not a valid object for the Field list Designer' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'VALIDFLDTYPE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'VALIDFLDTYPE' , '{default}' , 'You must enter a Field Type from the List' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'VALIDFLDNAME', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'VALIDFLDNAME' , '{default}' , 'You must enter a SQL Field Name e.g SEARCH1' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'VALIDBOUND', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'VALIDBOUND' , '{default}' , 'You must enter a Internal Value Name To replace e.g %SEARCH1%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'VALIDTEST', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'VALIDTEST' , '{default}' , 'You must enter a Literal Test Value to Replace the Internal Value with e.g TEST' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'USEDBOUND', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'USEDBOUND' , '{default}' , 'The Bound Value of ''%1%'' has already been used in another Parameter. Are you sure this is correct?' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'SCSCRIPTEXISTS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'SCSCRIPTEXISTS' , '{default}' , 'This Screen has a Script. What would you like to do?' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'REMOVEPAGE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'REMOVEPAGE' , '{default}' , 'Are you sure you want to Remove Page ''%1%''' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'OMSDESIGNER', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'OMSDESIGNER' , '{default}' , 'OMS Designer' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'STARTRETURN', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'STARTRETURN' , '{default}' , 'RmStartSession returned: 0x%1%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'VALIDWARN', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'VALIDWARN' , '{default}' , 'Validation warning' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'DELMILESTCONF', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'DELMILESTCONF' , '{default}' , 'Error Delete Milestone Config ...' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'LOADMILESTCONF', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'LOADMILESTCONF' , '{default}' , 'Error loading Milestone Config click Advanced ...' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'ADDGRTOTREE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'ADDGRTOTREE' , '{default}' , 'Would you like to add a group to the tree?' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'EMPTYTREE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'EMPTYTREE' , '{default}' , 'Empty Tree' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'DELETEGRMSG', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'DELETEGRMSG' , '{default}' , 'Deleting a group will remove all elements within the group. Are you sure you wish to continue?' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'DELETEGROUP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'DELETEGROUP' , '{default}' , 'Delete Group' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'DELMEMBERMSG', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'DELMEMBERMSG' , '{default}' , 'Are you sure you wish to delete the selected element?' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'DELETEMEMBER', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'DELETEMEMBER' , '{default}' , 'Delete Group Member' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'LOADPACKAGE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'LOADPACKAGE' , '{default}' , 'Error loading Package click Advanced ...' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'LABSELECTED', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'LABSELECTED' , '{default}' , ' - Package' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'EXPORTNOTEXIST', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'EXPORTNOTEXIST' , '{default}' , 'Error the Export Path Does not Exists please correct...' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'STARTSWITH', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'STARTSWITH' , '{default}' , 'Column ''Code, ImageIndex, Parent, Type'' is constrained to be unique' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ITEMEXISTS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ITEMEXISTS' , '{default}' , 'The Item already exists in the Package...' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ERRADDTOPAC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ERRADDTOPAC' , '{default}' , 'Error Adding to Package...' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ENTERCODE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ENTERCODE' , '{default}' , 'Enter a Script Code' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'CODEUNIQUE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'CODEUNIQUE' , '{default}' , 'You must give your scripts unique codes. The code you have entered is already in use. Please change the code for this script.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'SCRIPTCODE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'SCRIPTCODE' , '{default}' , 'Script Code' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'SLISTSWITCH', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'SLISTSWITCH' , '{default}' , 'You cannot switch SearchList whilst editing in the Admin Kit' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'MULTIPRESERR', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'MULTIPRESERR' , '{default}' , 'The Precedent you have selected for archiving is part of a multi-Precedent. Archiving it will remove it from multi-Precedent configuration. Do you wish to continue?' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'PRESARCH', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'PRESARCH' , '{default}' , 'Precedent Archiving' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'OMS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'OMS' , '{default}' , 'OMS' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'COPYSCRIPT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'COPYSCRIPT' , '{default}' , 'Copy Script' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'NEWSCRIPT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'NEWSCRIPT' , '{default}' , 'New Script' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CANCELEXC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CANCELEXC' , '{default}' , 'Cancel' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'OMSOBJSELECT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'OMSOBJSELECT' , '{default}' , 'OMS Object - ' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'TEXTFIND', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'TEXTFIND' , '{default}' , 'Enter text to find' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'OMSCODEBLD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'OMSCODEBLD' , '{default}' , 'OMS Code Builder' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'PROCDEL', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'PROCDEL' , '{default}' , 'Are you sure you wish to Delete this Procedure?' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'SCROBJERR', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'SCROBJERR' , '{default}' , 'WARNING DEBUG USE ONLY. After any changes. Please close and reopen the Scripted Object. Do you wish to continue?' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ERRORSHOW', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ERRORSHOW' , '{default}' , 'Error' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO


IF NOT EXISTS ( SELECT 'RESOURCE' , 'CODESMPL1000', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'CODESMPL1000' , '{default}' , 'Code Sample is larger then 1000 characters. Please note trimming to fit.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'ADVSCRUPGR', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'ADVSCRUPGR' , '{default}' , 'Are you sure you wish to Upgrade to Advanced Scripting any changes made in Advanced Scripting will not be applied to the old Scripting System. This current script will be the last version for the older clients to work with and converted one way only.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'INSRTSNPT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'INSRTSNPT' , '{default}' , 'Insert Snippet' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'NOSNIPPET', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'NOSNIPPET' , '{default}' , 'There are no snippets currently loaded for the current language.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'BREAKPOINT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'BREAKPOINT' , '{default}' , 'Please select some text and re-click within the indicator margin to set a breakpoint over the selected text.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'SAVESCRIPT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'SAVESCRIPT' , '{default}' , 'Please save the script before attempting to access Version Control functions.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'USRINTRFERR', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'USRINTRFERR' , '{default}' , 'User Interface Update Error' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'USRINTRFMSG', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'USRINTRFMSG' , '{default}' , 'The restoration process has completed however 3E MatterSphere has been unable to update the user interface. Please close the script window and re-open the script.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'LANGSHARP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'LANGSHARP' , '{default}' , 'The Default language is now C-Sharp this will take effect when you create a new Script' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'LANGBASIC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'LANGBASIC' , '{default}' , 'The Default language is now Visual Basic this will take effect when you create a new Script' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'LANGNOTSPEC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'LANGNOTSPEC' , '{default}' , 'The Default language is now Not Specified you will be asked to choose a language the next time you create a new script.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'LANGCAP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'LANGCAP' , '{default}' , 'Scripting' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'COPIED2CLP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'COPIED2CLP' , '{default}' , 'Copied to Clipboard!' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'DEMOCAP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'DEMOCAP' , '{default}' , 'Demo' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'DEMOMSG', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'DEMOMSG' , '{default}' , 'Demo Version Registered.  An email with demo keys will be delivered shortly.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ERRPRCXMLFRCTRL', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ERRPRCXMLFRCTRL' , '{default}' , 'ERROR Processing XML for the Control try cutting out the XML saving quiting the admin kit and the re-open and paste the XML Code back' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ERRMSGPRMSL', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ERRMSGPRMSL' , '{default}' , 'The parameters for the Search List configured for the control %1% must contain a value key.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGBKTIMFUT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGBKTIMFUT' , '{default}' , 'Are you sure you wish to book time for the future?' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'TIMEREC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'TIMEREC' , '{default}' , 'Time Recording' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'ERRMSGCNTFND', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'ERRMSGCNTFND' , '{default}' , '[%1%] control cannot be found ...' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'ERRMSGCDLMISS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'ERRMSGCDLMISS' , '{default}' , 'Country Code [''%1%''] Code Lookup Missing ...' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ERRCNTSTSLV', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ERRCNTSTSLV' , '{default}' , 'ERROR cannot set self as Slave...' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'MSGCRRALCLIC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'MSGCRRALCLIC' , '{default}' , 'Would you like to correct the allocated licenses? WARNING - Users and Terminals may be removed from a license.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'MSGRSTALCLIC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'MSGRSTALCLIC' , '{default}' , 'Would you like to restore the local licensing configuration? WARNING - Any changes made will be lost' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'MSGACTSFT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'MSGACTSFT' , '{default}' , 'Would you like to activate the software?' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'MSGDWNDLIC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'MSGDWNDLIC' , '{default}' , 'Would you like to download the licensing configuration? WARNING - Users and Terminals may be removed from a license.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'MSGSLGNTSUPP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'MSGSLGNTSUPP' , '{default}' , 'Slogan feature is not supported.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGASCDNTEXT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGASCDNTEXT' , '{default}' , 'The associate of the current document does not exist.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGERRPRMINC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGERRPRMINC' , '{default}' , 'Error UFNInfo Params Incorrect!' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGDCMCMPNSP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGDCMCMPNSP' , '{default}' , 'Document Comparing Not Supported' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGOMSDNTSPPR', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGOMSDNTSPPR' , '{default}' , 'PDF OMSApp does not support storing of precedents.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGFLDNTEXT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGFLDNTEXT' , '{default}' , 'File does not exist.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGPRMNTACPT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGPRMNTACPT' , '{default}' , 'The passed parameter is not an acceptable PDF document object.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGERRINSVG', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGERRINSVG' , '{default}' , 'Error in Saving %1%\n\n%2%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGSOMSDNTSPPR', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGSOMSDNTSPPR' , '{default}' , 'SimpleOMS OMSApp does not support storing of precedent versions.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGPRMISNTSDO', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGPRMISNTSDO' , '{default}' , 'The passed parameter is not a Simple Document object.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGNTSUPP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGNTSUPP' , '{default}' , 'Object (%1%) not supported for save searching' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGWHLNG', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGWHLNG' , '{default}' , 'Which language do you want to search?%1%%2%You may need to close this window for better search results if you change this setting.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CPTCLT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CPTCLT' , '{default}' , 'Culture?' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGERRINGRP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGERRINGRP' , '{default}' , 'Error in Group ''%1%''. TSelect Error' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ENQSYS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ENQSYS' , '{default}' , 'The Enquiry : %1% is a System Enquiry and can not be deleted' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ENQDEL', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ENQDEL' , '{default}' , 'Are you sure you wish to Delete Enquiry : %1%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'NOVERSCOMP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'NOVERSCOMP' , '{default}' , 'No versions to compare current version to.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'CANNOTCOMP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'CANNOTCOMP' , '{default}' , 'Cannot compare' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'COMMREQ', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'COMMREQ' , '{default}' , 'Comments Required' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'DATAEXISTS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'DATAEXISTS' , '{default}' , 'Version Data Already Exists' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'COMMREQMSG', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'COMMREQMSG' , '{default}' , 'Please provide a comment advising of the changes made.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'DATAEXISTSMSG', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'DATAEXISTSMSG' , '{default}' , 'Data for this version (%1%) already exists in the tables which store version data.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'UNXPERR', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'UNXPERR' , '{default}' , 'Unexpected error in Report ''%1%''. Please contact support.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'DBCRERR', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'DBCRERR' , '{default}' , 'Error creating Database Connection for Report Link!' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'OSMHCIC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'OSMHCIC' , '{default}' , 'Invalid Microsoft Report Viewer. Please contact support. Quote OSMHCIC%1%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGASKMTBEVLD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGASKMTBEVLD' , '{default}' , 'The assembly api key must be a valid GUID format.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGTPISNTSUPP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGTPISNTSUPP' , '{default}' , '''%1%'' is not supported.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGTSTFAILSRVC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGTSTFAILSRVC' , '{default}' , 'Testing failed for the connectable service of ''%1%''' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGNTWNTAVL', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGNTWNTAVL' , '{default}' , 'The network is not available.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGEXCLMDNTIM', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGEXCLMDNTIM' , '{default}' , 'Exclusion Mode:''%1%'' not yet implemented' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGARCHNTVLD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGARCHNTVLD' , '{default}' , 'The Archiving Directory ID (%1%) is not valid' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGASMSTBESPC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGASMSTBESPC' , '{default}' , 'Associate to copy to must be specified.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGINVDCTP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGINVDCTP' , '{default}' , 'Invalid Document Type' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGASCSNMTBESPC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGASCSNMTBESPC' , '{default}' , 'Associate to send to must be specified.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGEMLMSTBSPC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGEMLMSTBSPC' , '{default}' , 'Email precedent must be specified.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGVSPRPMBST', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGVSPRPMBST' , '{default}' , 'Visible property must be set.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGNBLCHVSDC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGNBLCHVSDC' , '{default}' , 'Unable to change visibility of document %1%: ''%2%''' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGCSNTSPONPV', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGCSNTSPONPV' , '{default}' , 'ChangeStorage is not supported on a Precedent Version, you must change the Storage provider of the primary Precedent.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGPRPRCMTHSM', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGPRPRCMTHSM' , '{default}' , 'The parent precedent must be the same as the current precedent' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGPRPRCMTHSMWP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'MSGPRPRCMTHSMWP' , '{default}' , 'The parent precedent must be the same as the current working precedent' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CLMDNTTOTBLVRS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CLMDNTTOTBLVRS' , '{default}' , 'Column ''%1%'' does not belong to table VERSIONS.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ERRGTXTRINFFLD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ERRGTXTRINFFLD' , '{default}' , 'Error Getting Extra Info Field %1% Probably Not Initialized' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'STRITMNDSPEC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'STRITMNDSPEC' , '{default}' , 'Storage item needs to be specified for the local document cache filter.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'DCMDRCNTEXST', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'DCMDRCNTEXST' , '{default}' , 'The document directory on the file system does not exist or is disconnected.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'PRCDRCNTEXST', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'PRCDRCNTEXST' , '{default}' , 'The precedent directory on the file system does not exist or is disconnected.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'UNBLRDNLSTNG', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'UNBLRDNLSTNG' , '{default}' , 'Unable to change a read only setting.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'STRGTMNTSPP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'STRGTMNTSPP' , '{default}' , 'Storage item type ''%1%''is not supported by provider type ''%2%''' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'PRVTPDSNTSPFTR', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'PRVTPDSNTSPFTR' , '{default}' , 'The provider type ''%1%'' does not support feature ''%2%''' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'PRVTPSPBTNIMFTR', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'PRVTPSPBTNIMFTR' , '{default}' , 'The provider type ''%1%'' supports but has not implemented feature ''%2%''' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'XMLFLNTVLD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'XMLFLNTVLD' , '{default}' , 'Xml file not a valid OMS Core Import File.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ADDRXMLNTXP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ADDRXMLNTXP' , '{default}' , 'Address XML Node Expected.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'PRDFCTDNTMTCH', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'PRDFCTDNTMTCH' , '{default}' , 'Pre-Defined Contact does not match. - Code: %1%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CFGCTDNTMTCH', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CFGCTDNTMTCH' , '{default}' , 'Configured Contact Type not valid. - Type: %1%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CTDFADDREQ', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CTDFADDREQ' , '{default}' , 'A contacts Default Address is required.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CNTNMREQ', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CNTNMREQ' , '{default}' , 'Contact Name is required.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CNTXMLNDEXP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CNTXMLNDEXP' , '{default}' , 'Contact XML Node Expected.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'PRDFCLDNTMTCHCD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'PRDFCLDNTMTCHCD' , '{default}' , 'Pre Defined Client Code does not match. - Code: %1%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'DFTCNTREQ', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'DFTCNTREQ' , '{default}' , 'A Default Contact is required.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CLNAMREQ', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CLNAMREQ' , '{default}' , 'Client Name is required.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CFGCLTPNTVLD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CFGCLTPNTVLD' , '{default}' , 'Configured Client Type not valid. - Type: %1%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CLTXMLNDEXP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CLTXMLNDEXP' , '{default}' , 'Client XML Node Expected.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'PRDFFLCDNTMTCH', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'PRDFFLCDNTMTCH' , '{default}' , 'Pre-defined File Code does not match. - Code: %1%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'FLDSCRREQ', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'FLDSCRREQ' , '{default}' , 'File Description is required.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CFGFLTPNTVLD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CFGFLTPNTVLD' , '{default}' , 'Configured File Type not valid. - Type: %1%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'FLXMLNDEXP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'FLXMLNDEXP' , '{default}' , 'File XML Node Expected.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'PRDFCNTDNTMTCH', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'PRDFCNTDNTMTCH' , '{default}' , 'Pre-Defined Contact Code does not match. - Code: %1%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CNTREQASSOC', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CNTREQASSOC' , '{default}' , 'Contact required for associate.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ASSXMLNDEXP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ASSXMLNDEXP' , '{default}' , 'Associate XML Node Expected.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CRSESSDADISNL', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CRSESSDADISNL' , '{default}' , 'CurrentSession.DistributedAssembliesDirectory is null' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CHKUPDASUNXPRS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CHKUPDASUNXPRS' , '{default}' , 'Check for Updated Assembly return unexpected result.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'PRPDSNTEXT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'PRPDSNTEXT' , '{default}' , 'Property does not exist' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'NOINSTSET', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'NOINSTSET' , '{default}' , 'No instance set.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ADVSCRDTRLTDT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ADVSCRDTRLTDT' , '{default}' , 'Cannot be an advanced security script does not have the relevant data.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'SCRMTHVNMBFCMP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'SCRMTHVNMBFCMP' , '{default}' , 'The script must have a name before it is compiled.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'SCRASCNTBEFND', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'SCRASCNTBEFND' , '{default}' , 'Script Assembly cannot be found.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'NBLTLDSCRERCRCS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'NBLTLDSCRERCRCS' , '{default}' , 'Unable to Load Script Error creating Class ''%1%''' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ADDCNTBELDINDS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ADDCNTBELDINDS' , '{default}' , 'Addin cannot be loaded when it is inactive/disabled' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'APNMNTLSDTACS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'APNMNTLSDTACS' , '{default}' , 'The application / assembly with the name ''%1%'' is not licensed to access the %2% API.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'NTVLDUSRINF', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'NTVLDUSRINF' , '{default}' , 'Not valid user information.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'NTVLDTRMINF', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'NTVLDTRMINF' , '{default}' , 'Not valid terminal information.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'NOBJPLSCRTRVW', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'NOBJPLSCRTRVW' , '{default}' , 'Error No ''FWBS.OMS.DESIGN.EXPORT.TREEVIEW'' Object Please create a blank TreeView and Recompile' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ERRNOOMSOBJFRCD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ERRNOOMSOBJFRCD' , '{default}' , 'Error no OMS Object for Code : %1%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ERRNOSCRFRCD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ERRNOSCRFRCD' , '{default}' , 'Error no Script for Code : %1%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ERRNOWRKFFRCD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ERRNOWRKFFRCD' , '{default}' , 'Error no Workflow for Code : %1%' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CPPKGGTPKCNDDP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CPPKGGTPKCNDDP' , '{default}' , 'You have copied a Package. Please go to Package and Deploy and Update this Package' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'PRCLRDEXT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'PRCLRDEXT' , '{default}' , 'Precedent : ''%1%, %2%, %3%'' already exists skipping' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'FLDTOCP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'FLDTOCP' , '{default}' , 'Failed to Copy : ''%1%'' to ''%2%\\%3%''' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'PRCISMSGFRMPKG', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'PRCISMSGFRMPKG' , '{default}' , 'Precedent : ''%1%'' is missing from the package' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ERRRPSRVLCNTST', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ERRRPSRVLCNTST' , '{default}' , 'Error Reporting Server location has not been set. Please enter the Location by going to the File Menu and choosing Change Subsription Server' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'STRGTMDNSPFT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'STRGTMDNSPFT' , '{default}' , 'The storage item type ''%1%'' does not support feature ''%2%''' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'UNHNDLDCMD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'UNHNDLDCMD' , '{default}' , 'Unhandled command.  Please make sure the Run Command exists and that you are logged in.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'PRBLLDSCRFRMM', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'PRBLLDSCRFRMM' , '{default}' , 'Problem Loading Script From Main Menu ''%1%''' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CMDBRCRPTNPRNT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CMDBRCRPTNPRNT' , '{default}' , 'Command bar is corrupt.  It has no parent.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'BJISINDTCST', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'BJISINDTCST' , '{default}' , 'The object is in a detached state.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'OBJISNTCMBJ', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'OBJISNTCMBJ' , '{default}' , 'The object to be attached is not a com object.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CNTSPWNOUTL', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CNTSPWNOUTL' , '{default}' , 'Cannot spawn outlook if the process is currently the addin instance.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'OUTHSLNGRSP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'OUTHSLNGRSP' , '{default}' , 'Outlook has taken too long to respond.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'OUTNTCRTD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'OUTNTCRTD' , '{default}' , 'Outlook Application not created.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'CMDBTNNTRGSTR', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'CMDBTNNTRGSTR' , '{default}' , 'ExecuteButton: CommandBarButton has not been registered. %1% has not been able to capture your key press.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'PRPRFLDCDNTBFND', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'PRPRFLDCDNTBFND' , '{default}' , 'The attempted operation failed.  An object could not be found.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'EXPLREQTOSCHR', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'EXPLREQTOSCHR' , '{default}' , 'An active explorer is required to synchronise the item.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'ADDFR07USD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'ADDFR07USD' , '{default}' , 'You are using a %1% Office Addin that was designed for Microsoft Office 2007.  You may continue using the addin but some problems may occur.' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'NODFLTMLSTP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'NODFLTMLSTP' , '{default}' , 'No default email setup' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'NVLDCLNMB', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'NVLDCLNMB' , '{default}' , 'Invalid client Number' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'NVLDFLNMB', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'NVLDFLNMB' , '{default}' , 'Invalid file Number' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'NOAPPPSDTCTR', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'NOAPPPSDTCTR' , '{default}' , 'No OMSAPP Passed to Constructor in ucFolderPage' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'WNAMENOTFOUND', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'WNAMENOTFOUND' , '{default}' , 'Filename window cannot be found!' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'FILENOTSAVED', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'FILENOTSAVED' , '{default}' , 'The dialog interceptor could not save the file correctly' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'MESSAGE' , 'COMMNOTSUPP', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'MESSAGE' , 'COMMNOTSUPP' , '{default}' , 'Command Not Supported' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'DOCFOLDERS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'DOCFOLDERS' , '{default}' , 'Document Folders' , 1 , 0 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'DOCPREVIEW', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'DOCPREVIEW' , '{default}' , 'Document Preview' , 1 , 0 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'RGTHCLKCFOROPTS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'RGTHCLKCFOROPTS' , '{default}' , 'Right Click for Options' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'WIPHOURS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'WIPHOURS' , '{default}' , 'WIP Hours' , 0 , 1 , NULL , NULL , NULL , 0)
END
 
IF NOT EXISTS ( SELECT 'DASHBOARD' , 'WIPTIME', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'WIPTIME' , '{default}' , 'WIP Time' , 0 , 1 , NULL , NULL , NULL , 0)
END
 
IF NOT EXISTS ( SELECT 'DASHBOARD' , 'WIPCOSTS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'WIPCOSTS' , '{default}' , 'WIP Costs' , 0 , 1 , NULL , NULL , NULL , 0)
END
 
IF NOT EXISTS ( SELECT 'DASHBOARD' , 'WIPCHARGES', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'WIPCHARGES' , '{default}' , 'WIP Charges' , 0 , 1 , NULL , NULL , NULL , 0)
END
 
IF NOT EXISTS ( SELECT 'DASHBOARD' , 'WIPTOTAL', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'WIPTOTAL' , '{default}' , 'WIP Total' , 0 , 1 , NULL , NULL , NULL , 0)
END
 
IF NOT EXISTS ( SELECT 'DASHBOARD' , 'CLNTS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'CLNTS' , '{default}' , 'Clients' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'MTTRS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'MTTRS' , '{default}' , 'Matters' , 0 , 1 , NULL , NULL , NULL , 0)
END 

IF NOT EXISTS ( SELECT 'RESOURCE' , 'MORE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'RESOURCE' , 'MORE' , '{default}' , 'More' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'RESOURCE' , 'ENTRCRDNTLS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'RESOURCE' , 'ENTRCRDNTLS' , '{default}' , 'Enter your credentials' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'RESOURCE' , 'CRDNTLWLSBEUSD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'RESOURCE' , 'CRDNTLWLSBEUSD' , '{default}' , 'These credentials will be used to connect to ''%1%''' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'RESOURCE' , 'mnuEXIT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'RESOURCE' , 'mnuEXIT' , '{default}' , 'E&xit' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'MESSAGE' , 'MSGGRBGCLCTED', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'MESSAGE' , 'MSGGRBGCLCTED' , '{default}' , 'Garbage Collection Successful...' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF EXISTS ( SELECT 'RESOURCE' , 'CLOSE', '{default}' , '&Close', NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdDesc , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
UPDATE dbo.dbCodeLookup SET cdDesc = 'Close'
WHERE cdType = 'RESOURCE' AND cdCode = 'CLOSE' AND cdUICultureInfo = '{default}' AND cdDesc = '&Close' AND cdAddLink IS NULL
END

IF EXISTS ( SELECT 'RESOURCE' , 'CANCEL', '{default}' , 'Cance&l', NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdDesc , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
UPDATE dbo.dbCodeLookup SET cdDesc = 'Cancel'
WHERE cdType = 'RESOURCE' AND cdCode = 'CANCEL' AND cdUICultureInfo = '{default}' AND cdDesc = 'Cance&l' AND cdAddLink IS NULL
END

IF EXISTS ( SELECT 'RESOURCE' , 'MNUCOPY', '{default}' , '&Copy To Clipboard', NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdDesc , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
UPDATE dbo.dbCodeLookup SET cdDesc = '&Copy'
WHERE cdType = 'RESOURCE' AND cdCode = 'MNUCOPY' AND cdUICultureInfo = '{default}' AND cdDesc = '&Copy To Clipboard' AND cdAddLink IS NULL
END

IF EXISTS ( SELECT 'RESOURCE' , 'CULTURE', '{default}' , 'Culture : ', NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdDesc , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
UPDATE dbo.dbCodeLookup SET cdDesc = 'Culture'
WHERE cdType = 'RESOURCE' AND cdCode = 'CULTURE' AND cdUICultureInfo = '{default}' AND cdDesc = 'Culture : ' AND cdAddLink IS NULL
END