
CREATE VIEW [external].[ContactAccessAllows]
AS 

SELECT  contID as ID FROM [external].vwdbContact


GO
GRANT UPDATE
    ON OBJECT::[external].[ContactAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[ContactAccessAllows] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[ContactAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[ContactAccessAllows] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[ContactAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[ContactAccessAllows] TO [OMSApplicationRole]
    AS [dbo];

