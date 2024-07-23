

CREATE VIEW[dbo].[FileTypeResource]
AS
SELECT        cdCode AS Code, cdDesc AS Description
FROM            dbo.dbCodeLookup
WHERE        (cdType = N'FILETYPE') AND (cdUICultureInfo = '{default}')

GO
GRANT UPDATE
    ON OBJECT::[dbo].[FileTypeResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[FileTypeResource] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[FileTypeResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[FileTypeResource] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[FileTypeResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[FileTypeResource] TO [OMSApplicationRole]
    AS [dbo];

