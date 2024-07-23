IF NOT EXISTS ( SELECT 'RESOURCE' , 'SCHACTIONBUT', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'SCHACTIONBUT' , '{default}' , 'Action Buttons' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'RESOURCE' , 'SCHPAGINATION', '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'RESOURCE' , 'SCHPAGINATION' , '{default}' , 'Pagination' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'SLPBUTTON' , 'RECEIVE' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'SLPBUTTON' , 'RECEIVE' , '{default}' , 'Receive' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'SLPBUTTON' , 'ALLCLIVIEW' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'SLPBUTTON' , 'ALLCLIVIEW' , '{default}' , 'Allow Client View' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO

IF NOT EXISTS ( SELECT 'SLPBUTTON' , 'LOCKCLIVIEW' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'SLPBUTTON' , 'LOCKCLIVIEW' , '{default}' , 'Lock Client View' , 0 , 1 , NULL , NULL , NULL , 0)
END
GO


DECLARE @VIEW nvarchar(MAX)
EXEC fwbsaddcolumn 'dbo.dbSearchListConfig', 'schPagination', 'BIT'
EXEC fwbsaddcolumn 'dbo.dbSearchListConfig', 'schActionButton', 'BIT'

/************************** Refresh VIEWs ********************************/		
SET @VIEW = (
		SELECT TOP 1 QUOTENAME(s.name) + N'.' + QUOTENAME(o.name)
		FROM sys.sql_expression_dependencies ed
			INNER JOIN sys.objects o ON o.object_id = ed.referencing_id
			INNER JOIN sys.schemas s ON s.schema_id = o.schema_id
		WHERE ed.referenced_id = OBJECT_ID('dbo.dbSearchListConfig')
			AND o.type_desc = 'VIEW'
		ORDER BY QUOTENAME(s.name) + N'.' + QUOTENAME(o.name)
		)
WHILE @VIEW IS NOT NULL
BEGIN
	EXEC sp_refreshview @VIEW;
	PRINT 'sp_refreshview ' + @VIEW;

	SET @VIEW = (
			SELECT TOP 1 QUOTENAME(s.name) + N'.' + QUOTENAME(o.name)
			FROM sys.sql_expression_dependencies ed
				INNER JOIN sys.objects o ON o.object_id = ed.referencing_id
				INNER JOIN sys.schemas s ON s.schema_id = o.schema_id
			WHERE ed.referenced_id = OBJECT_ID('dbo.dbSearchListConfig')
				AND o.type_desc = 'VIEW'
				AND QUOTENAME(s.name) + N'.' + QUOTENAME(o.name) > @VIEW
			ORDER BY QUOTENAME(s.name) + N'.' + QUOTENAME(o.name)
			)


END


GO
