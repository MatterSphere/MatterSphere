CREATE SYNONYM [external].[dbFile] FOR [external].[vwdbFile];




GO
GRANT UPDATE
    ON OBJECT::[external].[dbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[dbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[dbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[dbFile] TO [OMSApplicationRole]
    AS [dbo];

