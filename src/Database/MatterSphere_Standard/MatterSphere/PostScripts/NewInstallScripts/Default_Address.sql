Print 'Starting NewInstallScripts\Default_Address.sql'


SET IDENTITY_INSERT dbo.dbAddress ON

-- Add default address record
IF NOT EXISTS ( SELECT addID FROM dbo.dbAddress WHERE addID = 1)
BEGIN
	INSERT dbo.dbAddress (addid, addline1, addCountry)
	VALUES (1, 'Default Address', 223)
END
GO

-- Add FWBS address record
IF NOT EXISTS ( SELECT addID FROM dbo.dbAddress WHERE addID = -100)
BEGIN
	INSERT dbo.dbAddress (addid, addLine1, addLine2, addLine3, addLine4, addPostCode, addCountry )
	VALUES (-100, 'The Old Brewery', 'Towcester Road', 'Milton Malsor', 'Northamptonshire', 'NN7 3AP', 223)
END
GO

SET IDENTITY_INSERT dbo.dbAddress OFF
GO