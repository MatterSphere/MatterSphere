
exec fwbsaddcolumn 'dbUser', 'usrPowerUserProfileID', 'int'
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

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_dbUser_dbPowerUserProfiles') AND parent_object_id = OBJECT_ID(N'dbo.dbUser'))
ALTER TABLE dbo.dbUser ADD CONSTRAINT [FK_dbUser_dbPowerUserProfiles] FOREIGN KEY ([usrPowerUserProfileID]) REFERENCES [dbo].[dbPowerUserProfiles] ([id]) NOT FOR REPLICATION
