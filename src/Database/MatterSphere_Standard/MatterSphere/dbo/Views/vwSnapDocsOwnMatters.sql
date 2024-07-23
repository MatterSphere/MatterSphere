

CREATE VIEW[dbo].[vwSnapDocsOwnMatters]
AS
SELECT DATEPART(mm, dbo.dbDocument.Created) AS Month, DATEPART(yyyy, dbo.dbDocument.Created) AS Year, COUNT(dbo.dbDocument.docID) 
                      AS NoOwnDocuments, dbo.dbDocument.Createdby
FROM         dbo.dbDocument INNER JOIN
                      dbo.dbFile ON dbo.dbDocument.fileID = dbo.dbFile.fileID
GROUP BY DATEPART(mm, dbo.dbDocument.Created), DATEPART(yyyy, dbo.dbDocument.Created), dbo.dbDocument.Createdby

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwSnapDocsOwnMatters] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSnapDocsOwnMatters] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSnapDocsOwnMatters] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSnapDocsOwnMatters] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwSnapDocsOwnMatters] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwSnapDocsOwnMatters] TO [OMSApplicationRole]
    AS [dbo];

