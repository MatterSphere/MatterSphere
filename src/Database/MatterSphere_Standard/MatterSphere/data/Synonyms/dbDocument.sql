CREATE SYNONYM [data].[dbDocument] FOR [dbo].[dbDocument];




GO
GRANT UPDATE
    ON OBJECT::[data].[dbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[data].[dbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[data].[dbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[data].[dbDocument] TO [OMSApplicationRole]
    AS [dbo];

