

CREATE VIEW[dbo].[vwSnapClient]
AS
SELECT DATEPART(m, Created) AS Month, DATEPART(yyyy, Created) AS Year, COUNT(clID) AS NoClients, feeusrID
FROM         dbo.dbClient
GROUP BY DATEPART(m, Created), DATEPART(yyyy, Created), feeusrID

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwSnapClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSnapClient] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSnapClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSnapClient] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwSnapClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwSnapClient] TO [OMSApplicationRole]
    AS [dbo];

