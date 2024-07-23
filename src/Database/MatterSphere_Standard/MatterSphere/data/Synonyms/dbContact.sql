CREATE SYNONYM [data].[dbContact] FOR [dbo].[dbContact];




GO
GRANT UPDATE
    ON OBJECT::[data].[dbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[data].[dbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[data].[dbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[data].[dbContact] TO [OMSApplicationRole]
    AS [dbo];

