CREATE SYNONYM [external].[dbContact] FOR [external].[vwdbContact];




GO
GRANT UPDATE
    ON OBJECT::[external].[dbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[dbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[dbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[dbContact] TO [OMSApplicationRole]
    AS [dbo];

