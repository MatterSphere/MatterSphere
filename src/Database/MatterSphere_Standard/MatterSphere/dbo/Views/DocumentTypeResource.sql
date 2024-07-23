

CREATE VIEW [dbo].[DocumentTypeResource]
AS
SELECT        cdCode AS Code, cdDesc AS Description
FROM            dbo.dbCodeLookup
WHERE        (cdType = N'DOCTYPE') AND (cdUICultureInfo = '{default}')

GO
GRANT UPDATE
    ON OBJECT::[dbo].[DocumentTypeResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DocumentTypeResource] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DocumentTypeResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DocumentTypeResource] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[DocumentTypeResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[DocumentTypeResource] TO [OMSApplicationRole]
    AS [dbo];

