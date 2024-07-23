CREATE SYNONYM [config].[dbContact] FOR [dbo].[dbContact];




GO
GRANT UPDATE
    ON OBJECT::[config].[dbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[dbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[dbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[dbContact] TO [OMSApplicationRole]
    AS [dbo];

