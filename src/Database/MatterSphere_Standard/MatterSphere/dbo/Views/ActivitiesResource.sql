

CREATE VIEW [dbo].[ActivitiesResource]
AS
SELECT        cdCode AS Code, cdDesc AS Description
FROM            dbo.dbCodeLookup
WHERE        (cdType = N'TIMEACTCODE') AND (cdUICultureInfo = '{default}')



GO
GRANT UPDATE
    ON OBJECT::[dbo].[ActivitiesResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ActivitiesResource] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ActivitiesResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ActivitiesResource] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ActivitiesResource] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ActivitiesResource] TO [OMSApplicationRole]
    AS [dbo];

