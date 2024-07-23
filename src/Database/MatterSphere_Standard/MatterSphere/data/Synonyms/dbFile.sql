CREATE SYNONYM [data].[dbFile] FOR [dbo].[dbFile];




GO
GRANT UPDATE
    ON OBJECT::[data].[dbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[data].[dbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[data].[dbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[data].[dbFile] TO [OMSApplicationRole]
    AS [dbo];

