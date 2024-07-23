CREATE PROCEDURE search.AddFailedToReindex
AS
SET NOCOUNT ON
	
UPDATE search.ChangeVersionControl SET ReindexFailedItems = 1
