

CREATE VIEW [config].[ClientAccessDenies]
AS
	SELECT     C.clid AS ID
	FROM         
		config.dbClient AS C LEFT OUTER JOIN
		config.ClientAccess() AS CA ON c.clID = CA.ClID		
	WHERE     
		(CA.[Deny] = 1 and ca.secure is null)


GO
GRANT UPDATE
    ON OBJECT::[config].[ClientAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[ClientAccessDenies] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[ClientAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[ClientAccessDenies] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[ClientAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[ClientAccessDenies] TO [OMSApplicationRole]
    AS [dbo];

