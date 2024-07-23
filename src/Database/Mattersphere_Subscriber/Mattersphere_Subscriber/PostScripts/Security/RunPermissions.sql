Print 'Starting Security\RunPermissions.sql'


EXECUTE dbo.fwbsGrantToOMSApplicationRole

EXECUTE dbo.fwbsGrantToOMSRole

EXECUTE dbo.fwbsGrantToOMSAdminRole
GO



