Print 'Starting NewInstallScripts\Default_Enums.sql'


-- Add default Enums
IF NOT EXISTS ( SELECT EnumItem FROM dbo.dbEnums WHERE EnumItem = 'FWBS.OMS.Associate' )
BEGIN
	INSERT dbo.dbEnums ( EnumItem , EnumName )
	VALUES ( 'FWBS.OMS.Associate' , 'ConfigurableTypes' )
END
GO

IF NOT EXISTS ( SELECT EnumItem FROM dbo.dbEnums WHERE EnumItem = 'FWBS.OMS.Client' )
BEGIN
	INSERT dbo.dbEnums ( EnumItem , EnumName )
	VALUES ( 'FWBS.OMS.Client' , 'ConfigurableTypes' )
END
GO

IF NOT EXISTS ( SELECT EnumItem FROM dbo.dbEnums WHERE EnumItem = 'FWBS.OMS.Contact' )
BEGIN
	INSERT dbo.dbEnums ( EnumItem , EnumName )
	VALUES ( 'FWBS.OMS.Contact' , 'ConfigurableTypes' )
END
GO

IF NOT EXISTS ( SELECT EnumItem FROM dbo.dbEnums WHERE EnumItem = 'FWBS.OMS.DocType' )
BEGIN
	INSERT dbo.dbEnums ( EnumItem , EnumName )
	VALUES ( 'FWBS.OMS.DocType' , 'ConfigurableTypes' )
END
GO

IF NOT EXISTS ( SELECT EnumItem FROM dbo.dbEnums WHERE EnumItem = 'FWBS.OMS.FeeEarner' )
BEGIN
	INSERT dbo.dbEnums ( EnumItem , EnumName )
	VALUES ( 'FWBS.OMS.FeeEarner' , 'ConfigurableTypes' )
END
GO

IF NOT EXISTS ( SELECT EnumItem FROM dbo.dbEnums WHERE EnumItem = 'FWBS.OMS.OmsFile' )
BEGIN
	INSERT dbo.dbEnums ( EnumItem , EnumName )
	VALUES ( 'FWBS.OMS.OmsFile' , 'ConfigurableTypes' )
END
GO

IF NOT EXISTS ( SELECT EnumItem FROM dbo.dbEnums WHERE EnumItem = 'FWBS.OMS.User' )
BEGIN
	INSERT dbo.dbEnums ( EnumItem , EnumName )
	VALUES ( 'FWBS.OMS.User' , 'ConfigurableTypes' )
END
GO


