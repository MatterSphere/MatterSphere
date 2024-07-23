

CREATE VIEW[dbo].[PartialUser]
AS
SELECT        usrID AS ID, usrFullName AS Name, rowguid AS UniqueID
FROM            dbo.dbUser

GO
GRANT UPDATE
    ON OBJECT::[dbo].[PartialUser] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PartialUser] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PartialUser] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PartialUser] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[PartialUser] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[PartialUser] TO [OMSApplicationRole]
    AS [dbo];

