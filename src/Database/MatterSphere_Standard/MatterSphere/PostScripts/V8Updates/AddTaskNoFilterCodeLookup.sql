IF NOT EXISTS (SELECT 1 FROM dbCodeLookup WHERE cdtype = 'RESOURCE' AND cdcode = 'FMTASKNOFLTRID')
BEGIN
	insert into dbCodelookup (cdType, cdCode, cdDesc)
	values ('RESOURCE', 'FMTASKNOFLTRID', 'One or more tasks does not have an unique filter / id.  Please correct before saving the task flow.')
END