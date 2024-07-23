IF EXISTS ( SELECT 'MSSEARCH' INTERSECT SELECT cdType FROM dbo.dbCodeLookup )
	BEGIN
		DELETE FROM dbo.dbCodeLookup
		WHERE cdType = 'MSSEARCH'
	END

IF NOT EXISTS ( SELECT * FROM dbo.dbCodeLookup WHERE CDTYPE = 'OMS' AND CDCODE = 'SEARCH' AND CDGROUP = 1 )
	INSERT INTO dbo.dbCodeLookup ( CDTYPE , CDCODE , CDDESC , CDGROUP ) VALUES ( 'OMS' , 'SEARCH' , 'Global Search Facet Fields' , 1 ) 

IF NOT EXISTS ( SELECT * FROM dbo.dbCodeLookup WHERE CDTYPE = 'SEARCH' AND CDCODE = 'Author')
	INSERT INTO dbo.dbCodeLookup ( CDTYPE , CDCODE , CDDESC ) VALUES ( 'SEARCH' , 'Author' , 'Author(s)' ) 

IF NOT EXISTS ( SELECT * FROM dbo.dbCodeLookup WHERE CDTYPE = 'SEARCH' AND CDCODE = 'client type')
	INSERT INTO dbo.dbCodeLookup ( CDTYPE , CDCODE , CDDESC ) VALUES ( 'SEARCH' , 'client type' , '%CLIENT% Type(s)' ) 

IF NOT EXISTS ( SELECT * FROM dbo.dbCodeLookup WHERE CDTYPE = 'SEARCH' AND CDCODE = 'contact type')
	INSERT INTO dbo.dbCodeLookup ( CDTYPE , CDCODE , CDDESC ) VALUES ( 'SEARCH' , 'contact type' , '%CONTACT% Type(s)' ) 

IF NOT EXISTS ( SELECT * FROM dbo.dbCodeLookup WHERE CDTYPE = 'SEARCH' AND CDCODE = 'document type')
	INSERT INTO dbo.dbCodeLookup ( CDTYPE , CDCODE , CDDESC ) VALUES ( 'SEARCH' , 'document type' , 'Document Type(s)' ) 

IF NOT EXISTS ( SELECT * FROM dbo.dbCodeLookup WHERE CDTYPE = 'SEARCH' AND CDCODE = 'DocExtension')
	INSERT INTO dbo.dbCodeLookup ( CDTYPE , CDCODE , CDDESC ) VALUES ( 'SEARCH' , 'DocExtension' , 'Document Extension(s)' ) 

IF NOT EXISTS ( SELECT * FROM dbo.dbCodeLookup WHERE CDTYPE = 'SEARCH' AND CDCODE = 'PrecExtension')
	INSERT INTO dbo.dbCodeLookup ( CDTYPE , CDCODE , CDDESC ) VALUES ( 'SEARCH' , 'PrecExtension' , '%PRECEDENT% Extension(s)' ) 

IF NOT EXISTS ( SELECT * FROM dbo.dbCodeLookup WHERE CDTYPE = 'SEARCH' AND CDCODE = 'file status')
	INSERT INTO dbo.dbCodeLookup ( CDTYPE , CDCODE , CDDESC ) VALUES ( 'SEARCH' , 'file status' , '%FILE% Status' ) 

IF NOT EXISTS ( SELECT * FROM dbo.dbCodeLookup WHERE CDTYPE = 'SEARCH' AND CDCODE = 'file type')
	INSERT INTO dbo.dbCodeLookup ( CDTYPE , CDCODE , CDDESC ) VALUES ( 'SEARCH' , 'file type' , '%FILE% Type(s)' ) 

IF NOT EXISTS ( SELECT * FROM dbo.dbCodeLookup WHERE CDTYPE = 'SEARCH' AND CDCODE = 'associate type')
	INSERT INTO dbo.dbCodeLookup ( CDTYPE , CDCODE , CDDESC ) VALUES ( 'SEARCH' , 'associate type' , '%ASSOCIATE% Type(s)' ) 

IF NOT EXISTS ( SELECT * FROM dbo.dbCodeLookup WHERE CDTYPE = 'SEARCH' AND CDCODE = 'MCTYPE')
	INSERT INTO dbo.dbCodeLookup ( CDTYPE , CDCODE , CDDESC ) VALUES ( 'SEARCH' , 'MCTYPE' , 'OMS Type(s)' ) 

IF NOT EXISTS ( SELECT * FROM dbo.dbCodeLookup WHERE CDTYPE = 'SEARCH' AND CDCODE = 'precedent type')
	INSERT INTO dbo.dbCodeLookup ( CDTYPE , CDCODE , CDDESC ) VALUES ( 'SEARCH' , 'precedent type' , '%PRECEDENT% Type(s)' ) 

IF NOT EXISTS ( SELECT * FROM dbo.dbCodeLookup WHERE CDTYPE = 'SEARCH' AND CDCODE = 'appointmenttype')
	INSERT INTO dbo.dbCodeLookup ( CDTYPE , CDCODE , CDDESC ) VALUES ( 'SEARCH' , 'appointmenttype' , 'Appointment Type(s)' ) 

IF NOT EXISTS ( SELECT * FROM dbo.dbCodeLookup WHERE CDTYPE = 'SEARCH' AND CDCODE = 'task type')
	INSERT INTO dbo.dbCodeLookup ( CDTYPE , CDCODE , CDDESC ) VALUES ( 'SEARCH' , 'task type' , 'Task Type(s)' ) 
