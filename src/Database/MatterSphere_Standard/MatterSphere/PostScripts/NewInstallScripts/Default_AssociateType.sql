Print 'Starting NewInstallScripts\Default_AssociateType.sql'

-- Add default Associate types
IF NOT EXISTS ( SELECT typeCode FROM dbo.dbAssociateType WHERE typeCode = 'CLIENT' )
BEGIN
	INSERT dbo.dbAssociateType ( typeCode, typeVersion, typeXML, typeGlyph, typeActive )
	VALUES ( 'CLIENT', 1, '<Config><Dialog><Form lookup="ASSOCCAPTION" /><Tabs><Tab lookup="ASSOCDETAILS" glyph="23" source="SCRASSEDIT" tabtype="Enquiry" /></Tabs><Panels /></Dialog><Settings /><defaultTemplates /></Config>', -1, 1 )
END
GO	


IF NOT EXISTS ( SELECT typeCode FROM dbo.dbAssociateType WHERE typeCode = 'TEMPLATE' )
BEGIN
	INSERT dbo.dbAssociateType ( typeCode, typeVersion, typeXML, typeGlyph, typeActive )
	VALUES ( 'TEMPLATE', 2, '<Config><Dialog><Form lookup="ASSOCCAPTION" /><Tabs><Tab lookup="ASSOCDETAILS" glyph="23" source="SCRASSEDIT" tabtype="Enquiry" /></Tabs><Panels /></Dialog><Settings /><defaultTemplates /><ExtendedDataList /></Config>', -1, 1 )
END
GO	

IF NOT EXISTS ( SELECT typeCode FROM dbo.dbAssociateType WHERE typeCode = 'SOURCE' )
BEGIN
	INSERT dbo.dbAssociateType ( typeCode, typeVersion, typeXML, typeGlyph, typeActive )
	VALUES ( 'SOURCE', 1, '<Config><Dialog><Form lookup="ASSOCCAPTION" /><Tabs><Tab lookup="ASSOCDETAILS" glyph="23" source="SCRASSEDIT" tabtype="Enquiry" /></Tabs><Panels /></Dialog><Settings /><defaultTemplates /></Config>', -1, 1 )
END
GO	