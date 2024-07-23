CREATE PROCEDURE search.ESGetDocProcessDetailBYProcessID
@ProcessId INT
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

SELECT Sys_ErrorCode
	, COUNT(*) AS [Count]
FROM search.ESIndexDocumentLog
WHERE ESIndexProcessID = @ProcessId
	AND ErrB = 1
GROUP BY Sys_ErrorCode
