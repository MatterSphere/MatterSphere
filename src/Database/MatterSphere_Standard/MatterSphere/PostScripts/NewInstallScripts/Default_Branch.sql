
Print 'Starting NewInstallScripts\Default_Branch.sql'

-- Add default branch record
IF NOT EXISTS ( SELECT brID FROM dbo.dbBranch WHERE brID = 1 )
BEGIN
	SET IDENTITY_INSERT dbo.dbBranch ON
	INSERT dbo.dbBranch (brid, brcode, brname, braddid, brUICultureInfo, brDefCountry, brCurISOCode, brIntDefaultSecurity)
	VALUES (1, 1, 'Default Branch', 1, 'en-gb', 223, 'GBP', 10010)
	SET IDENTITY_INSERT dbo.dbBranch OFF
END
GO



