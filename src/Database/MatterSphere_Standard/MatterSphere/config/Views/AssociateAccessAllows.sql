

CREATE VIEW [config].[AssociateAccessAllows]
AS 

SELECT  assocID as ID, contid as ContactID, fileID as FileID FROM config.vwdbAssociates


GO
GRANT UPDATE
    ON OBJECT::[config].[AssociateAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[AssociateAccessAllows] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[AssociateAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[AssociateAccessAllows] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[AssociateAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[AssociateAccessAllows] TO [OMSApplicationRole]
    AS [dbo];

