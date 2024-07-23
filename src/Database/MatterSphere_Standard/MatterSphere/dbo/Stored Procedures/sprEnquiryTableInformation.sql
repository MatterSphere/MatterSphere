

CREATE PROCEDURE [dbo].[sprEnquiryTableInformation] (@Table nvarchar(25), @UI uUICultureInfo = '{default}')  AS
if @Table = 'CONTROLS'
	--Registered enquiry controls.
	select dbenquirycontrol.*
		, COALESCE(CL1.cdDesc, '~' + NULLIF(ctrlcode, '') + '~') as [ctrldesc]
		, dbo.GetCodeLookupHelp('ENQCONTROL', ctrlcode, @UI) as [ctrlhelp]
		, COALESCE(CL2.cdDesc, '~' + NULLIF(ctrlgroup, '') + '~') as [ctrlgroupdesc] 
	from dbenquirycontrol 
		LEFT JOIN dbo.GetCodeLookupDescription ( 'ENQCONTROL', @UI ) CL1 ON CL1.[cdCode] =  ctrlcode
		LEFT JOIN dbo.GetCodeLookupDescription ( 'ENQCTRLGROUP', @UI ) CL2 ON CL2.[cdCode] =  ctrlgroup
	order by ctrlgroupdesc, ctrldesc desc
else if @Table = 'LISTS'
	--Get all the data lists.
	select enqTable
		, COALESCE(CL1.cdDesc, '~' + NULLIF(enqTable, '') + '~') as enqTableDesc
		, enqSourceType
		, enqSource
		, enqCall
		, enqParameters
		, enqSystem 
	from dbenquirydatalist 
		LEFT JOIN dbo.GetCodeLookupDescription ( 'ENQDATALIST', @UI ) CL1 ON CL1.[cdCode] =  enqTable
	order by enqTableDesc asc
else if @Table = 'TABLES'
	-- Get all the tables to pick from.
	select [name] as tblname from sysobjects where type = 'U' order by name
else if @Table = 'FORMS'
	-- Gets all the enquiry forms so that the designer can navigate through the existing forms.
	select  enqcode
		, COALESCE(CL1.cdDesc, '~' + NULLIF(enqcode, '') + '~') as enqdesc
		, enqpath
		, enqbindings
		, enqsystem
		, enqScript 
	from dbenquiry 
		LEFT JOIN dbo.GetCodeLookupDescription ( 'ENQHEADER', @UI ) CL1 ON CL1.[cdCode] =  enqcode
	order by enqpath asc, enqdesc asc
else if @Table = 'COMMANDS'
	-- Gets all the commands.
	select dbenquirycommand.*
		, COALESCE(CL1.cdDesc, '~' + NULLIF(cmdcode, '') + '~') as [cmddesc]
		, dbo.GetCodeLookupHelp('ENQCOMMAND', cmdcode, @UI)  as [cmdhelp] 
	from dbenquirycommand
		LEFT JOIN dbo.GetCodeLookupDescription ( 'ENQCOMMAND', @UI ) CL1 ON CL1.[cdCode] =  cmdcode
else if @Table = 'EXTENDEDDATA'
	-- Gets all the extended data objects.
	select  extcode
		, COALESCE(CL1.cdDesc, '~' + NULLIF(extcode, '') + '~') as extdesc  
	from dbextendeddata
		LEFT JOIN dbo.GetCodeLookupDescription ( 'EXTENDEDDATA', @UI ) CL1 ON CL1.[cdCode] =  extcode

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprEnquiryTableInformation] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprEnquiryTableInformation] TO [OMSAdminRole]
    AS [dbo];

