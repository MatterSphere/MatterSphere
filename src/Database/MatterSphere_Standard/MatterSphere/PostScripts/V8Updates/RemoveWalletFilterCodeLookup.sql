IF EXISTS (SELECT 1 FROM dbCodeLookup WHERE cdtype = 'DOCGROUPING' AND cdcode = 'DOCWALLET')
BEGIN
	DELETE from dbCodeLookup where cdtype = 'DOCGROUPING' and cdcode = 'DOCWALLET'
END