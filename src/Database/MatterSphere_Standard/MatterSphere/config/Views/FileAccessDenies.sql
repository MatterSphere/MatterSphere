

CREATE VIEW [config].[FileAccessDenies]
AS
	SELECT     c.FileID AS ID
	FROM         
		config.dbFile AS C JOIN
		config.FileAccess() AS CA ON c.FileID = CA.FileID



GO
GRANT UPDATE
    ON OBJECT::[config].[FileAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[FileAccessDenies] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[FileAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[FileAccessDenies] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[FileAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[FileAccessDenies] TO [OMSApplicationRole]
    AS [dbo];

