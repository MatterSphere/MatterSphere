

CREATE PROCEDURE [dbo].[sprFundType] (@Code uCodeLookup, @Currency char(3), @UI uUICultureInfo = '{default}') AS
select *,
	dbo.GetCodeLookupDesc('FUNDTYPE', ftcode, @UI) as [ftdesc],
	dbo.GetCodeLookupDesc('FTCLDESC', ftclcode, @UI) as [ftcldesc],	
	dbo.GetCodeLookupDesc('FTREFDESC', ftrefcode, @UI) as [ftrefdesc],
	dbo.GetCodeLookupDesc('FTAGREEMENT', ftagreementcode, @UI) as [ftagreementdesc]
from dbfundtype 
where ftcode = @Code and ftcurISOCode = @Currency

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFundType] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFundType] TO [OMSAdminRole]
    AS [dbo];

