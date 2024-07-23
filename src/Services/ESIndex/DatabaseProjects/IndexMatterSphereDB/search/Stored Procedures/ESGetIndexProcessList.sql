CREATE PROCEDURE search.ESGetIndexProcessList
	@DateFrom DATETIME
	, @DateTo DATETIME
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;
WITH cpd AS
(
	SELECT cpd.ESIndexProcessId
		, SUM(cpd.SuccessNumber) AS SuccessNumber
		, SUM(cpd.FailedNumber) AS FailedNumber
		, SUM(ContentReadingFailedNumber) AS ContentReadingFailedNumber
	FROM search.ESIndexProcessDetail cpd
	GROUP BY cpd.ESIndexProcessId
)
SELECT cp.Id
	, cp.StartDate
	, cp.FinishDate
	, cpd.SuccessNumber
	, cpd.FailedNumber
	, cpd.ContentReadingFailedNumber
FROM search.ESIndexProcess cp
	JOIN cpd ON cpd.ESIndexProcessId = cp.Id
WHERE cp.StartDate >= @DateFrom
	AND cp.StartDate < @DateTo
ORDER BY cp.StartDate DESC

