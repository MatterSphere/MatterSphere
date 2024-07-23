

CREATE VIEW[dbo].[vwSnapDocsGenerated]
AS
SELECT DATEPART(m, Created) AS Month, DATEPART(yyyy, Created) AS Year, COUNT(docID) AS NoDocuments, Createdby
FROM         dbo.dbDocument
GROUP BY DATEPART(m, Created), DATEPART(yyyy, Created), Createdby

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwSnapDocsGenerated] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSnapDocsGenerated] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSnapDocsGenerated] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSnapDocsGenerated] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwSnapDocsGenerated] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwSnapDocsGenerated] TO [OMSApplicationRole]
    AS [dbo];

