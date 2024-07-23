Print 'NewInstallScripts\Default_Printer.sql'

-- Add default Printer
IF NOT EXISTS ( SELECT printID FROM dbo.dbPrinter WHERE printID = 1 )
BEGIN
	SET IDENTITY_INSERT dbo.dbPrinter ON
	INSERT dbo.dbPrinter (printID, printName)
	VALUES (1, 'Default Printer')
	SET IDENTITY_INSERT dbo.dbPrinter OFF
END
GO



