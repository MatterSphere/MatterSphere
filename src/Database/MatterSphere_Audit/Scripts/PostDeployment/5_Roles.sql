
if not exists (select 
					* 
				from 
					dbcodelookup 
				where 
					cdType = 'USRROLES'
					and cdCode = 'AUDITVIEWER')
begin
	insert into dbCodeLookup 
	(
		cdType
		,cdCode
		,cdUICultureInfo
		,cdDesc
		,cdSystem
		,cdDeletable
		,cdAddLink
		,cdHelp
		,cdNotes
		,cdGroup
	)
	values 
	(
		'USRROLES'
		, 'AUDITVIEWER'
		, '{default}'
		, 'Audit Viewer'
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
	print 'Audit Viewer user role already exists.'
end




if not exists (select 
					* 
				from 
					dbcodelookup 
				where 
					cdType = 'USRROLES'
					and cdCode = 'AUDITMANAGER')
begin
	insert into dbCodeLookup 
	(
		cdType
		,cdCode
		,cdUICultureInfo
		,cdDesc
		,cdSystem
		,cdDeletable
		,cdAddLink
		,cdHelp
		,cdNotes
		,cdGroup
	)
	values 
	(
		'USRROLES'
		, 'AUDITMANAGER'
		, '{default}'
		, 'Audit Manager'
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
	print 'Audit Manager user role already exists.'
end

