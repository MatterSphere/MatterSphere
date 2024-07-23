

CREATE FUNCTION[dbo].[GetContactMergeDeleteRecord] ( )
RETURNS TABLE
RETURN

SELECT TOP 1 * FROM dbo.dbContactMergeDelete WHERE ExecutionStatus = 1

GO
GRANT UPDATE
    ON OBJECT::[dbo].[GetContactMergeDeleteRecord] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[GetContactMergeDeleteRecord] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[GetContactMergeDeleteRecord] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[GetContactMergeDeleteRecord] TO [OMSApplicationRole]
    AS [dbo];

