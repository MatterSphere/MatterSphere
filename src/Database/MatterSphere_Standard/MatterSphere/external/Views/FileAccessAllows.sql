
CREATE VIEW [external].[FileAccessAllows]
AS 

SELECT  fileID as ID, clID as ClientID FROM [external].vwdbFile


GO
GRANT UPDATE
    ON OBJECT::[external].[FileAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[FileAccessAllows] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[FileAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[FileAccessAllows] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[FileAccessAllows] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[FileAccessAllows] TO [OMSApplicationRole]
    AS [dbo];

