

CREATE VIEW [external].[ClientAccessAllows]
AS 

SELECT  clID as ID FROM [external].vwdbClient


GO
GRANT UPDATE
    ON OBJECT::[external].[ClientAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[ClientAccessAllows] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[ClientAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[ClientAccessAllows] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[ClientAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[ClientAccessAllows] TO [OMSApplicationRole]
    AS [dbo];

