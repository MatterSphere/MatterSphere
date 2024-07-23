

CREATE VIEW [config].[AssociateAccessDenies]
AS 

	SELECT     A.AssocID AS ID
            FROM
		config.dbAssociates A JOIN 
		config.dbContact AS C ON A.ContID = C.ContID LEFT OUTER JOIN
		config.ContactAccess() AS CA ON C.ContID = CA.ContactID
	WHERE     
		(CA.[Deny] = 1 and ca.secure is null)


GO
GRANT UPDATE
    ON OBJECT::[config].[AssociateAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[AssociateAccessDenies] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[AssociateAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[AssociateAccessDenies] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[AssociateAccessDenies] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[AssociateAccessDenies] TO [OMSApplicationRole]
    AS [dbo];

