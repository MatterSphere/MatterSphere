Print 'NewInstallScripts\Default_FeeEarnerType.sql'


-- Add default FeeEarner type
IF NOT EXISTS ( SELECT typeCode FROM dbo.dbFeeEarnerType WHERE typeCode = 'STANDARD' )
BEGIN
	INSERT dbo.dbFeeEarnerType ( typeCode , typeVersion , typeXML , typeGlyph , typeActive )
	VALUES ( 'STANDARD' , 37 , '<Config><Dialog><Form lookup="FEECAPTION" /><Tabs><Tab lookup="2123444054" source="SCRFEEMAINUSER" tabtype="Enquiry" glyph="52" /><Tab lookup="-203692210" source="SCRFEEMAININFO" tabtype="Enquiry" glyph="54" conditional="" /><Tab lookup="-410578691" source="SCRFEEMAINTIME" tabtype="Enquiry" conditional="IsPackageInstalled(&quot;TIMERECORDING&quot;)" glyph="62" /><Tab lookup="820941674" source="SCRFEEMAINLAUK" tabtype="Enquiry" glyph="43" conditional="Ispackageinstalled(&quot;LegalAid&quot;)" /></Tabs><Panels width="200" /></Dialog><ExtendedDataList /><Settings /><defaultTemplates /></Config>' , 5 , 1 )
END
GO


