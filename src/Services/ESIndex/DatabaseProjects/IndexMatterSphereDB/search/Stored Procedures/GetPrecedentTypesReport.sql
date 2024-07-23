CREATE PROCEDURE [search].[GetPrecedentTypesReport]
	@documentExtension NVARCHAR(15) = NULL
	, @errorCode NVARCHAR(50) = NULL
	, @page INT = NULL
	, @pageSize INT = NULL
AS
DECLARE @SQL AS NVARCHAR(MAX)

IF @documentExtension IS NULL
	SET @SQL = N'
	SELECT d.docExtension AS Type
 		, ISNULL(SUM(CASE WHEN d.ErrB = 1 THEN 0 ELSE 1 END), 0) AS SuccessNumber
		, ISNULL(SUM(CASE WHEN d.ErrB = 1 THEN 1 ELSE 0 END), 0) AS FailedNumber
		, ISNULL(SUM(CASE WHEN EmptyContent = 1 THEN 1 END), 0) AS EmptyContentNumber
	FROM search.ESIndexPrecedentLog d WITH(NOLOCK)	
	INNER JOIN (SELECT precID, MAX(ESIndexProcessId) AS ESIndexProcessId FROM search.ESIndexPrecedentLog GROUP BY precID) m ON m.precID = d.precID AND m.ESIndexProcessId = d.ESIndexProcessId
	GROUP BY d.docExtension'
ELSE IF @errorCode IS NULL
	SET @SQL = N'
	SELECT d.[Sys_ErrorCode] AS [ErrorType]
		, COUNT(*) AS [Number]
	FROM search.ESIndexPrecedentLog d WITH(NOLOCK)
	INNER JOIN (SELECT precID, MAX(ESIndexProcessId) AS ESIndexProcessId FROM search.ESIndexPrecedentLog GROUP BY precID) m ON m.precID = d.precID AND m.ESIndexProcessId = d.ESIndexProcessId
	WHERE d.docExtension = @documentExtension
		AND d.ErrB = 1
	GROUP BY d.[Sys_ErrorCode]'
ELSE 
	SET @SQL = N'
	SELECT CAST(d.precID AS BIGINT) AS Id
		, doc.precDesc AS Name
		, d.[Sys_DocIndexingError] AS ErrorDetails
		, d.[Sys_FileName] AS [Path]
	FROM search.ESIndexPrecedentLog d WITH(NOLOCK)
		INNER JOIN dbo.dbPrecedents doc WITH(NOLOCK) ON doc.precID = d.precID
		INNER JOIN (SELECT precID, MAX(ESIndexProcessId) AS ESIndexProcessId FROM search.ESIndexPrecedentLog GROUP BY precID) m ON m.precID = d.precID AND m.ESIndexProcessId = d.ESIndexProcessId
	WHERE d.docExtension = @documentExtension
		AND d.[Sys_ErrorCode] = @errorCode
	ORDER BY d.precId OFFSET (@page-1)*@pageSize ROWS FETCH NEXT @pageSize ROWS ONLY;'

exec sp_executesql @SQL,  N'
	@documentExtension NVARCHAR(15)
	, @errorCode NVARCHAR(50)
	, @page INT
	, @pageSize INT'

	, @documentExtension
	, @errorCode
	, @page
	, @pageSize
