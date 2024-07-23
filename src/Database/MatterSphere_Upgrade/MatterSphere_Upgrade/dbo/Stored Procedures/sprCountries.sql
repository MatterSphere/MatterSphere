

CREATE  PROCEDURE [dbo].[sprCountries] (@UI uUICultureInfo ='{default}') AS
-- Adjusted MNW to add the XML Data for speed caching
select ctryid
	, COALESCE(CL1.cdDesc, '~' + NULLIF(dbcountry.ctryCode, '') + '~') as [ctryname], ctryaddressformat 
from dbcountry 
LEFT JOIN dbo.GetCodeLookupDescription ( 'COUNTRIES', @UI ) CL1 ON CL1.[cdCode] =  dbcountry.ctryCode
where ctryignore = 0 order by ctryname

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCountries] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCountries] TO [OMSAdminRole]
    AS [dbo];

