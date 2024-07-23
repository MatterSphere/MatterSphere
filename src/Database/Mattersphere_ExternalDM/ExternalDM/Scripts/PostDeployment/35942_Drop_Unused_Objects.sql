IF EXISTS(SELECT 1 FROM sys.procedures WHERE object_id = OBJECT_ID('[dbo].[GetTokenAccess]'))
	DROP PROCEDURE [dbo].[GetTokenAccess]

IF EXISTS(SELECT 1 FROM sys.procedures WHERE object_id = OBJECT_ID('[dbo].[AddUpdateTokenAccess]'))
	DROP PROCEDURE [dbo].[AddUpdateTokenAccess]

IF EXISTS(SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('[dbo].[dbIManageTokenAccess]'))
	DROP TABLE [dbo].[dbIManageTokenAccess]


