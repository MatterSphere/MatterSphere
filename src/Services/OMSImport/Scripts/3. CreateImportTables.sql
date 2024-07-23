-----------------------------------------------------------------
-- Run on newly created OMSImport Database
-----------------------------------------------------------------
USE [OMSImport]

CREATE TABLE [dbo].[ClientDetails] (
	[extContID] [nvarchar] (20) NOT NULL ,
	[clNo] [nvarchar] (12) NOT NULL ,
	[addLine1] [nvarchar] (64) NULL ,
	[addLine2] [nvarchar] (64) NULL ,
	[addLine3] [nvarchar] (64) NULL ,
	[addLine4] [nvarchar] (64) NULL ,
	[addLine5] [nvarchar] (64) NULL ,
	[addPostCode] [nvarchar] (20) NULL ,
	[addCountry] [nvarchar] (50) NULL ,
	[addDXCode] [nvarchar] (80) NULL ,
	[contType] [nvarchar] (12) NOT NULL ,
	[contSalut] [nvarchar] (50) NULL ,
	[contTitle] [nvarchar] (10) NULL ,
	[contFirstNames] [nvarchar] (50) NULL ,
	[contSurname] [nvarchar] (50) NOT NULL ,
	[contSex] [nchar] (1) NULL ,
	[contNotes] [nvarchar] (4000) NULL ,
	[contCreated] [datetime] NULL ,
	[contEmail] [nvarchar] (200) NULL ,
	[contTelHome] [nvarchar] (30) NULL ,
	[contTelWork] [nvarchar] (30) NULL ,
	[contTelMob] [nvarchar] (30) NULL ,
	[contFAX] [nvarchar] (30) NULL ,
	[clName] [nvarchar] (128) NOT NULL ,
	[clType] [nvarchar] (12) NULL ,
	[Imported] [smalldatetime] NULL ,
	[OMSaddID] [bigint] NULL ,
	[DCFlag] [tinyint] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[CodeLookup] (
	[cdType] [nvarchar] (15) NOT NULL ,
	[cdCode] [nvarchar] (15) NOT NULL ,
	[cdDesc] [nvarchar] (100) NOT NULL ,
	[Imported] [smalldatetime] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Defaults] (
	[defType] [nvarchar] (50) NOT NULL ,
	[defCode] [nvarchar] (12) NOT NULL ,
	[defDesc] [nvarchar] (100) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Documents] (
	[docID] [bigint] NOT NULL ,
	[clNo] [nvarchar] (12) NOT NULL ,
	[fileNo] [nvarchar] (20) NOT NULL ,
	[docDesc] [nvarchar] (150) NOT NULL ,
	[docWallet] [nvarchar] (15) NULL ,
	[docFileName] [nvarchar] (255) NOT NULL ,
	[docDirection] [bit] NOT NULL ,
	[docExtension] [nvarchar] (15) NOT NULL ,
	[CreatedBy] [int] NULL ,
	[Created] [datetime] NOT NULL ,
	[docDir] [nvarchar] (200) NOT NULL ,
	[docType] [nvarchar] (15) NULL ,
	[DCFlag] [tinyint] NULL ,
	[OMSfileID] [bigint] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[FeeEarner] (
	[usrID] [int] NOT NULL ,
	[Initials] [nvarchar] (30) NOT NULL ,
	[Alias] [nvarchar] (30) NOT NULL ,
	[FullName] [nvarchar] (50) NOT NULL ,
	[NetworkName] [nvarchar] (50) NULL ,
	[usrType] [nchar] (1) NOT NULL ,
	[Imported] [smalldatetime] NULL ,
	[DCFlag] [tinyint] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[FileDetails] (
	[clNo] [nvarchar] (12) NOT NULL ,
	[fileNo] [nvarchar] (20) NOT NULL ,
	[extFileID] [bigint] NULL ,
	[fileDesc] [nvarchar] (255) NOT NULL ,
	[fileResponsibleID] [int] NULL ,
	[filePrincipleID] [int] NULL ,
	[fileDept] [nvarchar] (15) NULL ,
	[fileType] [nvarchar] (15) NULL ,
	[fileFundCode] [nvarchar] (15) NULL ,
	[fileCurISOCode] [nchar] (3) NULL ,
	[fileStatus] [nvarchar] (15) NOT NULL ,
	[fileCreated] [datetime] NULL ,
	[fileUpdated] [datetime] NULL ,
	[fileClosed] [datetime] NULL ,
	[fileSource] [nvarchar] (15) NULL ,
	[Imported] [smalldatetime] NULL ,
	[DCFlag] [tinyint] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ImportLog] (
	[LogID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[LogInserts] [bigint] NULL ,
	[LogUpdates] [bigint] NULL ,
	[LogDeletes] [bigint] NULL ,
	[LogWarnings] [bigint] NULL ,
	[LogImportDesc] [nvarchar] (100) NOT NULL ,
	[LogCreated] [smalldatetime] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ClientDetails] ADD 
	CONSTRAINT [PK_ClientDetails] PRIMARY KEY  CLUSTERED 
	(
		[extContID],
		[clNo]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[CodeLookup] ADD 
	CONSTRAINT [PK_CodeLookup] PRIMARY KEY  CLUSTERED 
	(
		[cdType],
		[cdCode]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Defaults] ADD 
	CONSTRAINT [PK_ImportDefaults] PRIMARY KEY  CLUSTERED 
	(
		[defType]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Documents] ADD 
	CONSTRAINT [PK_Documents] PRIMARY KEY  CLUSTERED 
	(
		[docID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[FeeEarner] ADD 
	CONSTRAINT [PK_FeeEarner] PRIMARY KEY  CLUSTERED 
	(
		[usrID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ImportLog] ADD 
	CONSTRAINT [PK_ImportLog] PRIMARY KEY  CLUSTERED 
	(
		[LogID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ClientDetails] ADD 
	CONSTRAINT [CK_ClientDetails_contSex] CHECK  ([contsex] = 'M' or [contSex] = 'F')
GO

ALTER TABLE [dbo].[Documents] ADD 
	CONSTRAINT [DF_Documents_Created] DEFAULT (getdate()) FOR [Created]
GO

ALTER TABLE [dbo].[FeeEarner] ADD 
	CONSTRAINT [IX_FeeEarner] UNIQUE  NONCLUSTERED 
	(
		[Alias]
	)  ON [PRIMARY] ,
	CONSTRAINT [CK_FeeEarner_usrType] CHECK  ([usrType] = 'U' or [usrType] = 'F')
GO

ALTER TABLE [dbo].[FileDetails] ADD 
	CONSTRAINT [PK_FileDetails] PRIMARY KEY  CLUSTERED 
	(
		[clNo],
		[fileNo]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ImportLog] ADD 
	CONSTRAINT [DF_ImportLog_LogCreated] DEFAULT (getdate()) FOR [LogCreated]
GO


exec sp_addextendedproperty N'MS_Description', N'Contact''s country', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'addCountry'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s address DXcode', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'addDXCode'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s address line 1', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'addLine1'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s address line 2', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'addLine2'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s address line 3', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'addLine3'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s address line 4', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'addLine4'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s address line 5', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'addLine5'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s address postcode', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'addPostCode'
GO
exec sp_addextendedproperty N'MS_Description', N'Client''s name', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'clName'
GO
exec sp_addextendedproperty N'MS_Description', N'External client number', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'clNo'
GO
exec sp_addextendedproperty N'MS_Description', N'Client type (code lookup)', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'clType'
GO
exec sp_addextendedproperty N'MS_Description', N'Date contact record was created', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'contCreated'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s email address', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'contEmail'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s FAX number', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'contFAX'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s first name', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'contFirstNames'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s notes', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'contNotes'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s salutation', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'contSalut'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s gender. Must be ''M'' , ''F'' or Null', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'contSex'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s surname or the organization name of a corporate contact', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'contSurname'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s home telephone number', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'contTelHome'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s mobile telephone number', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'contTelMob'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s work telephone number', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'contTelWork'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact''s title', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'contTitle'
GO
exec sp_addextendedproperty N'MS_Description', N'Contact type (code lookup)', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'contType'
GO
exec sp_addextendedproperty N'MS_Description', N'Do not populate', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'DCFlag'
GO
exec sp_addextendedproperty N'MS_Description', N'External contactID', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'extContID'
GO
exec sp_addextendedproperty N'MS_Description', N'Do not populate', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'Imported'
GO
exec sp_addextendedproperty N'MS_Description', N'Do not populate', N'user', N'dbo', N'table', N'ClientDetails', N'column', N'OMSaddID'


GO


exec sp_addextendedproperty N'MS_Description', N'Code lookup code', N'user', N'dbo', N'table', N'CodeLookup', N'column', N'cdCode'
GO
exec sp_addextendedproperty N'MS_Description', N'Code lookup description', N'user', N'dbo', N'table', N'CodeLookup', N'column', N'cdDesc'
GO
exec sp_addextendedproperty N'MS_Description', N'Code lookup type', N'user', N'dbo', N'table', N'CodeLookup', N'column', N'cdType'
GO
exec sp_addextendedproperty N'MS_Description', N'Do Not Populate', N'user', N'dbo', N'table', N'CodeLookup', N'column', N'Imported'


GO


exec sp_addextendedproperty N'MS_Description', N'Defalut code (code lookup)', N'user', N'dbo', N'table', N'Defaults', N'column', N'defCode'
GO
exec sp_addextendedproperty N'MS_Description', N'Default defcode description', N'user', N'dbo', N'table', N'Defaults', N'column', N'defDesc'
GO
exec sp_addextendedproperty N'MS_Description', N'Default type (code lookup)', N'user', N'dbo', N'table', N'Defaults', N'column', N'defType'


GO


exec sp_addextendedproperty N'MS_Description', N'External client number', N'user', N'dbo', N'table', N'Documents', N'column', N'clNo'
GO
exec sp_addextendedproperty N'MS_Description', N'Document created. Defaults to row imported data when no value is supplied', N'user', N'dbo', N'table', N'Documents', N'column', N'Created'
GO
exec sp_addextendedproperty N'MS_Description', N'Document created by ID. Defaults to the default setting on  Import if NULL', N'user', N'dbo', N'table', N'Documents', N'column', N'CreatedBy'
GO
exec sp_addextendedproperty N'MS_Description', N'Do not populate', N'user', N'dbo', N'table', N'Documents', N'column', N'DCFlag'
GO
exec sp_addextendedproperty N'MS_Description', N'Document description', N'user', N'dbo', N'table', N'Documents', N'column', N'docDesc'
GO
exec sp_addextendedproperty N'MS_Description', N'Document storage directory.', N'user', N'dbo', N'table', N'Documents', N'column', N'docDir'
GO
exec sp_addextendedproperty N'MS_Description', N'Document direction in = 0, out = 1', N'user', N'dbo', N'table', N'Documents', N'column', N'docDirection'
GO
exec sp_addextendedproperty N'MS_Description', N'Document extension. i.e .msg, .doc', N'user', N'dbo', N'table', N'Documents', N'column', N'docExtension'
GO
exec sp_addextendedproperty N'MS_Description', N'Document file name', N'user', N'dbo', N'table', N'Documents', N'column', N'docFileName'
GO
exec sp_addextendedproperty N'MS_Description', N'External document ID', N'user', N'dbo', N'table', N'Documents', N'column', N'docID'
GO
exec sp_addextendedproperty N'MS_Description', N'Document Type (code lookup)', N'user', N'dbo', N'table', N'Documents', N'column', N'docType'
GO
exec sp_addextendedproperty N'MS_Description', N'Document wallet. Defaults to the default setting on  Import if NULL', N'user', N'dbo', N'table', N'Documents', N'column', N'docWallet'
GO
exec sp_addextendedproperty N'MS_Description', N'External file number', N'user', N'dbo', N'table', N'Documents', N'column', N'fileNo'
GO
exec sp_addextendedproperty N'MS_Description', N'Do not populate', N'user', N'dbo', N'table', N'Documents', N'column', N'OMSfileID'


GO


exec sp_addextendedproperty N'MS_Description', N'User''s alias', N'user', N'dbo', N'table', N'FeeEarner', N'column', N'Alias'
GO
exec sp_addextendedproperty N'MS_Description', N'Do Not Populate', N'user', N'dbo', N'table', N'FeeEarner', N'column', N'DCFlag'
GO
exec sp_addextendedproperty N'MS_Description', N'User''s full name', N'user', N'dbo', N'table', N'FeeEarner', N'column', N'FullName'
GO
exec sp_addextendedproperty N'MS_Description', N'Do Not Populate', N'user', N'dbo', N'table', N'FeeEarner', N'column', N'Imported'
GO
exec sp_addextendedproperty N'MS_Description', N'User''s initials', N'user', N'dbo', N'table', N'FeeEarner', N'column', N'Initials'
GO
exec sp_addextendedproperty N'MS_Description', N'User''s individual Windows account', N'user', N'dbo', N'table', N'FeeEarner', N'column', N'NetworkName'
GO
exec sp_addextendedproperty N'MS_Description', N'External user ID', N'user', N'dbo', N'table', N'FeeEarner', N'column', N'usrID'
GO
exec sp_addextendedproperty N'MS_Description', N'User type. Must be ''U'' for User or ''F'' for Fee Earner', N'user', N'dbo', N'table', N'FeeEarner', N'column', N'usrType'


GO


exec sp_addextendedproperty N'MS_Description', N'External client number', N'user', N'dbo', N'table', N'FileDetails', N'column', N'clNo'
GO
exec sp_addextendedproperty N'MS_Description', N'Do Not Populate', N'user', N'dbo', N'table', N'FileDetails', N'column', N'DCFlag'
GO
exec sp_addextendedproperty N'MS_Description', N'External file ID (numeric)', N'user', N'dbo', N'table', N'FileDetails', N'column', N'extFileID'
GO
exec sp_addextendedproperty N'MS_Description', N'File closed date and time', N'user', N'dbo', N'table', N'FileDetails', N'column', N'fileClosed'
GO
exec sp_addextendedproperty N'MS_Description', N'File created date. Defaults to the date and time of import if no value is supplied', N'user', N'dbo', N'table', N'FileDetails', N'column', N'fileCreated'
GO
exec sp_addextendedproperty N'MS_Description', N'File currency code (code lookup)', N'user', N'dbo', N'table', N'FileDetails', N'column', N'fileCurISOCode'
GO
exec sp_addextendedproperty N'MS_Description', N'File department (code lookup)', N'user', N'dbo', N'table', N'FileDetails', N'column', N'fileDept'
GO
exec sp_addextendedproperty N'MS_Description', N'File description', N'user', N'dbo', N'table', N'FileDetails', N'column', N'fileDesc'
GO
exec sp_addextendedproperty N'MS_Description', N'File funding code (code lookup)', N'user', N'dbo', N'table', N'FileDetails', N'column', N'fileFundCode'
GO
exec sp_addextendedproperty N'MS_Description', N'External file number', N'user', N'dbo', N'table', N'FileDetails', N'column', N'fileNo'
GO
exec sp_addextendedproperty N'MS_Description', N'File manager', N'user', N'dbo', N'table', N'FileDetails', N'column', N'filePrincipleID'
GO
exec sp_addextendedproperty N'MS_Description', N'File owner', N'user', N'dbo', N'table', N'FileDetails', N'column', N'fileResponsibleID'
GO
exec sp_addextendedproperty N'MS_Description', N'File source (code lookup)', N'user', N'dbo', N'table', N'FileDetails', N'column', N'fileSource'
GO
exec sp_addextendedproperty N'MS_Description', N'File status (code lookup)', N'user', N'dbo', N'table', N'FileDetails', N'column', N'fileStatus'
GO
exec sp_addextendedproperty N'MS_Description', N'File type (code lookup)', N'user', N'dbo', N'table', N'FileDetails', N'column', N'fileType'
GO
exec sp_addextendedproperty N'MS_Description', N'File updated date and time.', N'user', N'dbo', N'table', N'FileDetails', N'column', N'fileUpdated'
GO
exec sp_addextendedproperty N'MS_Description', N'Do Not Populate', N'user', N'dbo', N'table', N'FileDetails', N'column', N'Imported'


GO


exec sp_addextendedproperty N'MS_Description', N'Date log record was created', N'user', N'dbo', N'table', N'ImportLog', N'column', N'LogCreated'
GO
exec sp_addextendedproperty N'MS_Description', N'Log deletes', N'user', N'dbo', N'table', N'ImportLog', N'column', N'LogDeletes'
GO
exec sp_addextendedproperty N'MS_Description', N'Log ID', N'user', N'dbo', N'table', N'ImportLog', N'column', N'LogID'
GO
exec sp_addextendedproperty N'MS_Description', N'log message', N'user', N'dbo', N'table', N'ImportLog', N'column', N'LogImportDesc'
GO
exec sp_addextendedproperty N'MS_Description', N'Log inserts', N'user', N'dbo', N'table', N'ImportLog', N'column', N'LogInserts'
GO
exec sp_addextendedproperty N'MS_Description', N'Log updates', N'user', N'dbo', N'table', N'ImportLog', N'column', N'LogUpdates'
GO
exec sp_addextendedproperty N'MS_Description', N'Log warnings', N'user', N'dbo', N'table', N'ImportLog', N'column', N'LogWarnings'


GO

