CREATE SYNONYM [config].[dbAssociates] FOR [dbo].[dbAssociates];




GO
GRANT UPDATE
    ON OBJECT::[config].[dbAssociates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[dbAssociates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[dbAssociates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[dbAssociates] TO [OMSApplicationRole]
    AS [dbo];

