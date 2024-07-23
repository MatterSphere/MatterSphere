Print 'Starting UserType_Remote'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT typeCode FROM dbo.dbUserType WHERE typeCode = 'CLIENT' )
BEGIN
	INSERT dbo.dbUserType ( typeCode, typeVersion, typeXML, typeGlyph, typeActive )
	VALUES ( 'CLIENT', 18, '<Config><Dialog><Form /><Tabs><Tab lookup="2123444054" glyph="23" source="SCRUSRCLIENT" tabtype="Enquiry" /></Tabs><Panels /></Dialog><ExtendedDataList><ExtendedData code="EXTUSERCLI" /></ExtendedDataList><Settings /><defaultTemplates /></Config>' , 5, 1 )
END

IF EXISTS ( SELECT typeCode FROM dbo.dbUserType WHERE typeCode = 'CLIENT' AND typeVersion = 14 )
BEGIN
	UPDATE dbo.dbUserType SET  typeXML = '<Config><Dialog><Form /><Tabs><Tab lookup="2123444054" glyph="23" source="SCRUSRCLIENT" tabtype="Enquiry" /></Tabs><Panels /></Dialog><ExtendedDataList><ExtendedData code="EXTUSERCLI" /></ExtendedDataList><Settings /><defaultTemplates /></Config>'
			, typeVersion = 18 WHERE typeCode = 'CLIENT' AND typeVersion = 14
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO