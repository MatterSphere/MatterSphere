CREATE PROCEDURE search.ESIndexProcessFinish
AS
SET NOCOUNT ON;
UPDATE search.ESIndexProcess
SET FinishDate = GETDATE()
WHERE Id = (SELECT TOP 1 Id FROM search.ESIndexProcess ORDER BY Id DESC)
	AND FinishDate IS NULL;