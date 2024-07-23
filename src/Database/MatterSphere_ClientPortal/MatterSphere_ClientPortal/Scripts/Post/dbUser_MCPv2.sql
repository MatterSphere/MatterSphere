
IF NOT EXISTS ( SELECT [name] from sys.Columns WHERE object_id = object_id('dbUser') AND [name] = 'usrMCPToken' )
BEGIN
	ALTER TABLE [dbo].[dbUser]
	ADD usrMCPToken nVarChar(50) NULL
END
GO

IF NOT EXISTS ( SELECT [name] from sys.Columns WHERE object_id = object_id('dbUser') AND [name] = 'usrMCPPWReset' )
BEGIN
	ALTER TABLE [dbo].[dbUser]
	ADD usrMCPPWReset datetime NULL
END
GO

IF NOT EXISTS ( SELECT [name] from sys.Columns WHERE object_id = object_id('dbUser') AND [name] = 'usrDocumentNotification' )
BEGIN
	ALTER TABLE [dbo].[dbUser]
	ADD usrDocumentNotification bit NULL
END
GO

IF NOT EXISTS ( SELECT [name] from sys.Columns WHERE object_id = object_id('dbUser') AND [name] = 'usrDocNotifyFeeEarnerManager' )
BEGIN
	ALTER TABLE [dbo].[dbUser]
	ADD usrDocNotifyFeeEarnerManager bit NULL
END
GO

DECLARE @VIEW nvarchar(MAX)
DECLARE @BASECHEMATABLENAME nvarchar(max) = N'dbo.dbUser '
SET @VIEW = (
            SELECT TOP 1 QUOTENAME(s.name) + N'.' + QUOTENAME(o.name)
            FROM sys.sql_expression_dependencies ed
                INNER JOIN sys.objects o ON o.object_id = ed.referencing_id
                INNER JOIN sys.schemas s ON s.schema_id = o.schema_id
            WHERE ed.referenced_id = OBJECT_ID(@BASECHEMATABLENAME)
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
            WHERE ed.referenced_id = OBJECT_ID(@BASECHEMATABLENAME)
                AND o.type_desc = 'VIEW'
                AND QUOTENAME(s.name) + N'.' + QUOTENAME(o.name) > @VIEW
            ORDER BY QUOTENAME(s.name) + N'.' + QUOTENAME(o.name)
            )
END

GO