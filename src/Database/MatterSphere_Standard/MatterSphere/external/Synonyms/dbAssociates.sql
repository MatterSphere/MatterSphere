CREATE SYNONYM [external].[dbAssociates] FOR [external].[vwdbAssociates];




GO
GRANT UPDATE
    ON OBJECT::[external].[dbAssociates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[dbAssociates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[dbAssociates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[dbAssociates] TO [OMSApplicationRole]
    AS [dbo];

