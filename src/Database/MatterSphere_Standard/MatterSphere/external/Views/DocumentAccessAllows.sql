
CREATE VIEW [external].[DocumentAccessAllows]
AS 

SELECT  docID as ID FROM [external].vwdbDocument


GO
GRANT UPDATE
    ON OBJECT::[external].[DocumentAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[DocumentAccessAllows] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[DocumentAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[DocumentAccessAllows] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[DocumentAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[DocumentAccessAllows] TO [OMSApplicationRole]
    AS [dbo];

