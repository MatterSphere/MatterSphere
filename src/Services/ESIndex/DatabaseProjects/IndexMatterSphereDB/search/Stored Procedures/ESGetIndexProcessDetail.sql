CREATE PROCEDURE search.ESGetIndexProcessDetail
	@ProcessId INT
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;
SELECT cpd.MessageType
	, cpd.StartDate
	, cpd.SuccessNumber
	, cpd.FailedNumber
	, cpd.Size
	, cpd.FinishDate
	, cpd.ContentReadingFailedNumber
FROM search.ESIndexProcessDetail cpd
WHERE cpd.ESIndexProcessId = @ProcessId
ORDER BY cpd.StartDate
