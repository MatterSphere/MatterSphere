

CREATE FUNCTION[dbo].[GetClientDeleteRecord] ( )
RETURNS TABLE
RETURN

SELECT TOP 1 * FROM dbo.dbClientDelete WHERE ExecutionStatus = 1

GO
GRANT UPDATE
    ON OBJECT::[dbo].[GetClientDeleteRecord] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[GetClientDeleteRecord] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[GetClientDeleteRecord] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[GetClientDeleteRecord] TO [OMSApplicationRole]
    AS [dbo];

