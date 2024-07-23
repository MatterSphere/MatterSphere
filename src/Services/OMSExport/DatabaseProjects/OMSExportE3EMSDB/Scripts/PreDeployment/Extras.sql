DECLARE @VIEW NVARCHAR(MAX)
	, @BASECHEMATABLENAME NVARCHAR(max) 
	, @SQL NVARCHAR(MAX)

--ADD COLUMN USED TO STORE ENTITY ID FROM E3E - WILL PUT FORWARD FOR INCLUSION IN CORE

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbContact')
-- Advanced Security enabled
	SET @BASECHEMATABLENAME = N'config.dbContact'
ELSE
-- Non-Advanced Security
	SET @BASECHEMATABLENAME = N'dbo.dbContact'

EXEC FWBSADDCOLUMN 
	@TABLENAME = @BASECHEMATABLENAME
	, @COLUMNNAME = 'contExtID'
	, @COLUMNDESC = 'INT NULL'

--ADD COLUMN USED TO STORE ENTITY ID FROM E3E - WILL PUT FORWARD FOR INCLUSION IN CORE
EXEC FWBSADDCOLUMN 
	@TABLENAME = @BASECHEMATABLENAME 
	, @COLUMNNAME = 'contNeedExport' --NEED A TRIGGER FOR THIS FROM DBCONTACT AND RELATED TABLES!
	, @COLUMNDESC = 'BIT NULL DEFAULT ( 1 )' --CHECK THIS FOR REPLICATION!

-- Add NOT NULL property for update flag
IF EXISTS (
  SELECT 1
    FROM sys.all_columns
   WHERE OBJECT_ID = OBJECT_ID(@BASECHEMATABLENAME, 'U')
     AND name = 'contNeedExport'
     AND is_nullable = 1)
    BEGIN
        SET @SQL = 'UPDATE ' + @BASECHEMATABLENAME + '
                    SET    contNeedExport = 1
                    WHERE  contNeedExport IS NULL

                    ALTER TABLE ' + @BASECHEMATABLENAME + '
                    ALTER COLUMN contNeedExport BIT NOT NULL'
        EXEC (@SQL)
    END

/************************** Refresh VIEWs ********************************/		
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

--NEW COLUMNS FOR 3E PHASE, TASK AND ACTIVITY CODE
EXEC FWBSADDCOLUMN 
	@TABLENAME = 'dbTimeLedger' 
	, @COLUMNNAME = 'E3E_PHASECODE'
	, @COLUMNDESC = 'VARCHAR ( 10 ) NULL'

EXEC FWBSADDCOLUMN 
	@TABLENAME = 'dbTimeLedger' 
	, @COLUMNNAME = 'E3E_TASKCODE'
	, @COLUMNDESC = 'VARCHAR ( 10 ) NULL'

EXEC FWBSADDCOLUMN 
	@TABLENAME = 'dbTimeLedger' 
	, @COLUMNNAME = 'E3E_ACTIVITYCODE'
	, @COLUMNDESC = 'VARCHAR ( 10 ) NULL'

-- EXTRA COLUMNS ADDED TO DBTIMELEDGER
EXEC FWBSADDCOLUMN --COLUMN TO STORE LOCAL RECORDED DATE TIME IN - POPULATED IN SCRIPTING ON TIME SCREEN WHERE USED IF NEEDED FOR MULTI-OFFICE SCENARIOS IN DIFFERENT TIME ZONES - TEXT FIELD SO DOES NOT GET TRANSLATED TO UTC AGAIN - NVARCHAR AS LOWEST SQL PLATFORM IS SQL 2005
@TABLENAME = 'dbTimeLedger'
, @COLUMNNAME = 'timeRecorded_Actual'
, @COLUMNDESC = 'datetime NULL'

EXEC FWBSADDCOLUMN --COLUMN TO STORE LOCAL RECORDED TIME ZONE - POPULATED IN SCRIPTING ON TIME SCREEN WHERE USED IF NEEDED FOR MULTI-OFFICE SCENARIOS IN DIFFERENT TIME ZONES - TEXT FIELD SO DOES NOT GET TRANSLATED TO UTC AGAIN - NVARCHAR AS LOWEST SQL PLATFORM IS SQL 2005
@TABLENAME = 'dbTimeLedger'
, @COLUMNNAME = 'timeRecorded_tz'
, @COLUMNDESC = 'NVARCHAR ( 100 )'

SET @BASECHEMATABLENAME = N'dbo.dbTimeLedger'

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

--ASSOCIATES UPDATES JULY 2014
--DBASSOCIATETYPE
EXEC fwbsAddColumn 
	@Tablename = 'dbAssociateType'
	, @ColumnName = 'typeAccCode'
	, @columnDesc = 'NVARCHAR ( 64 ) NULL'

EXEC fwbsAddColumn 
	@Tablename = 'dbAssociateType'
	, @ColumnName = 'typeAccRelationshipCode'
	, @columnDesc = 'NVARCHAR ( 64 ) NULL'

SET @BASECHEMATABLENAME = N'dbo.dbAssociateType'

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

--DBASSOCIATES

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbAssociates')
-- Advanced Security enabled
	SET @BASECHEMATABLENAME = N'config.dbAssociates'
ELSE
-- Non-Advanced Security
	SET @BASECHEMATABLENAME = N'dbo.dbAssociates'

EXEC fwbsAddColumn 
	@Tablename = @BASECHEMATABLENAME
	, @ColumnName = 'assocExtTxtID'
	, @columnDesc = 'NVARCHAR ( 36 ) NULL' --ALLOW NULL AS THIS CAUSES ISSUES WITH REPLICATION

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

EXEC FWBSADDCOLUMN
@TABLENAME = 'DBFINANCIALLEDGER'
, @COLUMNNAME = 'finExtID'
, @COLUMNDESC = 'INT NULL'

EXEC FWBSADDCOLUMN
@TABLENAME = 'DBFINANCIALLEDGER'
, @COLUMNNAME = 'finExtTxtID'
, @COLUMNDESC = 'NVARCHAR ( 36 ) NULL'

SET @BASECHEMATABLENAME = N'dbo.DBFINANCIALLEDGER'

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

--USED IF UPDATES NEEDED TO FLAG EFFECTIVE DATE UPDATES REQUIRED
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbFile')
-- Advanced Security enabled
	SET @BASECHEMATABLENAME = N'config.dbFile'
ELSE
-- Non-Advanced Security
	SET @BASECHEMATABLENAME = N'dbo.dbFile'

EXEC FWBSADDCOLUMN 
	@TABLENAME = @BASECHEMATABLENAME 
	, @COLUMNNAME = 'fileE3EEffectiveDatedNeedUpdate' 
	, @COLUMNDESC = 'BIT NOT NULL DEFAULT ( 0 )'

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

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbClient')
-- Advanced Security enabled
	SET @BASECHEMATABLENAME = N'config.dbClient'
ELSE
-- Non-Advanced Security
	SET @BASECHEMATABLENAME = N'dbo.dbClient'

EXEC FWBSADDCOLUMN 
	@TABLENAME = @BASECHEMATABLENAME 
	, @COLUMNNAME = 'clE3EEffectiveDatedNeedUpdate' 
	, @COLUMNDESC = 'BIT NOT NULL DEFAULT ( 0 )'

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


IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tgrExportContactToAccounts]'))
DROP TRIGGER [dbo].[tgrExportContactToAccounts]
GO

--TRIGGER NO LONGER REQUIRED - EDITING CONTACTINDIVIDUAL IN MATTERCENTRE UPDATES THROUGH EXTENDED DATA LAYER UPDATES CONTACT RECORD
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tgrExportContactIndividualToAccounts]'))
DROP TRIGGER [dbo].[tgrExportContactIndividualToAccounts]
GO
--TRIGGER NO LONGER REQUIRED - EDITING CONTACTCOMPANY IN MATTERCENTRE UPDATES CONTACT THROUGH EXTENDED DATA LAYER UPDATES CONTACT RECORD
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tgrExportContactCompanyToAccounts]'))
DROP TRIGGER [dbo].[tgrExportContactCompanyToAccounts]
GO

---------------
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[config].[tgrExportContactToAccounts]'))
DROP TRIGGER [config].[tgrExportContactToAccounts]
GO

--TRIGGER NO LONGER REQUIRED - EDITING CONTACTINDIVIDUAL IN MATTERCENTRE UPDATES THROUGH EXTENDED DATA LAYER UPDATES CONTACT RECORD
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[config].[tgrExportContactIndividualToAccounts]'))
DROP TRIGGER [config].[tgrExportContactIndividualToAccounts]
GO
--TRIGGER NO LONGER REQUIRED - EDITING CONTACTCOMPANY IN MATTERCENTRE UPDATES CONTACT THROUGH EXTENDED DATA LAYER UPDATES CONTACT RECORD
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[config].[tgrExportContactCompanyToAccounts]'))
DROP TRIGGER [config].[tgrExportContactCompanyToAccounts]
GO

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tgrFileE3EEffectiveDatedNeedUpdate]'))
	DROP TRIGGER [dbo].[tgrFileE3EEffectiveDatedNeedUpdate]
GO

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[config].[tgrFileE3EEffectiveDatedNeedUpdate]'))
	DROP TRIGGER [config].[tgrFileE3EEffectiveDatedNeedUpdate]
GO

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tgrClientE3EEffectiveDatedNeedUpdate]'))
	DROP TRIGGER [dbo].[tgrClientE3EEffectiveDatedNeedUpdate]
GO

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[config].[tgrClientE3EEffectiveDatedNeedUpdate]'))
	DROP TRIGGER [config].[tgrClientE3EEffectiveDatedNeedUpdate]
GO

DECLARE @BASECHEMANAME NVARCHAR(MAX) 
	, @Cmd NVARCHAR(MAX)

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbContact')
-- Advanced Security enabled
	SET @BASECHEMANAME = N'[config].'
ELSE
-- Non-Advanced Security
	SET @BASECHEMANAME = N'[dbo].'

SET @Cmd = N'
CREATE TRIGGER ' + @BASECHEMANAME + N'[tgrExportContactToAccounts] ON ' + @BASECHEMANAME + N'[dbContact]
FOR UPDATE  NOT FOR REPLICATION
AS
if not update(contneedexport)
begin
	update dbContact set contNeedExport = 1 where contID in (select contID from inserted)
end'

EXEC sp_executesql @Cmd;

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'dbFile')
-- Advanced Security enabled
	SET @BASECHEMANAME = N'[config].'
ELSE
-- Non-Advanced Security
	SET @BASECHEMANAME = N'[dbo].'

SET @Cmd = N'
CREATE TRIGGER ' + @BASECHEMANAME + N'[tgrFileE3EEffectiveDatedNeedUpdate] ON ' + @BASECHEMANAME + N'[dbFile]
FOR UPDATE NOT FOR REPLICATION
AS
	/*
		TRIGGER TO HANDLE KNOWN COLUMNS NEEDED TO BE EXPORTED FOR EFFECTIVEDATED INFORMATION
		SOME CHECKS ARE PERFORMED TO PREVENT ADJUSTING TGREXPORTFILETOACCOUNTS TRIGGER FOR NESTED TRIGGERS
		SINGLE ROW UPDATES HANDLED ONLY
	*/
	
	DECLARE @ROWCOUNT INT
	SELECT @ROWCOUNT = COUNT(*) FROM INSERTED
	
	IF @ROWCOUNT = 1
	BEGIN
		--PERFORM UPDATE FOR COLUMNS NEEDED TO FLAG FOR EFFECTIVE DATE UPDATE
		IF UPDATE ( BRID ) OR UPDATE ( FILEDEPARTMENT ) OR UPDATE ( FILEPRINCIPLEID ) OR UPDATE ( FILERESPONSIBLEID ) OR UPDATE ( FILEMANAGERID )
			UPDATE DBFILE SET FILEE3EEFFECTIVEDATEDNEEDUPDATE = 1 WHERE FILEID IN ( SELECT FILEID FROM INSERTED )

		--IF FILENEEDEXPORT IS UPDATED, CHECK THE RESULT
		IF UPDATE ( FILENEEDEXPORT ) 
		BEGIN
			DECLARE @FILENEEDEXPORT BIT
		
			SELECT @FILENEEDEXPORT = FILENEEDEXPORT FROM INSERTED
			IF @FILENEEDEXPORT = 0 
			BEGIN
				UPDATE 
					DBFILE 
				SET 
					FILEE3EEFFECTIVEDATEDNEEDUPDATE = 0 
					, DBFILE.FILENEEDEXPORT = DBFILE.FILENEEDEXPORT --SET THIS TO PREVENT OTHER TRIGGER FIRING
				FROM 
					DBFILE 
				INNER JOIN 
					INSERTED ON INSERTED.FILEID = DBFILE.FILEID 
				WHERE 
					INSERTED.FILENEEDEXPORT = 0
			END 
		END 
	END
'
EXEC sp_executesql @Cmd;

--CLIENT TABLE
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'config' AND TABLE_NAME = 'DBCLIENT')
-- Advanced Security enabled
	SET @BASECHEMANAME = N'[config].'
ELSE
-- Non-Advanced Security
	SET @BASECHEMANAME = N'[dbo].'

SET @Cmd = N'
CREATE TRIGGER ' + @BASECHEMANAME + N'tgrClientE3EEffectiveDatedNeedUpdate ON ' + @BASECHEMANAME + N'DBCLIENT
FOR UPDATE NOT FOR REPLICATION
AS
	/*
		TRIGGER TO HANDLE KNOWN COLUMNS NEEDED TO BE EXPORTED FOR EFFECTIVEDATED INFORMATION
		SOME CHECKS ARE PERFORMED TO PREVENT ADJUSTING TGREXPORTCLIENTTOACCOUNTS TRIGGER FOR NESTED TRIGGERS
		SINGLE ROW UPDATES HANDLED ONLY
	*/
	
	DECLARE @ROWCOUNT INT
	SELECT @ROWCOUNT = COUNT(*) FROM INSERTED
	
	IF @ROWCOUNT = 1
	BEGIN
		IF UPDATE ( BRID ) 
			UPDATE DBCLIENT SET CLE3EEFFECTIVEDATEDNEEDUPDATE = 1 WHERE CLID IN ( SELECT CLID FROM INSERTED )

		--IF CLNEEDEXPORT IS UPDATED, CHECK THE RESULT
		IF UPDATE ( CLNEEDEXPORT ) 
		BEGIN
			DECLARE @CLNEEDEXPORT BIT
			
			SELECT @CLNEEDEXPORT = CLNEEDEXPORT FROM INSERTED
			IF @CLNEEDEXPORT = 0 
			BEGIN
				UPDATE 
					DBCLIENT 
				SET 
					CLE3EEFFECTIVEDATEDNEEDUPDATE = 0 
					, DBCLIENT.CLNEEDEXPORT = DBCLIENT.CLNEEDEXPORT --SET THIS TO PREVENT OTHER TRIGGER FIRING
				FROM 
					DBCLIENT 
				INNER JOIN 
					INSERTED ON INSERTED.CLID = DBCLIENT.CLID 
				WHERE 
					INSERTED.CLNEEDEXPORT = 0
			END 
		END 
	END'

EXEC sp_executesql @Cmd;
GO


/****** Object:  UserDefinedFunction [dbo].[GetHtmlEncode]    Script Date: 05/15/2013 14:49:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetHtmlEncode]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetHtmlEncode]
GO

--ONLY CREATE - AS MAY BE ROLLED INTO CORE DATABASE AND COULD CHANGE IN FUTURE SO WILL ERROR IF ALREADY EXISTS - CREATE FUNCTION HAS TO BE FIRST STATEMENT IN BATCH - RUN UN-COMMENTED IF NEEDED
CREATE FUNCTION [dbo].[GetHtmlEncode] ( @VALUE NVARCHAR ( MAX ) , @PRESERVENEWLINE BIT = 0 )
RETURNS NVARCHAR ( MAX )
AS
BEGIN
	DECLARE @RESULT NVARCHAR ( MAX )
	SET @RESULT = @VALUE
	IF @RESULT IS NOT NULL AND LEN ( @RESULT ) > 0
	BEGIN
	SET @RESULT = REPLACE ( @RESULT , N'&', N'&amp;')
	SET @RESULT = REPLACE ( @RESULT , N'<', N'<')
	SET @RESULT = REPLACE ( @RESULT , N'>', N'>')
	SET @RESULT = REPLACE ( @RESULT , N'''', N'&#39;')
	SET @RESULT = REPLACE ( @RESULT , N'"', N'&quot;')
	IF @PRESERVENEWLINE = 1
		SET @RESULT = REPLACE( @RESULT , CHAR(10) , CHAR(10) + N'<br>')
	END
	RETURN @RESULT
END
GO


-- STUB STORED PROCEDURES
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sprE3ELogFinancialError]') AND type in (N'P', N'PC'))
EXEC (N'CREATE PROCEDURE [dbo].[sprE3ELogFinancialError] @FinID bigint, @Message nvarchar(2000) AS
	BEGIN
		PRINT CAST(@FinID AS NVARCHAR(20)) + N'': '' + @Message
	END
')
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sprC3ELogFinancialError]') AND type in (N'P', N'PC'))
EXEC (N'CREATE PROCEDURE [dbo].[sprC3ELogFinancialError] @FinID bigint, @Message nvarchar(2000) AS
	BEGIN
		PRINT CAST(@FinID AS NVARCHAR(20)) + N'': '' + @Message
	END
')
GO

-- Migrate InvoiceSync timestamp from dbRegInfo XML to dbState table
DECLARE @brID AS int, @regXML AS XML, @invoiceSyncTime datetime
SELECT TOP 1 @brID = brID, @regXML = regXML FROM [dbo].[dbRegInfo]
SET @invoiceSyncTime = @regXML.value('(/config/InvoiceSync)[1]', 'datetime')
IF @invoiceSyncTime IS NOT NULL
BEGIN
	IF NOT EXISTS (SELECT * FROM dbState WHERE stateCode = '3E_INVOICE_SYNC' AND brID IS NULL AND usrID IS NULL)
	INSERT INTO dbState (stateCode, stateData) VALUES ('3E_INVOICE_SYNC', @invoiceSyncTime)
	
	SET @regXML.modify('delete /config/InvoiceSync')
	UPDATE[dbo].[dbRegInfo] SET regXML = CAST(@regXML AS NVARCHAR(MAX)) WHERE brID = @brID
END
GO