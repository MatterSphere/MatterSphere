CREATE SYNONYM [config].[dbDocument] FOR [dbo].[dbDocument];




GO
GRANT UPDATE
    ON OBJECT::[config].[dbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[dbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[dbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[dbDocument] TO [OMSApplicationRole]
    AS [dbo];

