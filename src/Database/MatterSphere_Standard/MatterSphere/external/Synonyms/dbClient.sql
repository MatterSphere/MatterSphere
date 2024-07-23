CREATE SYNONYM [external].[dbClient] FOR [external].[vwdbClient];




GO
GRANT UPDATE
    ON OBJECT::[external].[dbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[dbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[dbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[dbClient] TO [OMSApplicationRole]
    AS [dbo];

