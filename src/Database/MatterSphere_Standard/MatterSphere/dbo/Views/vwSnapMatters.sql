

CREATE VIEW[dbo].[vwSnapMatters]
AS
SELECT DATEPART(m, Created) AS Month, DATEPART(yyyy, Created) AS Year, COUNT(fileID) AS NoMatters, filePrincipleID
FROM         dbo.dbFile
GROUP BY DATEPART(m, Created), DATEPART(yyyy, Created), filePrincipleID

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwSnapMatters] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSnapMatters] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSnapMatters] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSnapMatters] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwSnapMatters] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwSnapMatters] TO [OMSApplicationRole]
    AS [dbo];

