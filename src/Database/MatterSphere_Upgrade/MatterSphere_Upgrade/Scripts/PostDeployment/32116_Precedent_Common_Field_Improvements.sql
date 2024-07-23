IF EXISTS ( SELECT 'RESOURCE' , 'UNMERGESPECIFIC' , '{default}', 'This specifc data is secure and cannot be merged', NULL INTERSECT SELECT cdType , cdCode , cdUICultureInfo , cdDesc, cdAddLink FROM dbo.dbCodeLookup )
BEGIN
	UPDATE dbo.dbCodeLookup
	SET cdDesc = 'This specific data is secure and cannot be merged'
	WHERE cdType = 'RESOURCE'
		AND cdCode = 'UNMERGESPECIFIC'
		AND cdUICultureInfo = '{default}'
		AND cdAddLink IS NULL
END