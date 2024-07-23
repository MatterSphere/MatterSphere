

CREATE VIEW[dbo].[ClientTypeResource]
AS
SELECT        cdCode AS Code, cdDesc AS Description
FROM            dbo.dbCodeLookup
WHERE        (cdType = N'CLTYPE') AND (cdUICultureInfo = '{default}')

GO
GRANT UPDATE
    ON OBJECT::[dbo].[ClientTypeResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ClientTypeResource] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ClientTypeResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ClientTypeResource] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ClientTypeResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ClientTypeResource] TO [OMSApplicationRole]
    AS [dbo];

