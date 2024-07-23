Print 'NewInstallScripts\Default_Seeds.sql'

-- Add default seed records
IF NOT EXISTS ( SELECT brID FROM dbSeed WHERE brID = 1 AND seedType = 'CL')
BEGIN
	INSERT dbo.dbSeed (brid, seedtype, seedauto, seedprefix, seedlastused)
	VALUES (1, 'CL', 1, 'F', 0)
END
GO

IF NOT EXISTS ( SELECT brID FROM dbSeed WHERE brID = 1 AND seedType = 'BILL')
BEGIN
	INSERT dbo.dbSeed (brid, seedtype, seedauto, seedprefix, seedsuffix, seedlastused)
	VALUES (1, 'BILL', 1, 'INV', 'A', 0)
END
GO

IF NOT EXISTS ( SELECT brID FROM dbSeed WHERE brID = 1 AND seedType = 'FI')
BEGIN
	INSERT dbo.dbSeed (brid, seedtype, seedauto, seedlastused)
	VALUES (1, 'FI', 1, 0)
END
GO



