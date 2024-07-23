

CREATE VIEW[dbo].[vwDBAssociates]
AS
SELECT     dbo.dbAssociates.assocID AS AssocID, dbo.dbAssociates.fileID AS MatterID, dbo.dbAssociates.assocOrder AS AssocOrder, 
                      dbo.dbAssociates.assocType AS AssocAddType, dbo.dbAssociates.contID AS AssocAddID, dbo.dbContact.contName AS AssocAddName, 
                      dbo.dbAssociates.assocRef AS AssocRef, dbo.dbAssociates.assocSalut AS AssocContact, dbo.dbAssociates.assocNotes AS AssocNotes, 
                      dbo.dbAssociates.assocAddressee AS AssocAddressee, dbo.dbAssociates.assocEmail AS AssocEmail, dbo.dbAssociates.assocDDI AS AssocDirectTel, 
                      dbo.dbAssociates.assocFax AS AssocDirectFax, dbo.dbAssociates.assocHeading AS AssocHeading, 0 AS AssocUseDX, 
                      dbo.dbAssociates.Created AS AssocLinked, dbo.dbUser.usrInits AS AssocLinkedBy, dbo.dbAssociates.Updated AS AssocUpdated, 
                      dbo.dbAssociates.assocActive AS AssocActive, dbUser_1.usrInits AS AssocUpdatedBy, dbo.dbAssociates.assocMobile AS AssocMobile
FROM         dbo.dbAssociates INNER JOIN
                      dbo.dbContact ON dbo.dbAssociates.contID = dbo.dbContact.contID INNER JOIN
                      dbo.dbUser ON dbo.dbAssociates.CreatedBy = dbo.dbUser.usrID INNER JOIN
                      dbo.dbUser dbUser_1 ON dbo.dbAssociates.UpdatedBy = dbUser_1.usrID

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBAssociates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBAssociates] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBAssociates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBAssociates] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBAssociates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBAssociates] TO [OMSApplicationRole]
    AS [dbo];

