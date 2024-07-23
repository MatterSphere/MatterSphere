CREATE PROCEDURE search.ESGetDocProcessDetail
	@ProcessId INT
	, @Sys_ErrorCode NVARCHAR(MAX) 
	, @docExtension NVARCHAR(15)
	, @MAX_RECORDS INT = 50
	, @PageNo INT = 1
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @OFFSET INT = 0
	, @Total INT
	, @TOP INT 

DECLARE @Res TABLE(Id INT IDENTITY(1, 1) PRIMARY KEY, ESIndexProcessId INT, DocID BIGINT)

IF @MAX_RECORDS > 0
	SET @TOP = @MAX_RECORDS
ELSE
	SET @TOP = 50

IF @PageNo IS NULL
	SET @OFFSET = 0
ELSE
	SET @OFFSET = @TOP * (@PageNo- 1)


INSERT INTO @Res(ESIndexProcessId, DocID)
SELECT ESIndexProcessID
	, docID
FROM search.ESIndexDocumentLog
WHERE ESIndexProcessID = @ProcessId
	AND ErrB = 1
	AND Sys_ErrorCode = @Sys_ErrorCode
	AND docExtension = @docExtension
ORDER BY DocID

SET @Total = @@ROWCOUNT

SELECT TOP (@TOP) 
	b.docID
	, doc.docDesc AS [Name]
	, [Sys_FileName]
	, [Sys_FileSize]
	, [Sys_ProcessTime]
	, [Sys_DocIndexingError]
	, b.[docExtension]
	, [EmptyContent]
FROM search.ESIndexDocumentLog b
	INNER JOIN @Res r ON r.ESIndexProcessId = b.ESIndexProcessID AND r.DocID = b.docID
	INNER JOIN config.dbDocument doc WITH(NOLOCK) ON doc.docID = b.docID
WHERE r.Id > @OFFSET
ORDER BY r.Id