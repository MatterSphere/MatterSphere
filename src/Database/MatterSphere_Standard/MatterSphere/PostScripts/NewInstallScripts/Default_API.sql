Print 'Starting NewInstallScripts\Default_API.sql'


-- Add default API records
IF NOT EXISTS ( SELECT apiGuid FROM dbo.dbAPI WHERE apiGuid = '31ED0BE4-3B00-469F-B50B-7CF27154AF9C' )
BEGIN
	INSERT dbo.dbAPI (apiGUID, apiCode, apiDesc, apiAuthor, apiUIType, apiDesigner, apiRegistered, apiService)
	VALUES ('31ED0BE4-3B00-469F-B50B-7CF27154AF9C', 'omsadmin', 'Admin Kit', 'FWBS Ltd', 0, 1, 1, 0)
END
GO


IF NOT EXISTS ( SELECT apiGuid FROM dbo.dbAPI WHERE apiGuid = 'BD82970C-6AA8-4834-8482-2B0DB119ED5C' )
BEGIN
	INSERT dbo.dbAPI (apiGUID, apiCode, apiDesc, apiAuthor, apiUIType, apiDesigner, apiRegistered, apiService)
	VALUES ('BD82970C-6AA8-4834-8482-2B0DB119ED5C',	'OMSOffice',	'OMS Integration for Office XP', 'FWBS Ltd', 0, 0, 1, 0)
END
GO


IF NOT EXISTS ( SELECT apiGuid FROM dbo.dbAPI WHERE apiGuid = '4E6F49D1-8A34-4B96-ADC7-EF9EE37498C5' )
BEGIN
	INSERT dbo.dbAPI (apiGUID, apiCode, apiDesc, apiAuthor, apiUIType, apiDesigner, apiRegistered, apiService)	
	VALUES ('4E6F49D1-8A34-4B96-ADC7-EF9EE37498C5', 'FWBS.OMS.FileManagement', 'File Management Addin', 'FWBS Ltd', 1, 0, 1, 0)
END
GO	


IF NOT EXISTS ( SELECT apiGuid FROM dbo.dbAPI WHERE apiGuid = 'DB81D80C-5DF5-4EB6-A3F1-F9A90066F236' )
BEGIN
	INSERT dbo.dbAPI (apiGUID, apiCode, apiDesc, apiAuthor, apiUIType, apiDesigner, apiRegistered, apiService)
	VALUES ('DB81D80C-5DF5-4EB6-A3F1-F9A90066F236', 'FWBSStub', 'FWBS Stub', 'FWBS Ltd', 1, 0, 1, 0)
END
GO

IF NOT EXISTS ( SELECT apiGuid FROM dbo.dbAPI WHERE apiGuid = 'A89756EA-08D3-45B5-BDEA-8D59E5225C7C' )
BEGIN
	INSERT dbo.dbAPI (apiGUID, apiCode, apiDesc, apiAuthor, apiUIType, apiDesigner, apiRegistered, apiService)
	VALUES ('A89756EA-08D3-45B5-BDEA-8D59E5225C7C', 'FWBS.OMS.WFRuntime', 'FWBS WorkFlow', 'FWBS Ltd', 1, 0, 1, 0)
END
GO		

IF NOT EXISTS ( SELECT apiGuid FROM dbo.dbAPI WHERE apiGuid = 'A3840119-FEB9-4980-96F2-B61E642D4A30' )
BEGIN
INSERT INTO [dbo].[dbAPI]([apiGUID],[apiCode],[apiDesc],[apiAuthor],[apiUIType],[apiDesigner],[apiRegistered],[apiService])
	 VALUES   ('A3840119-FEB9-4980-96F2-B61E642D4A30','omsexternalreports','Reports External','FWBS Ltd',1, 0, 1, 0)
END
GO