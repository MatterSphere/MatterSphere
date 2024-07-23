UPDATE [dbo].[dbContactType]
SET [typeXML] = REPLACE ([typeXML], '<Tab lookup="868940124" glyph="30" source="SCRCONINDINFO" tabtype="Enquiry" group="NAVGRPCONTINFO" />', '')
WHERE typeCode IN
	('COURT'
	,'EAGENT'
	,'FBROKER'
	,'FINANCIAL'
	,'INSBROKER'
	,'INSCO'
	,'INSCOMASS'
	,'IREV'
	,'LANDREG'
	,'LBROKER'
	,'LEGOMORG'
	,'LOCALAUTH'
	,'LOCAUTAGY'
	,'ORGANISATION'
	,'REMOTE'
	,'SHERIFF'
	,'SOLICITOR'
	,'WATAUTH')
AND typeXML LIKE '%<Tab lookup="868940124" glyph="30" source="SCRCONINDINFO" tabtype="Enquiry" group="NAVGRPCONTINFO" />%'