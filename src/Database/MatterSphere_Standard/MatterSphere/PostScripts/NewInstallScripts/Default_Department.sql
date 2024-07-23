Print 'Starting NewInstallScripts\Default_Department.sql'


-- Add default department
IF NOT EXISTS ( SELECT deptCode FROM dbo.dbDepartment WHERE deptCode = 'TEMPLATE' )
BEGIN
	INSERT dbo.dbDepartment ( deptCode , deptEmail , deptActive , deptAccCode , deptXML )
	VALUES ( 'TEMPLATE' , 'dept@domain.com' , 1 , NULL , '<config/>' )
END
GO



