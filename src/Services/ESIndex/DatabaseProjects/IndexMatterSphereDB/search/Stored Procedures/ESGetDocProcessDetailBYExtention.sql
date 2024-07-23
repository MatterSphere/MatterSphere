CREATE PROCEDURE search.ESGetDocProcessDetailBYExtention
	@ProcessId INT
	, @Sys_ErrorCode NVARCHAR(MAX) 
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

SELECT docExtension
	, COUNT(*) AS [Count]
FROM search.ESIndexDocumentLog
WHERE ESIndexProcessID = @ProcessId
	AND ErrB = 1
	AND Sys_ErrorCode = @Sys_ErrorCode
GROUP BY docExtension
