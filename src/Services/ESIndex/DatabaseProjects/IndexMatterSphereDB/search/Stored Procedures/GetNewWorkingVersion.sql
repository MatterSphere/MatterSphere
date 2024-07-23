CREATE PROCEDURE search.GetNewWorkingVersion
AS 
SET NOCOUNT ON

UPDATE search.ChangeVersionControl SET WorkingVersion = CHANGE_TRACKING_CURRENT_VERSION(), ReindexFailedItems = CASE ReindexFailedItems WHEN 1 THEN 2 ELSE ReindexFailedItems END;
UPDATE search.ExtensionToReindex SET Flag = 1

GO
