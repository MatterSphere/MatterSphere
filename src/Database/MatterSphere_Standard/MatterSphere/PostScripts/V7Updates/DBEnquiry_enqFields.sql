Print 'Starting DBEnquiry_enqFields.sql'

IF NOT EXISTS (Select * from SysColumns where Name = 'contNoofEmp' and xType = 56 and Length = 4)
BEGIN
EXEC FWBSColumnChanges 	 @spTable = 'dbContactCompany'
	,@spColumn = 'contNoofEmp'
	,@spDefinition = 'INT NULL'
END

IF NOT EXISTS (Select * from SysColumns where Name = 'contNoofBranch' and xType = 56 and Length = 4)
BEGIN
EXEC FWBSColumnChanges 	 @spTable = 'dbContactCompany'
	,@spColumn = 'contNoofBranch'
	,@spDefinition = 'INT NULL'
END