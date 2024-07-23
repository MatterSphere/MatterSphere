

CREATE VIEW [config].[DocumentAccessDenies]
AS
	SELECT     DocID as ID
	FROM         
		config.dbDocument AS C LEFT OUTER JOIN
		config.DocumentAccess() AS CA ON c.DocID = CA.DocumentID
	WHERE     
		(CA.[Deny] = 1 and ca.secure is null)


GO
GRANT UPDATE
    ON OBJECT::[config].[DocumentAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[DocumentAccessDenies] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[DocumentAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[DocumentAccessDenies] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[DocumentAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[DocumentAccessDenies] TO [OMSApplicationRole]
    AS [dbo];

