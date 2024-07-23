IF OBJECT_ID('dbo.fwbsGrantToOMSAdminRole') IS NOT NULL
BEGIN
 DROP PROCEDURE dbo.fwbsGrantToOMSAdminRole;
 EXEC('CREATE PROCEDURE dbo.fwbsGrantToOMSAdminRole AS EXECUTE dbo.fwbsGrantToRole ''OMSAdminRole'';')
END
GO

IF OBJECT_ID('dbo.fwbsGrantToOMSApplicationRole') IS NOT NULL
BEGIN
 DROP PROCEDURE dbo.fwbsGrantToOMSApplicationRole;
 EXEC('CREATE PROCEDURE dbo.fwbsGrantToOMSApplicationRole AS EXECUTE dbo.fwbsGrantToRole ''OMSApplicationRole'';')
END
GO

IF OBJECT_ID('dbo.fwbsGrantToOMSRole') IS NOT NULL
BEGIN
 DROP PROCEDURE dbo.fwbsGrantToOMSRole;
 EXEC('CREATE PROCEDURE dbo.fwbsGrantToOMSRole AS EXECUTE dbo.fwbsGrantToRole ''OMSRole'';')
END
GO