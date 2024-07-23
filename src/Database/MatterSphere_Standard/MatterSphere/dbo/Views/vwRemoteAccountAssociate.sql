

CREATE VIEW[dbo].[vwRemoteAccountAssociate]
AS
SELECT        A.assocRef AS AssociateReference, A.fileID, A.assocID
FROM            config.dbAssociates AS A INNER JOIN
                         config.dbContact AS C ON A.contID = C.contID INNER JOIN
                         dbo.dbUser AS U ON C.userID = U.usrID
WHERE        (U.usrADID = config.GetUserLogin()) AND (A.assocActive = 1)


GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwRemoteAccountAssociate] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwRemoteAccountAssociate] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwRemoteAccountAssociate] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwRemoteAccountAssociate] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwRemoteAccountAssociate] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwRemoteAccountAssociate] TO [OMSApplicationRole]
    AS [dbo];

