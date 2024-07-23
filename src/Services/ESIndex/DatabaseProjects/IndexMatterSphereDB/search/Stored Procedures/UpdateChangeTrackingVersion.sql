CREATE PROCEDURE search.UpdateChangeTrackingVersion
AS 
SET NOCOUNT ON;
UPDATE search.ChangeVersionControl SET LastCopiedVersion = WorkingVersion, FullCopyRequired = 0, ReindexFailedItems = CASE ReindexFailedItems WHEN 2 THEN 0 ELSE ReindexFailedItems END;
UPDATE search.ESIndexTable SET FullCopyRequired =  0
DELETE search.ExtensionToReindex;
