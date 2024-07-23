CREATE SYNONYM [config].[dbFile] FOR [dbo].[dbFile];




GO
GRANT UPDATE
    ON OBJECT::[config].[dbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[dbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[dbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[dbFile] TO [OMSApplicationRole]
    AS [dbo];

