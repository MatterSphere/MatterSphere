Print 'Starting NewInstallScripts\Default_AdminMenu.sql'

-- Add default admin menu
IF NOT EXISTS ( SELECT * FROM dbAdminMenu WHERE admnuName = 'ADMIN' AND admnuParent = 1 AND admnuCode = '1924068877' )
BEGIN
	INSERT dbo.dbAdminMenu (admnuName, admnuParent, admnuCode, admnuImageIndex, admnuSearchListCode, admnuorder)
	VALUES ('ADMIN', 1, '1924068877', 27, 'LI', 99)
END
GO



