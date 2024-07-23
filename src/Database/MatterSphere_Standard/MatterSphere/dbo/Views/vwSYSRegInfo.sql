

CREATE VIEW[dbo].[vwSYSRegInfo]
AS
SELECT     regCompanyName AS CompanyName, '' AS IntLawUpdate, '' AS IntLawUpdateFrom, '' AS IntLawUpdateSubject
FROM         dbo.dbRegInfo

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwSYSRegInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSYSRegInfo] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSYSRegInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSYSRegInfo] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwSYSRegInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwSYSRegInfo] TO [OMSApplicationRole]
    AS [dbo];

