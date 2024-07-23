CREATE SYNONYM [external].[dbDocument] FOR [external].[vwdbDocument];




GO
GRANT UPDATE
    ON OBJECT::[external].[dbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[dbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[dbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[dbDocument] TO [OMSApplicationRole]
    AS [dbo];

