Print 'NewInstallScripts\Default_UserType.sql'


-- Add default User types
IF NOT EXISTS ( SELECT typeCode FROM dbo.dbUserType WHERE typeCode = 'ADMIN' )
BEGIN
	INSERT dbo.dbUserType ( typeCode, typeVersion, typeXML, typeGlyph, typeActive )
	VALUES ( 'ADMIN', 6, '<Config><Dialog><Form /><Tabs><Tab lookup="2123444054" glyph="23" source="SCRUSRMAIN" tabtype="Enquiry" /><Tab lookup="230312994" glyph="59" source="SCRUSRROLES" tabtype="Enquiry" /><Tab lookup="1278337421" source="SCRUSRPREF" tabtype="Enquiry" glyph="19" /></Tabs><Panels /></Dialog><ExtendedDataList /><Settings /><defaultTemplates /></Config>', 5, 1 )
END
GO

IF NOT EXISTS ( SELECT typeCode FROM dbo.dbUserType WHERE typeCode = 'SERVICE' )
BEGIN
	INSERT dbo.dbUserType ( typeCode, typeVersion, typeXML, typeGlyph, typeActive )
	VALUES ( 'SERVICE', 2, '<Config><Dialog><Form /><Tabs><Tab lookup="2123444054" glyph="23" source="ENQUSER" tabtype="Enquiry" /><Tab lookup="230312994" glyph="59" source="ENQROLES" tabtype="Enquiry" /></Tabs><Panels /></Dialog><ExtendedDataList /><Settings /></Config>', 5, 0 )
END
GO

IF NOT EXISTS ( SELECT typeCode FROM dbo.dbUserType WHERE typeCode = 'STANDARD' )
BEGIN
	INSERT dbo.dbUserType ( typeCode, typeVersion, typeXML, typeGlyph, typeActive )
	VALUES ( 'STANDARD', 15, '<Config><Dialog><Form /><Tabs><Tab lookup="2123444054" glyph="23" source="SCRUSRMAIN" tabtype="Enquiry" /><Tab lookup="1278337421" source="SCRUSRPREF" tabtype="Enquiry" glyph="19" /><Tab lookup="230312994" source="SCRUSRROLES" tabtype="Enquiry" glyph="59" /></Tabs><Panels /></Dialog><ExtendedDataList></ExtendedDataList><Settings /><defaultTemplates /></Config>' , 5, 1 )
END
GO

IF NOT EXISTS ( SELECT typeCode FROM dbo.dbUserType WHERE typeCode = 'SYSTEM' )
BEGIN
	INSERT dbo.dbUserType ( typeCode, typeVersion, typeXML, typeGlyph, typeActive )
	VALUES ( 'SYSTEM', 9, '<Config><Dialog><Form /><Tabs><Tab lookup="2123444054" glyph="23" source="ENQUSER" tabtype="Enquiry" /><Tab lookup="230312994" glyph="59" source="ENQROLES" tabtype="Enquiry" /></Tabs><Panels /></Dialog><ExtendedDataList /><Settings /><defaultTemplates /></Config>', 5, 0 )
END
GO

IF NOT EXISTS ( SELECT typeCode FROM dbo.dbUserType WHERE typeCode = 'REMOTE' )
BEGIN
	INSERT dbo.dbUserType ( typeCode, typeVersion, typeXML, typeGlyph, typeActive )
	VALUES ( 'REMOTE', 15, '<Config><Dialog><Form /><Tabs><Tab lookup="2123444054" glyph="23" source="SCRUSRMAIN" tabtype="Enquiry" /></Tabs><Panels /></Dialog><ExtendedDataList></ExtendedDataList><Settings /><defaultTemplates /></Config>' , 5, 1 )
END
GO

