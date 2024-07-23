IF NOT EXISTS ( SELECT 'DASHBOARD' , 'PASTDUE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'PASTDUE' , '{default}' , 'Past Due' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'DUESOON', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'DUESOON' , '{default}' , 'Due Soon' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'ONTIME', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'ONTIME' , '{default}' , 'On Time' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'CMPLT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'CMPLT' , '{default}' , 'Complete' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'ADDTIME', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'ADDTIME' , '{default}' , 'Add Time' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'VWCLNT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'VWCLNT' , '{default}' , 'View Client' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'FILE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'FILE' , '{default}' , 'Matter' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'BUDGET', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'BUDGET' , '{default}' , 'Budget' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'DSCRPTN', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'DSCRPTN' , '{default}' , 'Description' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'ACTNS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'ACTNS' , '{default}' , 'Actions' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'FLLIST', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'FLLIST' , '{default}' , 'Matter List' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'FLFORRVW', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'FLFORRVW' , '{default}' , 'Matters for Review' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'DELETE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'DELETE' , '{default}' , 'Delete' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'POPOUT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'POPOUT' , '{default}' , 'Pop Out' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'GRDSTTNGS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'GRDSTTNGS' , '{default}' , 'Grid Settings' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'MXMZ', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'MXMZ' , '{default}' , 'Maximize' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'MNMZ', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'MNMZ' , '{default}' , 'Minimize' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'MARKCOMPL', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'MARKCOMPL' , '{default}' , 'Mark As Complete' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'ASSIGN', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'ASSIGN' , '{default}' , 'Assign' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'UNASSIGN', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'UNASSIGN' , '{default}' , 'Unassign' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'MYTSKS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'MYTSKS' , '{default}' , 'My Tasks' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'TEAMTSKS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'TEAMTSKS' , '{default}' , 'Team Tasks' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'ALLTSKS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'ALLTSKS' , '{default}' , 'All Tasks' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'DUE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'DUE' , '{default}' , 'Due' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'STATUS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'STATUS' , '{default}' , 'Status' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'TYPE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'TYPE' , '{default}' , 'Type' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'CRTDBY', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'CRTDBY' , '{default}' , 'Created By' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'TMNAME', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'TMNAME' , '{default}' , 'Team Name' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'ASSGNDTO', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'ASSGNDTO' , '{default}' , 'Assigned To' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'NOSPACE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'NOSPACE' , '{default}' , 'Not enough space' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'ADD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'ADD' , '{default}' , 'Add' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'SAVE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'SAVE' , '{default}' , 'Save' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'RCNTS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'RCNTS' , '{default}' , 'Recents' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'FVRTS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'FVRTS' , '{default}' , 'Favorites' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'DTMDFD', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'DTMDFD' , '{default}' , 'Date Modified' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'KDTS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'KDTS' , '{default}' , 'Key Dates' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'CLNDR', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'CLNDR' , '{default}' , 'Calendar' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'VWFL', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'VWFL' , '{default}' , 'View Matter' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'DSHBRDKDCLNTILE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'DSHBRDKDCLNTILE' , '{default}' , 'Key Dates / Calendar' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'DSHBRDCLNTILE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'DSHBRDCLNTILE' , '{default}' , 'Calendar' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'DSHBRDMLTILE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'DSHBRDMLTILE' , '{default}' , 'Matter List / Matters for Review' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'DSHBRDRFTILE', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'DSHBRDRFTILE' , '{default}' , 'Recents / Favorites' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'WKUPCS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'WKUPCS' , '{default}' , 'Week' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'MTNGS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'MTNGS' , '{default}' , 'Meetings' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'ALLDAY', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'ALLDAY' , '{default}' , 'All day' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'FVRT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'FVRT' , '{default}' , 'Favorite' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'NTFVRT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'NTFVRT' , '{default}' , 'Not Favorite' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'ADDTSK', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'ADDTSK' , '{default}' , 'Add New Task' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'EDTTSK', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'EDTTSK' , '{default}' , 'Edit Task' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'SEARCHLISTS', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'SEARCHLISTS' , '{default}' , 'Search Lists' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF NOT EXISTS ( SELECT 'DASHBOARD' , 'RVWDT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'DASHBOARD' , 'RVWDT' , '{default}' , 'Review Date' , 0 , 1 , NULL , NULL , NULL , 0)
END