

CREATE PROCEDURE [dbo].[sprCommandBarBuilder] (@Code uCodeLookup, @Version bigint = 0, @Force bit = 0, @UI uUICultureInfo = '{default}')  AS
declare @curversion bigint
-- Retrieve the enquiry version information.
select @curversion = cbversion from dbcommandbar where cbcode = @code
-- Check the version numbers.
if (@version < @curversion) or @force = 1
begin
	--Header Information
	--DataTable = COMMANDBAR
	select dbo.GetCodeLookupDesc('COMMANDBAR', cbcode, @UI) as [cbdesc], cbversion, cbposition,cbscript from dbcommandbar where cbcode = @code
	--Control Information
	--DataTable = CONTROLS
	select
		ctrlid,
		ctrlcode, 
		ctrlfilter,
		COALESCE(CL1.cdDesc, '~' + NULLIF(dbcommandbarcontrol.ctrlcode, '') + '~') as [ctrldesc],  
		dbo.GetCodeLookupHelp('CBCCAPTIONS', ctrlcode, @UI) as [ctrlhelp], 
		COALESCE(CL2.cdDesc, '~' + NULLIF(dbcommandbarcontrol.ctrlparent, '') + '~') as [ctrlparentdesc], 
		ctrltype, 
		ctrlbegingroup, 
		ctrlicon,
		ctrlruncommand,
		ctrlincfav,
		ctrlrole,
		ctrlcondition
	from dbcommandbarcontrol
	LEFT JOIN dbo.GetCodeLookupDescription ( 'CBCCAPTIONS', @UI ) CL1 ON CL1.[cdCode] =  dbcommandbarcontrol.ctrlcode
	LEFT JOIN dbo.GetCodeLookupDescription ( 'CBCCAPTIONS', @UI ) CL2 ON CL2.[cdCode] =  dbcommandbarcontrol.ctrlparent
	where ctrlcommandbar = @code and ctrlhide = 0 order by ctrllevel asc, ctrlparent asc, ctrlorder asc
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCommandBarBuilder] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCommandBarBuilder] TO [OMSAdminRole]
    AS [dbo];

