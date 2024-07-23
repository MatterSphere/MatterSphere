CREATE PROCEDURE [dbo].[sprEnquiryBuilder] (@Code uCodeLookup, @Version bigint = 0, @Force bit = 0, @UI uUICultureInfo = '{default}', @Edition varchar(2) = null, @UIType tinyint = 0, @Designer bit = 0)  AS
declare @id int
declare @curversion bigint

-- Retrieve the enquiry version information.
select @id = enqid, @curversion = enqversion from dbenquiry where enqcode = @code

if @Designer = 0
begin

	-- Check the version numbers.
	if (@version < @curversion) or @force = 1
	begin

		--Header Information
		--DataTable = ENQUIRY
		select enqcode, dbo.GetCodeLookupDesc('ENQHEADER', enqcode, @UI) as enqdesc, enqversion, enqengineversion, enqsourcetype, enqsource, enqparameters, enqcall, enqwhere, dbo.GetCodeLookupDesc('ENQPAGE', enqWelcomeHeaderCode, @UI) as enqWelcomeHeader, dbo.GetCodeLookupDesc('ENQWELCOME', enqWelcomeTextCode, @UI) as enqWelcomeText, enqPaddingX, enqPaddingY,   enqLeadingX, enqLeadingY, enqModes, enqBindings, enqCanvasHeight, enqCanvasWidth, enqWizardHeight, enqWizardWidth, enqFields, enqScript, enqHelp, enqHelpKeyword, enqSettings from dbenquiry where enqcode = @code
	
		--Get data source insertion order.
		--DataTable = UPDATEORDER
		select enqtable from dbenquirydatasource where enqid =  @id order by enqorder

		--Get the page header descriptions.
		--DataTable = PAGES
		select pgename
			, pgecustom
			, COALESCE(CL1.cdDesc, '~' + NULLIF(pgeCode, '') + '~') as pgeDesc
			, COALESCE(CL2.cdDesc, '~' + NULLIF(pgeShortCode, '') + '~') as pgeShortDesc
			, pgeFinishedEnabled, pgeCondition, pgeRole, pgeLicense, pgeSettings 
		from dbenquirypage 
		LEFT JOIN dbo.GetCodeLookupDescription ( 'ENQPAGE', @UI ) CL1 ON CL1.[cdCode] =  pgeCode
		LEFT JOIN dbo.GetCodeLookupDescription ( 'ENQTAB', @UI ) CL2 ON CL2.[cdCode] =  pgeShortCode
		where enqid = @id and (pgeedition is null or pgeedition = @Edition) order by pgeorder asc

		--Get the validation methods of the form.
		--DataTable = METHODS
		select enqmethod, enqparameters from dbenquirymethod where enqid = @id order by enqorder


		--Get the questions.
		--DataTable = QUESTIONS
		select 
			qucode, 
			COALESCE(CL1.cdDesc, '~' + NULLIF(qucode, '') + '~') as qudesc,
			quname,
			(case when @UIType = 0 then C.ctrlwintype when @UIType = 1 then C.ctrlwebtype else C.ctrlpdatype end) as [qucontrol],
			C.ctrlAssemblyFileName as [quAssembly],
			C.ctrlwebtype,
			quctrlID,
			qudatalist, 
			DS.enqsourcetype as [qusourcetype], 
			DS.enqsource as [qusource], 
			DS.enqcall as [qucall], 
			DS.enqparameters as [quparameters], 
			qutype, 
			quminlength,
			qumaxlength,	
			quwidth,
			quheight,
			quhidden,
			qupage,
			dbo.GetCodeLookupHelp('ENQQUESTION', qucode, @UI) as quhelp,
			qumask,
			qudefault,
			quextendeddata,
			qutable,
			qufieldname,
			quproperty,
			qureadonly,
			qutaborder,
			qurequired,
			quunique,
			quconstraint,
			quX,
			quY,
			quwizx,
			quwizy,
			qucaptionwidth,
			qucustom,
			quanchor,
			quFormat,
			qucolumn,
			(case when qucommand is null then null else COALESCE(CL2.cdDesc, '~' + NULLIF(CMD.cmdcode, '') + '~') end) as [qucmddesc],
			(case when qucommand is null then null else dbo.GetCodeLookupHelp('ENQCOMMAND', CMD.cmdcode, @UI) end) as [qucmdhelp],
			(case when qucommand is null then null else CMD.cmdmethod end) as [qucmdmethod],
			(case when qucommand is null then null else CMD.cmdparameters end) as [qucmdparameters],
			(case when qucommand is null then null else CMD.cmdtype end) as [qucmdtype],
			qucommandretval,
			quadd,
			quedit,
			quaddseclevel,
			queditseclevel,
			qusearch,
			qufilter,
			qucasing,
			quHelpKeyword,
			quCondition,
			quRole,
			quLicense,
			quVisibleRole,
			quEditableRole
		from
			dbenquiryquestion
		inner join dbenquirycontrol C
			on quctrlid = C.ctrlid
		left join dbenquirydatalist DS
			on qudatalist = DS.enqtable
		left join dbenquirycommand CMD
			on qucommand = CMD.cmdcode
		LEFT JOIN dbo.GetCodeLookupDescription ( 'ENQQUESTION', @UI ) CL1 ON CL1.[cdCode] =  qucode
		LEFT JOIN dbo.GetCodeLookupDescription ( 'ENQCOMMAND', @UI ) CL2 ON CL2.[cdCode] =  CMD.cmdcode
		where 
			enqid = @id and not enqid is null
		and 
			(quedition is null or quedition = @Edition)




		order by quorder asc

	end
end
else
begin
	--Header Information
	--DataTable = ENQUIRY
	select *, dbo.GetCodeLookupDesc('ENQHEADER', enqcode, @UI) as enqdesc, dbo.GetCodeLookupDesc('ENQPAGE', enqWelcomeHeaderCode, @UI)  as enqWelcomeHeader, dbo.GetCodeLookupDesc('ENQWELCOME', enqWelcomeTextCode, @UI) as enqWelcomeText, enqScript from dbenquiry where enqcode = @code
	
	--Get data source insertion order.
	--DataTable = UPDATEORDER
	select * from dbenquirydatasource where enqid =  @id order by enqorder

	--Get the page header descriptions.	
	--DataTable = PAGES
	select dbenquirypage.*
		, COALESCE(CL1.cdDesc, '~' + NULLIF(pgeCode, '') + '~') as pgeDesc
		, COALESCE(CL2.cdDesc, '~' + NULLIF(pgeShortCode, '') + '~') as pgeShortDesc
	from dbenquirypage 
	LEFT JOIN dbo.GetCodeLookupDescription ( 'ENQPAGE', @UI ) CL1 ON CL1.[cdCode] =  pgeCode
	LEFT JOIN dbo.GetCodeLookupDescription ( 'ENQTAB', @UI ) CL2 ON CL2.[cdCode] =  pgeShortCode
	where enqid = @id 
	order by pgeorder asc

	--Get the validation methods of the form.
	--DataTable = METHODS
	select * from dbenquirymethod where enqid = @id order by enqorder

	--Get the enquiry forms questions.
	--DataTable = QUESTIONS
	select 
		dbenquiryquestion.*,
		C.ctrlAssemblyFileName as [quAssembly],
		COALESCE(CL1.cdDesc, '~' + qucode + '~') as qudesc,
		(case when @UIType = 0 then C.ctrlwintype else C.ctrlwebtype end)	as [qucontrol],
		DS.enqsourcetype as [qusourcetype], 
		DS.enqsource as [qusource], 
		DS.enqcall as [qucall], 
		DS.enqparameters as [quparameters], 
		dbo.GetCodeLookupHelp('ENQQUESTION', qucode, @UI) as quhelp,
		(case when qucommand is null then null else COALESCE(CL2.cdDesc, '~' + NULLIF(CMD.cmdcode, '') + '~') end) as [qucmddesc],
		(case when qucommand is null then null else COALESCE(CL2.cdDesc, '~' + NULLIF(CMD.cmdcode, '') + '~') end) as [qucmdhelp],
		(case when qucommand is null then null else CMD.cmdmethod end) as [qucmdmethod],
		(case when qucommand is null then null else CMD.cmdparameters end) as [qucmdparameters],
		(case when qucommand is null then null else CMD.cmdtype end) as [qucmdtype]

	from
		dbenquiryquestion
	inner join dbenquirycontrol C
		on quctrlid = C.ctrlid
	left join dbenquirydatalist DS
		on qudatalist = DS.enqtable
	left join dbenquirycommand CMD
			on qucommand = CMD.cmdcode
	LEFT JOIN dbo.GetCodeLookupDescription ( 'ENQQUESTION', @UI ) CL1 ON CL1.[cdCode] =  qucode
	LEFT JOIN dbo.GetCodeLookupDescription ( 'ENQCOMMAND', @UI ) CL2 ON CL2.[cdCode] =  CMD.cmdcode
	where 
		enqid = @id and not enqid is null
	order by quorder asc

	
	--Registered enquiry controls.
	--DataTable = CONTROLS
	execute sprEnquiryTableInformation 'CONTROLS', @UI

	--Get all the data lists.
	--DataTable = LISTS
	execute sprEnquiryTableInformation 'LISTS', @UI

	-- Get all the tables to pick from.
	--DataTable = TABLES
	execute sprEnquiryTableInformation 'TABLES', @UI


	-- Get all of the potentially used code lookups.
	--DataTable = LOOKUPS
	-- Get all of the potentially used code lookups.
	select * from dbcodelookup where cdtype in ('ENQHEADER', 'ENQWELCOME', 'ENQPAGE', 'ENQTAB', 'ENQQUESTION', 'ENQDATALIST', 'ENQCOMMAND', 'ENQCONSTANT', 'ENQQUESTCUETXT')


	-- Gets all the enquiry forms so that the designer can navigate through the existing forms.
	--DataTable = FORMS
	execute sprEnquiryTableInformation 'FORMS', @UI

	-- Gets all the commands.
	-- DataTable = COMMANDS
	execute sprEnquiryTableInformation 'COMMANDS', @UI

	-- Gets all the extended data objects.
	--DataTable = EXTENDEDDATA
	execute sprEnquiryTableInformation 'EXTENDEDDATA', @UI
end
