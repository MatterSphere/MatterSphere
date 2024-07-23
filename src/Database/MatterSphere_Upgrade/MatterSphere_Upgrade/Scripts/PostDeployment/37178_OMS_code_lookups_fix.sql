IF EXISTS ( SELECT 'CATEGORY' , 'FILEREVIEW', '{default}' , NULL , 1 INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink , cdGroup FROM dbo.dbCodeLookup )
BEGIN
UPDATE dbo.dbCodeLookup SET cdGroup = 0
WHERE cdType = 'CATEGORY' AND cdCode = 'FILEREVIEW' AND cdUICultureInfo = '{default}' AND cdAddLink IS NULL AND cdGroup = 1
END

IF EXISTS ( SELECT 'SOURCETYPE' , 'OMS', '{default}' , NULL, 1 INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink , cdGroup FROM dbo.dbCodeLookup )
BEGIN
UPDATE dbo.dbCodeLookup SET cdGroup = 0
WHERE cdType = 'SOURCETYPE' AND cdCode = 'OMS' AND cdUICultureInfo = '{default}' AND cdAddLink IS NULL AND cdGroup = 1
END

IF EXISTS ( SELECT 'OMS' , 'OMS', '{default}' , NULL , 1 INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink , cdGroup FROM dbo.dbCodeLookup )
BEGIN
	IF NOT EXISTS ( SELECT 'CATEGORY' , 'OMS', '{default}' , NULL , 0 INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdAddLink , cdGroup FROM dbo.dbCodeLookup )
	BEGIN
		UPDATE dbo.dbCodeLookup SET cdType = 'CATEGORY', cdGroup = 0
		WHERE cdType = 'OMS' AND cdCode = 'OMS' AND cdUICultureInfo = '{default}' AND cdAddLink IS NULL AND cdGroup = 1
	END
	ELSE
	BEGIN
		DELETE FROM dbo.dbCodeLookup
		WHERE cdType = 'OMS' AND cdCode = 'OMS' AND cdUICultureInfo = '{default}' AND cdAddLink IS NULL AND cdGroup = 1
	END
END

