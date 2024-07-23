IF NOT EXISTS ( SELECT 'OMS' , 'STATES' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'OMS' , 'STATES' , '{default}' , 'States' , 0 , 1 , NULL , NULL , NULL , 1)
END

IF NOT EXISTS ( SELECT 'STATES' , '224' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'STATES' , '224' , '{default}' , 'United States of America' , 0 , 1 , NULL , NULL , NULL , 1)
END

IF NOT EXISTS ( SELECT 'STATES' , '38' , '{default}' , NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	VALUES ( 'STATES' , '38' , '{default}' , 'Canada' , 0 , 1 , NULL , NULL , NULL , 1)
END

IF NOT EXISTS(SELECT 1 FROM dbo.dbCodeLookup WHERE cdType = '224')
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	SELECT 
		'224'
		, cdCode
		, '{default}'
		, cdDesc
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	FROM
	(
	VALUES ('Alabama', 'AL')
		, ('Alaska', 'AK')
		, ('Arizona', 'AZ')
		, ('Arkansas', 'AR')
		, ('California', 'CA')
		, ('Colorado', 'CO')
		, ('Connecticut', 'CT')
		, ('Delaware', 'DE')
		, ('Florida', 'FL')
		, ('Georgia', 'GA')
		, ('Hawaii', 'HI')
		, ('Idaho', 'ID')
		, ('Illinois', 'IL')
		, ('Indiana', 'IN')
		, ('Iowa', 'IA')
		, ('Kansas', 'KS')
		, ('Kentucky', 'KY')
		, ('Louisiana', 'LA')
		, ('Maine', 'ME')
		, ('Maryland', 'MD')
		, ('Massachusetts', 'MA')
		, ('Michigan', 'MI')
		, ('Minnesota', 'MN')
		, ('Mississippi', 'MS')
		, ('Missouri', 'MO')
		, ('Montana', 'MT')
		, ('Nebraska', 'NE')
		, ('Nevada', 'NV')
		, ('New Hampshire', 'NH')
		, ('New Jersey', 'NJ')
		, ('New Mexico', 'NM')
		, ('New York', 'NY')
		, ('North Carolina', 'NC')
		, ('North Dakota', 'ND')
		, ('Ohio', 'OH')
		, ('Oklahoma', 'OK')
		, ('Oregon', 'OR')
		, ('Pennsylvania', 'PA')
		, ('Rhode Island[F]', 'RI')
		, ('South Carolina', 'SC')
		, ('South Dakota', 'SD')
		, ('Tennessee', 'TN')
		, ('Texas', 'TX')
		, ('Utah', 'UT')
		, ('Vermont', 'VT')
		, ('Virginia', 'VA')
		, ('Washington', 'WA')
		, ('West Virginia', 'WV')
		, ('Wisconsin', 'WI')
		, ('Wyoming', 'WY')
		, ('District of Columbia', 'DC')
	) S  (cdDesc, cdCode)
	WHERE NOT EXISTS(SELECT * FROM dbo.dbCodeLookup WHERE cdType = '224' AND cdCode = S.cdCode AND cdUICultureInfo = '{default}' AND cdAddLink IS NULL)
END


IF NOT EXISTS(SELECT 1 FROM dbo.dbCodeLookup WHERE cdType = '38')
BEGIN
	INSERT dbo.dbCodeLookup ( cdType , cdCode , cdUICultureInfo , cdDesc , cdSystem , cdDeletable , cdAddLink , cdHelp , cdNotes , cdGroup )
	SELECT 
		'38'
		, cdCode
		, '{default}'
		, cdDesc
		, 0
		, 1
		, NULL
		, NULL
		, NULL
		, 0
	FROM
	(
	VALUES 
		('Ontario', 'ON')
		, ('Quebec', 'QC')
		, ('Nova Scotia', 'NS')
		, ('New Brunswick', 'NB')
		, ('Manitoba', 'MB')
		, ('British Columbia', 'BC')
		, ('Prince Edward Island', 'PE')
		, ('Saskatchewan', 'SK')
		, ('Alberta', 'AB')
		, ('Newfoundland and Labrador', 'NL')
	) S  (cdDesc, cdCode)
	WHERE NOT EXISTS(SELECT * FROM dbo.dbCodeLookup WHERE cdType = '38' AND cdCode = S.cdCode AND cdUICultureInfo = '{default}' AND cdAddLink IS NULL)
END