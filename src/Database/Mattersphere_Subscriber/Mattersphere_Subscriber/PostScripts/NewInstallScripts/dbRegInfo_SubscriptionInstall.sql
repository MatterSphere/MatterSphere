Print 'Starting NewInstallScripts\dbRegInfo_SubscriptionInstall.sql'


-- Add default record for subscription install
IF NOT EXISTS ( SELECT brID FROM dbo.dbRegInfo )
BEGIN
	INSERT dbo.dbRegInfo (regCompanyname, brID, regCompanyID, regEdition, regSerialNo, regAdministrator, regPartnerAddress, regPartnerWebSite, regPartnerSupportTel, regPartnerSupportEmail, regPartnerTel, 
										regPartnerFax, regPartnerCompanyName, regUICultureInfo, regBinSerialCheckSum, regSerialCheckSum)
	VALUES ('Default Company', 99, 1234567, 'EN', -1, -1, -100, 'www.fwbs.net', '+ 44 (0)1604 857857', 'support@fwbs.net', '+ 44 (0)1604 857857', '+ 44 (0)1604 857858', 'FWBS Ltd', 'en-gb', CONVERT(varbinary(MAX),''), '')
END
GO



