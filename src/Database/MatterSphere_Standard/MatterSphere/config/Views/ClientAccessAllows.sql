

CREATE VIEW [config].[ClientAccessAllows]
AS 

SELECT  clID as ID FROM config.vwdbClient


GO
GRANT UPDATE
    ON OBJECT::[config].[ClientAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[ClientAccessAllows] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[ClientAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[ClientAccessAllows] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[ClientAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[ClientAccessAllows] TO [OMSApplicationRole]
    AS [dbo];

