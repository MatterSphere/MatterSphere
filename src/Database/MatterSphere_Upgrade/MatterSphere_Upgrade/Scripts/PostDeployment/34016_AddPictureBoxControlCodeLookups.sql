IF NOT EXISTS ( SELECT 'RESOURCE' , 'BRWSFRIMG' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
VALUES ( 'RESOURCE' , 'BRWSFRIMG' , '{default}' , 'Browse for Image ...' , 0 , 1 , NULL , NULL , NULL , 0)
END

IF EXISTS ( SELECT 'RESOURCE' , 'SLCTIMGPLCHLDR' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
DELETE dbo.dbCodeLookup WHERE cdType = 'RESOURCE' AND cdCode = 'SLCTIMGPLCHLDR' AND cdUICultureInfo = '{default}'
END