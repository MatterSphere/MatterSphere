IF EXISTS ( SELECT 'RESOURCE' , 'Author', 'Author(s)' INTERSECT SELECT cdType , cdCode , cdDesc FROM dbo.dbCodeLookup )
	BEGIN
		DELETE FROM dbo.dbCodeLookup
		WHERE cdType = 'RESOURCE' AND cdCode = 'Author' AND cdDesc = 'Author(s)'
	END

IF EXISTS ( SELECT 'RESOURCE' , 'client type', 'Client Type(s)' INTERSECT SELECT cdType , cdCode , cdDesc FROM dbo.dbCodeLookup )
	BEGIN
		DELETE FROM dbo.dbCodeLookup
		WHERE cdType = 'RESOURCE' AND cdCode = 'client type' AND cdDesc = 'Client Type(s)'
	END

IF EXISTS ( SELECT 'RESOURCE' , 'contact type', 'Contact Type(s)' INTERSECT SELECT cdType , cdCode , cdDesc FROM dbo.dbCodeLookup )
	BEGIN
		DELETE FROM dbo.dbCodeLookup
		WHERE cdType = 'RESOURCE' AND cdCode = 'contact type' AND cdDesc = 'Contact Type(s)'
	END

IF EXISTS ( SELECT 'RESOURCE' , 'document type', 'Document Type(s)' INTERSECT SELECT cdType , cdCode , cdDesc FROM dbo.dbCodeLookup )
	BEGIN
		DELETE FROM dbo.dbCodeLookup
		WHERE cdType = 'RESOURCE' AND cdCode = 'document type' AND cdDesc = 'Document Type(s)'
	END

IF EXISTS ( SELECT 'RESOURCE' , 'DocExtension', 'Document Extension(s)' INTERSECT SELECT cdType , cdCode , cdDesc FROM dbo.dbCodeLookup )
	BEGIN
		DELETE FROM dbo.dbCodeLookup
		WHERE cdType = 'RESOURCE' AND cdCode = 'DocExtension' AND cdDesc = 'Document Extension(s)'
	END

IF EXISTS ( SELECT 'RESOURCE' , 'PrecExtension', 'Precedent Extension(s)' INTERSECT SELECT cdType , cdCode , cdDesc FROM dbo.dbCodeLookup )
	BEGIN
		DELETE FROM dbo.dbCodeLookup
		WHERE cdType = 'RESOURCE' AND cdCode = 'PrecExtension' AND cdDesc = 'Precedent Extension(s)'
	END

IF EXISTS ( SELECT 'RESOURCE' , 'file status', 'File Status' INTERSECT SELECT cdType , cdCode , cdDesc FROM dbo.dbCodeLookup )
	BEGIN
		DELETE FROM dbo.dbCodeLookup
		WHERE cdType = 'RESOURCE' AND cdCode = 'file status' AND cdDesc = 'File Status'
	END

IF EXISTS ( SELECT 'RESOURCE' , 'file type', 'File Type(s)' INTERSECT SELECT cdType , cdCode , cdDesc FROM dbo.dbCodeLookup )
	BEGIN
		DELETE FROM dbo.dbCodeLookup
		WHERE cdType = 'RESOURCE' AND cdCode = 'file type' AND cdDesc = 'File Type(s)'
	END

IF EXISTS ( SELECT 'RESOURCE' , 'associate type', 'Associate Type(s)' INTERSECT SELECT cdType , cdCode , cdDesc FROM dbo.dbCodeLookup )
	BEGIN
		DELETE FROM dbo.dbCodeLookup
		WHERE cdType = 'RESOURCE' AND cdCode = 'associate type' AND cdDesc = 'Associate Type(s)'
	END

IF EXISTS ( SELECT 'RESOURCE' , 'MCTYPE', 'OMS Type(s)' INTERSECT SELECT cdType , cdCode , cdDesc FROM dbo.dbCodeLookup )
	BEGIN
		DELETE FROM dbo.dbCodeLookup
		WHERE cdType = 'RESOURCE' AND cdCode = 'MCTYPE' AND cdDesc = 'OMS Type(s)'
	END

IF EXISTS ( SELECT 'RESOURCE' , 'precedent type', 'Precedent Type(s)' INTERSECT SELECT cdType , cdCode , cdDesc FROM dbo.dbCodeLookup )
	BEGIN
		DELETE FROM dbo.dbCodeLookup
		WHERE cdType = 'RESOURCE' AND cdCode = 'precedent type' AND cdDesc = 'Precedent Type(s)'
	END

IF EXISTS ( SELECT 'RESOURCE' , 'appointmenttype', 'Appointment Type(s)' INTERSECT SELECT cdType , cdCode , cdDesc FROM dbo.dbCodeLookup )
	BEGIN
		DELETE FROM dbo.dbCodeLookup
		WHERE cdType = 'RESOURCE' AND cdCode = 'appointmenttype' AND cdDesc = 'Appointment Type(s)'
	END

IF EXISTS ( SELECT 'RESOURCE' , 'task type', 'Task Type(s)' INTERSECT SELECT cdType , cdCode , cdDesc FROM dbo.dbCodeLookup )
	BEGIN
		DELETE FROM dbo.dbCodeLookup
		WHERE cdType = 'RESOURCE' AND cdCode = 'task type' AND cdDesc = 'Task Type(s)'
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
