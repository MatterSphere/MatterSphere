
print 'starting dbvervsion'

-- Script to insert build Revision number in dbVersion  
IF NOT EXISTS ( SELECT rowguid FROM [dbo].[dbVersion] WHERE verMajor = 10 AND verMinor = 1 AND verRevision = 2 AND verBuild = 0 )
BEGIN
INSERT dbVersion (verMajor, verMinor, verBuild, verRevision, verBeta, verTag, verNotes)
	VALUES	( 10 , 1 , 0 , 2 , '' , 'V10.1.02' , 'Database schema V10.1.02' )
END
GO