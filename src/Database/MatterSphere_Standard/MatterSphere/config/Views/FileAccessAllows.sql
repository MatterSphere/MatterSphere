
CREATE VIEW [config].[FileAccessAllows]
AS 

SELECT  fileID as ID, clID as ClientID FROM config.vwdbFile


GO
GRANT UPDATE
    ON OBJECT::[config].[FileAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[FileAccessAllows] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[FileAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[FileAccessAllows] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[FileAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[FileAccessAllows] TO [OMSApplicationRole]
    AS [dbo];

