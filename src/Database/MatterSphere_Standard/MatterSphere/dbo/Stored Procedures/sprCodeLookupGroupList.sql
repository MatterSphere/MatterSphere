

CREATE PROCEDURE [dbo].[sprCodeLookupGroupList] (@UI uUICultureInfo = '{default}',  @IncludeNull bit = 0)
as
	if @IncludeNull = 0
		select cdcode, dbo.GetCodeLookupDesc(cdtype, cdCode, @UI) as [cddesc], dbo.GetCodeLookupHelp(cdtype, cdCode, @UI) as [cdhelp] from dbcodelookup where cdgroup = 1 group by cdtype, cdcode
	else
		select null as cdcode, dbo.GetCodeLookupDesc('RESOURCE', 'RESNOTSET', @UI) as cddesc, null as cdhelp union select cdcode, dbo.GetCodeLookupDesc(cdtype, cdCode, @UI) as [cddesc], dbo.GetCodeLookupHelp(cdtype, cdCode, @UI) as [cdhelp] from dbcodelookup where cdgroup = 1 group by cdtype, cdcode

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCodeLookupGroupList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCodeLookupGroupList] TO [OMSAdminRole]
    AS [dbo];

