

CREATE VIEW [external].[AssociateAccessAllows]
AS 

SELECT  assocID as ID, contid as ContactID, fileID as FileID FROM [external].vwdbAssociates


GO
GRANT UPDATE
    ON OBJECT::[external].[AssociateAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[AssociateAccessAllows] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[AssociateAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[AssociateAccessAllows] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[AssociateAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[AssociateAccessAllows] TO [OMSApplicationRole]
    AS [dbo];

