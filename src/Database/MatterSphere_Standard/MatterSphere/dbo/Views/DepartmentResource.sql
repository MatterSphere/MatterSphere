

CREATE VIEW[dbo].[DepartmentResource]
AS
SELECT        cdCode AS Code, cdDesc AS Description
FROM            dbo.dbCodeLookup
WHERE        (cdType = N'DEPT') AND (cdUICultureInfo = '{default}')

GO
GRANT UPDATE
    ON OBJECT::[dbo].[DepartmentResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DepartmentResource] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DepartmentResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DepartmentResource] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[DepartmentResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[DepartmentResource] TO [OMSApplicationRole]
    AS [dbo];

