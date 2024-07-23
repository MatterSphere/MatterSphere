

CREATE VIEW[dbo].[vwDBActivities]
AS
SELECT     actCode AS ActivityCode, actAccCode AS ActivityAccCode, COALESCE(CL1.cdDesc, '~' + NULLIF(actCode, '') + '~') AS ActivityDesc, 
                      actChargeable AS ActivityChargeable, actFixedRateLegal AS ActivityFixedRate, actTemplateMatch AS ActivityTemplateMatch, 
                      actActive AS ActivityActive, actFixedValue AS ActivityFixedValue
FROM         dbo.dbActivities
LEFT JOIN 
	dbo.GetCodeLookupDescription('TIMEACTCODE', 'en-gb') CL1 ON CL1.cdCode = actCode

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBActivities] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBActivities] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBActivities] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBActivities] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBActivities] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBActivities] TO [OMSApplicationRole]
    AS [dbo];

