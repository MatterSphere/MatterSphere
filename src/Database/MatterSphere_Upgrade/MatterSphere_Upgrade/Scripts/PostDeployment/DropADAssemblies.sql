IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[dbo].[ADSyncAllUsers]') AND type = 'PC')
	DROP PROCEDURE [dbo].[ADSyncAllUsers]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[dbo].[ADGroupUsers]') AND type = 'FT')
	DROP FUNCTION [dbo].[ADGroupUsers]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[dbo].[ADSyncGroup]') AND type = 'PC')
	DROP PROCEDURE [dbo].[ADSyncGroup]
GO

IF  EXISTS (SELECT * FROM sys.assemblies asms WHERE asms.name = N'ADGroupSync' and is_user_defined = 1)
	DROP ASSEMBLY [ADGroupSync]
GO

IF  EXISTS (SELECT * FROM sys.assemblies WHERE name = N'ADUserSync' AND is_user_defined = 1)
	DROP ASSEMBLY [ADUserSync]
GO

IF  EXISTS (SELECT * FROM sys.assemblies WHERE name = N'DirectoryServices' AND is_user_defined = 1)
	DROP ASSEMBLY [DirectoryServices]
GO
