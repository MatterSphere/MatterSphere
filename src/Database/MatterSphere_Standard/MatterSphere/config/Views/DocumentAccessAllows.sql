
CREATE VIEW [config].[DocumentAccessAllows]
AS 

SELECT  docID as ID FROM config.vwdbDocument


GO
GRANT UPDATE
    ON OBJECT::[config].[DocumentAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[DocumentAccessAllows] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[DocumentAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[DocumentAccessAllows] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[DocumentAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[DocumentAccessAllows] TO [OMSApplicationRole]
    AS [dbo];

