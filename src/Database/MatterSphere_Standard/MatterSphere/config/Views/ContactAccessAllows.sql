
CREATE VIEW [config].[ContactAccessAllows]
AS 

SELECT  contID as ID FROM config.vwdbContact


GO
GRANT UPDATE
    ON OBJECT::[config].[ContactAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[ContactAccessAllows] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[ContactAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[ContactAccessAllows] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[ContactAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[ContactAccessAllows] TO [OMSApplicationRole]
    AS [dbo];

