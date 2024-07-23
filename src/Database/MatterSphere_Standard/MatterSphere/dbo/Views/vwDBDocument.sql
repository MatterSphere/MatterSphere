

CREATE VIEW[dbo].[vwDBDocument]
AS
SELECT     dbo.dbDocument.docID AS DOCID, dbo.dbDocument.clID AS CLID, dbo.dbDocument.fileID AS MatterID, dbo.dbDocument.assocID AS AssocID, 
                      dbo.dbDocument.docDesc AS DocDesc, dbo.dbDocument.docFileName AS DocFileName, dbo.dbDocument.docType AS DocType, 
                      dbo.dbDocument.docDirection AS DocDirection, dbo.dbUser.usrInits AS CreatedBy, dbo.dbDocument.Created AS CreatedOn, 0 AS Protect, 
                      dbo.dbDocument.docprecID AS PrecID, dbo.dbDocument.docAppID AS AppName, dbo.dbDocument.docPassword AS DocPassword, 0 AS DocPrivilege, 
                      0 AS DocArchived, 0 AS DocArchivedLink, '' AS DocBranch, 0 AS DocOfflineID
FROM         dbo.dbDocument INNER JOIN
                      dbo.dbUser ON dbo.dbDocument.Createdby = dbo.dbUser.usrID

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBDocument] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBDocument] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBDocument] TO [OMSApplicationRole]
    AS [dbo];

