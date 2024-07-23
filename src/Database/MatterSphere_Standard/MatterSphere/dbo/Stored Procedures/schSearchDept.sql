

CREATE PROCEDURE [dbo].[schSearchDept]
(
@UI uCodelookup
)
AS
SELECT d.deptCode, COALESCE(CL.cdDesc, '~' + NULLIF(d.deptCode, '') + '~') as deptdesc, d.deptEmail, d.deptActive FROM
	dbDepartment d
	LEFT JOIN	[dbo].[GetCodeLookupDescription] ( 'DEPT', @ui ) CL ON CL.[cdCode] = d.deptCode
	order by d.deptcode

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchDept] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchDept] TO [OMSAdminRole]
    AS [dbo];

