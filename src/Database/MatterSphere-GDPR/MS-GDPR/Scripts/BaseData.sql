TRUNCATE TABLE [dbo].[PII_Tables]
GO
/**** IF Stage 1 not Required don't set PII_UIHIDDEN *******/
INSERT INTO [dbo].[PII_Tables]
 ([PII_Schema],[PII_TableName],[PII_ISController],[PII_UIHidden],[PII_Rule])
--VALUES ( 'dbo','dbContact',1,1,'contisclient = 0 AND contTypeCode not in (''CLIENT'') AND DATEDIFF (dd,isnull(Last_Activity,Updated),getdate()) >= 2190')
VALUES ( 'dbo','dbContact',1,1,'ishidden=1')
GO

INSERT INTO [dbo].[PII_Tables]
 ([PII_Schema],[PII_TableName],[PII_ISController],[PII_UIHidden],[PII_Rule])
VALUES ( 'dbo','dbClient',1,1,'ishidden=1')
GO


INSERT INTO [dbo].[PII_Tables]
 ([PII_Schema],[PII_TableName],[PII_ISController],[PII_UIHidden])
select 'dbo',TABLE_NAME,0,1 from INFORMATION_SCHEMA.TABLES 
Where TABLE_NAME in ('dbAssociates','dbDocument','dbFile')
GO

INSERT INTO [dbo].[PII_Tables]
 ([PII_Schema],[PII_TableName],[PII_ISController],[PII_UIHidden])
select 'dbo',TABLE_NAME,0,0 from INFORMATION_SCHEMA.TABLES 
Where TABLE_NAME in ('dbContactAddresses','dbContactNUmbers','dbContactEmails','dbContactIndividual','dbClientContacts')
GO

TRUNCATE TABLE [dbo].[PII_Fields]
GO

INSERT INTO [dbo].[PII_Fields]
VALUES (1,'dbo','dbContact','contName',1,NULL,'PII DATA REMOVED'),
(1,'dbo','dbContact','isHidden',2,2,NULL),
(1,'dbo','dbContact','contSalut',1,NULL,NULL),
(1,'dbo','dbContact','contAddressee',1,NULL,NULL),
(1,'dbo','dbContact','contAddInfo',1,NULL,NULL),
(1,'dbo','dbContact','contdefaultAddress',2,-99,NULL),
(1,'dbo','dbContact','contNotes',1,NULL,NULL),
(1,'dbo','dbContact','contWebsite',1,NULL,NULL),
(1,'dbo','dbContact','contSource',1,NULL,NULL),
(1,'dbo','dbContact','contIdent',1,NULL,NULL),
(1,'dbo','dbContact','contIdent2',1,NULL,NULL),
(1,'dbo','dbContact','contShortName',1,NULL,NULL),
(1,'dbo','dbClient','cldefaultcontact',2,-99,NULL),
(1,'dbo','dbContactIndividual','contID',0,NULL,NULL),
(1,'dbo','dbContactAddresses','contID',0,NULL,NULL),
(1,'dbo','dbContactNUmbers','contID',0,NULL,NULL),
(1,'dbo','dbContactEmails','contID',0,NULL,NULL),
(1,'dbo','dbAssociates','assocHeading',1,NULL,'REMOVED'),
(1,'dbo','dbAssociates','assocSalut',1,NULL,'REMOVED'),
(1,'dbo','dbAssociates','assocAddressee',1,NULL,NULL),
(1,'dbo','dbAssociates','assocRef',1,NULL,NULL),
(1,'dbo','dbAssociates','assocNotes',1,NULL,NULL),
(1,'dbo','dbAssociates','assocDDI',1,NULL,NULL),
(1,'dbo','dbAssociates','assocFax',1,NULL,NULL),
(1,'dbo','dbAssociates','assocEmail',1,NULL,NULL),
(1,'dbo','dbAssociates','assocMobile',1,NULL,NULL),
(2,'dbo','dbClient','clName',1,NULL,'PII DATA REMOVED'),
(2,'dbo','dbClient','isHidden',2,2,NULL),
(2,'dbo','dbClient','clDefaultAddress',2,-99,NULL),
(2,'dbo','dbClient','clDefaultContact',2,-99,NULL),
(2,'dbo','dbClient','clsearch1',1,NULL,NULL),
(2,'dbo','dbClient','clsearch2',1,NULL,NULL),
(2,'dbo','dbClient','clsearch3',1,NULL,NULL),
(2,'dbo','dbClient','clsearch4',1,NULL,NULL),
(2,'dbo','dbClient','clsearch5',1,NULL,NULL),
(2,'dbo','dbClientContacts','clID',0,NULL,NULL);


GO
TRUNCATE TABLE [dbo].[PII_Table_Join]
GO
INSERT INTO [dbo].[PII_Table_Join]
VALUES (1,'CONTID','dbContact','CONTID',1,'CONTID'),
(1,'CONTID','dbContactAddresses','contID',1,'CONTID'),
(1,'CONTID','dbContactIndividual','contID',1,'CONTID'),
(1,'CONTID','dbContactNUmbers','contID',1,'CONTID'),
(1,'CONTID','dbContactEmails','contID',1,'CONTID'),
(1,'CONTID','dbAssociates','contID',1,'ASSOCID'),
(1,'CONTID','dbClient','cldefaultcontact',0,NULL),
(2,'CLID','dbClient','clID',1,'CLID'),
(2,'CLID','dbClientContacts','clID',1,'ID');

GO

IF NOT EXISTS (SELECT 1 FROM [config].[SystemPolicyconfig] WHERE [SecurableType] = 'SYSTEM' AND [Permission] = 'PIIADMIN' )
	UPDATE [config].[SystemPolicyconfig] set [SecurableType] = 'SYSTEM', [Permission] = 'PIIADMIN' WHERE [Number] = (SELECT max( [Number])  FROM [config].[SystemPolicyConfig] WHERE SecurableType is not null)+1
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[dbCodeLookup] WHERE [cdTYPE] = 'PERMISSION' AND [cdCODE] = 'PIIADMIN')
	INSERT INTO [dbo].[dbCodeLookup]
	([cdType]
		  ,[cdCode]
		  ,[cdUICultureInfo]
		  ,[cdDesc]
		  ,[cdSystem]
		  ,[cdDeletable])
	VALUES 
	('PERMISSION'
		  ,'PIIADMIN'
		  ,'{default}'
		  ,'PII Administrator'
		  ,0
		  ,0)
	  
 GO

IF NOT EXISTS (select 1 from [dbo].[dbAddress] where addid = -99)
BEGIN
SET IDENTITY_INSERT [dbo].[dbAddress] ON;

	INSERT INTO [dbo].[dbAddress]
	([addID]
		  ,[addLine1]
		  ,[Created]
		  ,[Updated])
	 VALUES (-99,'PII_HIDDEN',getdate(),getdate());

 SET IDENTITY_INSERT [dbo].[dbAddress] OFF;
 END

 GO

IF EXISTS (select 1 from INFORMATION_SCHEMA.TABLES Where Table_Schema = 'Config' And Table_Name = 'dbContact')
BEGIN
	IF NOT EXISTS (select 1 from [config].[dbContact] where contid = -99)
	BEGIN

	 SET IDENTITY_INSERT [config].[dbContact] ON;

		INSERT INTO [config].[dbContact]
		 ([contid],[contIsClient]
			  ,[contTypeCode]
			  ,[contName]
			  ,[contDefaultAddress]
			  ,[contSalut]
			  ,[Created]
			  ,[Updated])
		VALUES (-99,1,'INDIVIDUAL','PII DATA REMOVED',-99,'REMOVED',getdate(),getdate());

	 SET IDENTITY_INSERT [config].[dbContact] OFF;
	END
END

GO

	IF NOT EXISTS (select 1 from [dbo].[dbContact] where contid = -99)
	BEGIN
		SET IDENTITY_INSERT [dbo].[dbContact] ON;

		INSERT INTO [dbo].[dbContact]
			([contid],[contIsClient]
				,[contTypeCode]
				,[contName]
				,[contDefaultAddress]
				,[contSalut]
				,[Created]
				,[Updated])
		VALUES (-99,1,'INDIVIDUAL','PII DATA REMOVED',-99,'REMOVED',getdate(),getdate());

		SET IDENTITY_INSERT [dbo].[dbContact] OFF;
	END

 GO

IF NOT EXISTS (SELECT 1 FROM config.ChangeVersionControl)
	INSERT INTO config.ChangeVersionControl VALUES (0,0)
GO

IF EXISTS(SELECT 1 from Sys.columns c join sys.tables t on t.object_id = c.object_id where t.NAME = 'systempolicy' AND c.NAME = 'isdefault')
BEGIN

	declare @sql nvarchar ( max ) 


	IF NOT EXISTS (SELECT 1 FROM [config].[SystemPolicy] WHERE [TYPE] = 'PIIADMIN')
	BEGIN

		set @sql = '
		INSERT into [config].[SystemPolicy]
		([Type],AllowMask,DenyMask,[Name],IsDefault)
		values (''PIIADMIN'',convert(binary(32),''0x0000000400000000000000000000000000000000000000000000000000000000'',1),convert(binary(32),''0x0000000000000000000000000000000000000000000000000000000000000000'',1),''PII ADMINISTRATOR ONLY'',NULL)'
		exec (@sql)
	END
	

	IF NOT EXISTS (SELECT 1 FROM [config].[SystemPolicy] WHERE [TYPE] = 'GLOBALSYSDEF')
	BEGIN
		set @sql = 'INSERT into [config].[SystemPolicy]
		([ID],[Type],AllowMask,DenyMask,[Name],IsDefault)
		values (''3CC3BD00-7D7E-4D4A-96C6-44E44E140C5E'',''GLOBALSYSDEF'',convert(binary(32),''0x00F7FF0300000000000000000000000000000000000000000000000000000000'',1),convert(binary(32),''0x0000000000000000000000000000000000000000000000000000000000000000'',1),''Standard Full Access'',1)'
		exec (@sql)
	END
END
ELSE
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [config].[SystemPolicy] WHERE [TYPE] = 'PIIADMIN')
	set @sql =
	'INSERT into [config].[SystemPolicy]
	([Type],AllowMask,DenyMask,[Name])
	values (''PIIADMIN'',convert(binary(32),''0x0000000400000000000000000000000000000000000000000000000000000000'',1),convert(binary(32),''0x0000000000000000000000000000000000000000000000000000000000000000'',1),''PII ADMINISTRATOR ONLY'')'
		exec (@sql)
	BEGIN

		IF NOT EXISTS (SELECT 1 FROM [config].[SystemPolicy] WHERE [TYPE] = 'GLOBALSYSDEF')
		set @sql =
		'INSERT into [config].[SystemPolicy]
		([ID],[Type],AllowMask,DenyMask,[Name])
		values (''3CC3BD00-7D7E-4D4A-96C6-44E44E140C5E'',''GLOBALSYSDEF'',convert(binary(32),''0x00F7FF0300000000000000000000000000000000000000000000000000000000'',1),convert(binary(32),''0x0000000000000000000000000000000000000000000000000000000000000000'',1),''Standard Full Access'')'
		exec (@sql)
	END
END