

CREATE PROCEDURE [dbo].[sprCreateObjectListOptions] 
       @report nvarchar(12) = null
AS
BEGIN
       if @report = 'ADMIN'
             select null as cdCode, '(Not Specified)' as cdDesc union
             select cdCode, cdDesc from dbCodeLookup where cdtype = 'ADMTNODECATEGRY' and cdDesc != 'Report'
       else
             select null as cdCode, '(Not Specified)' as cdDesc union
             select cdCode, cdDesc from dbCodeLookup where cdtype = 'ADMTNODECATEGRY' and cdDesc = 'Report'
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCreateObjectListOptions] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCreateObjectListOptions] TO [OMSAdminRole]
    AS [dbo];

