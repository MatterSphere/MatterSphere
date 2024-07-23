

CREATE VIEW[dbo].[AssociateTypeResource]
AS
SELECT        cdCode AS Code, cdDesc AS Description
FROM            dbo.dbCodeLookup
WHERE        (cdType = N'SUBASSOC') AND (cdUICultureInfo = '{default}')


GO
GRANT UPDATE
    ON OBJECT::[dbo].[AssociateTypeResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AssociateTypeResource] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AssociateTypeResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AssociateTypeResource] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[AssociateTypeResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[AssociateTypeResource] TO [OMSApplicationRole]
    AS [dbo];

