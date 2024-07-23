

CREATE VIEW [config].[ContactAccessDenies]
AS
	SELECT     ContID as ID
	FROM         
		config.dbContact AS C LEFT OUTER JOIN
		config.ContactAccess() AS CA ON c.ContID = CA.ContactID
	WHERE     
		(CA.[Deny] = 1 and ca.secure is null)

GO
GRANT UPDATE
    ON OBJECT::[config].[ContactAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[ContactAccessDenies] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[ContactAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[ContactAccessDenies] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[ContactAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[ContactAccessDenies] TO [OMSApplicationRole]
    AS [dbo];

