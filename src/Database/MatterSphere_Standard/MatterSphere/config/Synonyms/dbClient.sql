CREATE SYNONYM [config].[dbClient] FOR [dbo].[dbClient];




GO
GRANT UPDATE
    ON OBJECT::[config].[dbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[dbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[dbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[dbClient] TO [OMSApplicationRole]
    AS [dbo];

