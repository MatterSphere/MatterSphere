CREATE PROCEDURE search.GetNewWorkingVersion
AS 
SET NOCOUNT ON

UPDATE search.ChangeVersionControl SET WorkingVersion = CHANGE_TRACKING_CURRENT_VERSION();
GO
