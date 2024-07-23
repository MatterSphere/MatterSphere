IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'dbFileManagementApplication' AND COLUMN_NAME = 'appActive')
BEGIN
	ALTER TABLE [dbo].[dbFileManagementApplication] ADD [appActive] BIT NOT NULL
	CONSTRAINT [DF_dbFileManagementApplication_appActive] DEFAULT (1)
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'dbFileManagementApplication' AND COLUMN_NAME = 'Created')
BEGIN
	ALTER TABLE [dbo].[dbFileManagementApplication] ADD [Created] [dbo].[uCreated] NULL
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'dbFileManagementApplication' AND COLUMN_NAME = 'CreatedBy')
BEGIN
	ALTER TABLE [dbo].[dbFileManagementApplication] ADD [CreatedBy] [dbo].[uCreatedBy] NULL
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'dbFileManagementApplication' AND COLUMN_NAME = 'Updated')
BEGIN
	ALTER TABLE [dbo].[dbFileManagementApplication] ADD [Updated] [dbo].[uCreated] NULL
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'dbFileManagementApplication' AND COLUMN_NAME = 'UpdatedBy')
BEGIN
	ALTER TABLE [dbo].[dbFileManagementApplication] ADD [UpdatedBy] [dbo].[uCreatedBy] NULL
END
GO
