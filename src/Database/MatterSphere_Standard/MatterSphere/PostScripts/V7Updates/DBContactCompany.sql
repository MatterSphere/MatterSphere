print 'Starting DBContactCompany.sql'

IF NOT EXISTS (Select * from SysColumns where Name = 'enqFields' and xType = 231 and Length = -1)
BEGIN
EXEC FWBSColumnChanges 	 @spTable = 'DBEnquiry'
	,@spColumn = 'enqFields'
	,@spDefinition = 'nvarchar(MAX) NULL'
END
