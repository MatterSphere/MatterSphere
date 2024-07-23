CREATE PROCEDURE search.UpdateChangeTrackingVersion
AS 
SET NOCOUNT ON;
UPDATE search.ChangeVersionControl SET LastCopiedVersion = WorkingVersion, FullCopyRequired = 0;
UPDATE search.IndexedEntity SET FullCopyRequired =  0
