
	CREATE FUNCTION [dbo].[GetMatterMergeDeleteRecord] ()
	RETURNS TABLE
	RETURN

	SELECT TOP 1 * FROM dbo.dbMatterMergeDelete WHERE ExecutionStatus = 1


GO
GRANT UPDATE
    ON OBJECT::[dbo].[GetMatterMergeDeleteRecord] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[GetMatterMergeDeleteRecord] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[GetMatterMergeDeleteRecord] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[GetMatterMergeDeleteRecord] TO [OMSApplicationRole]
    AS [dbo];

