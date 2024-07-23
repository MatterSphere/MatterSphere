print 'Starting VersionCodeLookup_AK.sql'

update dbAdminMenu 
set admnuSystem = 1
where admnuSearchListCode = 'LOBJLOCKAUDIT'
or admnuSearchListCode = 'LOBJLOCKLOCKED'
or admnuSearchListCode = 'LOBJRESTAUDIT'

GO

if not exists (select * from dbCodeLookup where cdType = 'USRROLES' and cdCode = 'OBJECTLOCKING')
begin
	insert into dbCodeLookup (cdType, cdCode, cdUICultureInfo, cdDesc, cdSystem, cdDeletable)
	values ('USRROLES', 'OBJECTLOCKING', '{default}', 'Object Locking', 0, 1)
end

GO

-- Version Comparison Selector

if not exists (select * from dbcodelookup where cdType = 'RESOURCE' and cdCode = 'VCSMSGCAPTION')
BEGIN
	insert into dbCodelookup (cdType, cdCode, cdDesc)
	values ('RESOURCE', 'VCSMSGCAPTION', 'Version Comparison')
END

GO

if not exists (select * from dbcodelookup where cdType = 'RESOURCE' and cdCode = 'VCSNOCURVERSION')
BEGIN
	insert into dbCodelookup (cdType, cdCode, cdDesc)

	values ('RESOURCE', 'VCSNOCURVERSION', 'There is no current version to work with within the version list.')
END

GO

if not exists (select * from dbcodelookup where cdType = 'RESOURCE' and cdCode = 'VCSNORESTSAMEV')
BEGIN
insert into dbCodelookup (cdType, cdCode, cdDesc)
values ('RESOURCE', 'VCSNORESTSAMEV', 'Restoration will not take place as the version selected from the list is the same as the current (production) version.')
END

GO

if not exists (select * from dbcodelookup where cdType = 'RESOURCE' and cdCode = 'VCSSELDIFFOBJ')
BEGIN
	insert into dbCodelookup (cdType, cdCode, cdDesc)
	values ('RESOURCE', 'VCSSELDIFFOBJ', 'Please select a version that is different to the version in the object list.')
END

GO

-- Object Version Data Selector

if not exists (select * from dbcodelookup where cdType = 'RESOURCE' and cdCode = 'OVDSCISLIST1')
BEGIN
	insert into dbCodelookup (cdType, cdCode, cdDesc)
	values ('RESOURCE', 'OVDSCISLIST1', 'Below is a list of check-in sets which include version.')
END

GO

if not exists (select * from dbcodelookup where cdType = 'RESOURCE' and cdCode = 'OVDSCISLIST2')
BEGIN
	insert into dbCodelookup (cdType, cdCode, cdDesc)
	values ('RESOURCE', 'OVDSCISLIST2', 'of')
END

GO

if not exists (select * from dbcodelookup where cdType = 'RESOURCE' and cdCode = 'OVDSCISOBJECTS')
BEGIN
	insert into dbCodelookup (cdType, cdCode, cdDesc)
	values ('RESOURCE', 'OVDSCISOBJECTS', 'Below is list of the objects in the selected check-in set.')
END

GO

if not exists (select * from dbcodelookup where cdType = 'RESOURCE' and cdCode = 'OVDSMSGCAPTION')
BEGIN
	insert into dbCodelookup (cdType, cdCode, cdDesc)
	values ('RESOURCE', 'OVDSMSGCAPTION', 'Object Restoration')
END

GO

if not exists (select * from dbcodelookup where cdType = 'RESOURCE' and cdCode = 'OVDSONLYONEOBJ1')
BEGIN
	insert into dbCodelookup (cdType, cdCode, cdDesc)
	values ('RESOURCE', 'OVDSONLYONEOBJ1', 'You have opted to rollback the selected item only. No linked items will be affected by this process.')
END

GO

if not exists (select * from dbcodelookup where cdType = 'RESOURCE' and cdCode = 'OVDSONLYONEOBJ2')
BEGIN
	insert into dbCodelookup (cdType, cdCode, cdDesc)
	values ('RESOURCE', 'OVDSONLYONEOBJ2', 'Are you sure you wish to continue?')
END

GO


-- IObjectComparison

if not exists (select * from dbcodelookup where cdType = 'RESOURCE' and cdCode = 'IOCCONFIGCHECK')
BEGIN
	insert into dbCodelookup (cdType, cdCode, cdDesc)
	values ('RESOURCE', 'IOCCONFIGCHECK', 'The system configuration for object comparison needs to be checked.
Please contact your System Administrator.')
END

GO

if not exists (select * from dbcodelookup where cdType = 'RESOURCE' and cdCode = 'IOCMSGCAPTION')
BEGIN
	insert into dbCodelookup (cdType, cdCode, cdDesc)
	values ('RESOURCE', 'IOCMSGCAPTION', 'System Configuration')
END

GO

-- Precedent Script Restoration

if not exists (select * from dbcodelookup where cdType = 'RESOURCE' and cdCode = 'PRECSCRRESTORE1')
BEGIN
	insert into dbCodelookup (cdType, cdCode, cdDesc)
	values ('RESOURCE', 'PRECSCRRESTORE1', 'Do you wish to set the Precedent''s script to that linked to the version you have selected?')
END

GO

if not exists (select * from dbcodelookup where cdType = 'RESOURCE' and cdCode = 'PRECSCRRESTORE2')
BEGIN
	insert into dbCodelookup (cdType, cdCode, cdDesc)
	values ('RESOURCE', 'PRECSCRRESTORE2', 'Script Restoration')
END

GO

if not exists (select * from dbcodelookup where cdType = 'RESOURCE' and cdCode = 'PRECSCRRESTORE3')
BEGIN
	insert into dbCodelookup (cdType, cdCode, cdDesc)
	values ('RESOURCE', 'PRECSCRRESTORE3', 'The script has been successfully restored.')
END

GO

-- Precedent Developer Role

if not exists (select * from dbcodelookup where cdType = 'USRROLES' and cdCode = 'PRECDEVELOPER')
BEGIN
	insert into dbCodelookup (cdType, cdCode, cdDesc)
	values ('USRROLES', 'PRECDEVELOPER', 'Precedent Developer')
END

GO

