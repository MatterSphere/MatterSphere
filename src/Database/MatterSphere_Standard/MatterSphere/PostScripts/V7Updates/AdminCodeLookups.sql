
Print 'Starting V7Updates\AdminCodeLookups.sql'
if not exists (select * from dbCodeLookup where cdType = 'CATEGORY' and cdCode = 'GROUP')
begin
 insert into dbCodeLookup
(
cdType
, cdCode
, cdUICultureInfo
, cdDesc
, cdSystem
, cdDeletable
, cdAddLink
, cdHelp
, cdNotes
, cdGroup
)
values
(
'CATEGORY'
, 'GROUP'        
, '{default}'
, 'Group'
, 1
, 1
, NULL
, NULL
, NULL
, 0
)
End
 
 
if not exists (select * from dbCodeLookup where cdType = 'DIALOG' and cdCode = 'DLGGROUPCAPTION')
begin
insert into dbCodeLookup
(
cdType
, cdCode
, cdUICultureInfo
, cdDesc
, cdSystem
, cdDeletable
, cdAddLink
, cdHelp
, cdNotes
, cdGroup
)
values
(
'DIALOG'
, 'DLGGROUPCAPTION'        
, '{default}'
, 'Dialog Group Captions'
, 1
, 0
, NULL
, NULL
, NULL
, 1
)
end
 
 
if not exists (select * from dbCodeLookup where cdType = 'RESOURCE' and cdCode = 'GROUP')
begin
insert into dbCodeLookup
(
cdType
, cdCode
, cdUICultureInfo
, cdDesc
, cdSystem
, cdDeletable
, cdAddLink
, cdHelp
, cdNotes
, cdGroup
)
values
(
'RESOURCE'
, 'GROUP'        
, '{default}'
, 'Group'
, 1
, 0
, NULL
, NULL
, NULL
, 0
)
end




if not exists (select * from dbCodeLookup where cdtype = 'DLGGROUPCAPTION' and cdCode = 'NAVGRPCLINFO')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	( 
		'DLGGROUPCAPTION'	
		, 'NAVGRPCLINFO'
		, '{default}'
		, '%CLIENT% Information'
		, 0	
		, 1	
		, NULL
		, NULL
		, NULL
		, 0
	)
end
else
begin
	print 'Code lookup %CLIENT% Information (NAVGRPCLINFO) already exists in the database.'
end



if not exists (select * from dbCodeLookup where cdtype = 'DLGGROUPCAPTION' and cdCode = 'NAVGRPCLMANAGE')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	( 
		'DLGGROUPCAPTION'	
		, 'NAVGRPCLMANAGE'
		, '{default}'
		, '%CLIENT% Management'
		, 0	
		, 1	
		, NULL
		, NULL
		, NULL
		, 0
	)
end
begin
	print 'Code lookup %CLIENT% Management (NAVGRPCLMANAGE) already exists in the database.'
end



if not exists (select * from dbCodeLookup where cdtype = 'DLGGROUPCAPTION' and cdCode = 'NAVGRPCONTINFO')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	( 
		'DLGGROUPCAPTION'	
		, 'NAVGRPCONTINFO'
		, '{default}'
		, 'Contact Information'
		, 0	
		, 1	
		, NULL
		, NULL
		, NULL
		, 0
	)
end
begin
	print 'Code lookup Contact Information (NAVGRPCONTINFO) already exists in the database.'
end



if not exists (select * from dbCodeLookup where cdtype = 'DLGGROUPCAPTION' and cdCode = 'NAVGRPDOCUMENTS')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	( 
		'DLGGROUPCAPTION'	
		, 'NAVGRPDOCUMENTS'
		, '{default}'
		, 'Documents'
		, 0	
		, 1	
		, NULL
		, NULL
		, NULL
		, 0
	)
end
begin
	print 'Code lookup Documents (NAVGRPDOCUMENTS) already exists in the database.'
end



if not exists (select * from dbCodeLookup where cdtype = 'DLGGROUPCAPTION' and cdCode = 'NAVGRPFILINFO')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	( 
		'DLGGROUPCAPTION'	
		, 'NAVGRPFILINFO'
		, '{default}'
		, '%FILE% information'
		, 0	
		, 1	
		, NULL
		, NULL
		, NULL
		, 0
	)
end
begin
	print 'Code lookup %FILE% information (NAVGRPFILINFO) already exists in the database.'
end



if not exists (select * from dbCodeLookup where cdtype = 'DLGGROUPCAPTION' and cdCode = 'NAVGRPFILMANAGE')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	( 
		'DLGGROUPCAPTION'	
		, 'NAVGRPFILMANAGE'
		, '{default}'
		, '%FILE% Management'
		, 0	
		, 1	
		, NULL
		, NULL
		, NULL
		, 0
	)
end
begin
	print 'Code lookup %FILE% Management (NAVGRPFILMANAGE) already exists in the database.'
end



if not exists (select * from dbCodeLookup where cdtype = 'DLGGROUPCAPTION' and cdCode = 'NAVGRPFINANCE')
begin
	insert into dbCodeLookup
	(
		cdType
		, cdCode
		, cdUICultureInfo
		, cdDesc
		, cdSystem
		, cdDeletable
		, cdAddLink
		, cdHelp
		, cdNotes
		, cdGroup
	)
	values
	( 
		'DLGGROUPCAPTION'	
		, 'NAVGRPFINANCE'
		, '{default}'
		, 'Finance'
		, 0	
		, 1	
		, NULL
		, NULL
		, NULL
		, 0
	)
end
begin
	print 'Code lookup Finance (NAVGRPFINANCE) already exists in the database.'
end


