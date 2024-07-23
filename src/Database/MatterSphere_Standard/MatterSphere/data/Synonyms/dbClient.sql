CREATE SYNONYM [data].[dbClient] FOR [dbo].[dbClient];




GO
GRANT UPDATE
    ON OBJECT::[data].[dbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[data].[dbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[data].[dbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[data].[dbClient] TO [OMSApplicationRole]
    AS [dbo];

