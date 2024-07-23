

CREATE PROCEDURE [dbo].[sprFetchReport]  (@Code uCodeLookup, @Version bigint = 0, @Force bit = 0, @UI uUICultureInfo = '{default}') AS
declare @curversion bigint
select @curversion = rptversion from dbreport where rptcode = @Code
-- Check the version numbers.
if (@version < @curversion) or @force = 1
begin
	--HEADER
	select *, dbo.GetCodeLookupDesc('REPORT', rptcode, @UI) as [rptdesc] from dbreport where rptcode = @Code
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFetchReport] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFetchReport] TO [OMSAdminRole]
    AS [dbo];

