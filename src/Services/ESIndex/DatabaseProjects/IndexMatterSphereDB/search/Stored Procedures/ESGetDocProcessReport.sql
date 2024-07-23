CREATE PROCEDURE [search].[ESGetDocProcessReport]
	@ProcessId INT
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

SELECT 
	b.docID
	, doc.docDesc AS [Name]
	, [Sys_FileName]
	, [Sys_FileSize]
	, [Sys_ProcessTime]
	, [Sys_DocIndexingError]
	, [Sys_ErrorCode]
	, b.[docExtension]
	, [EmptyContent]
FROM search.ESIndexDocumentLog b	
	LEFT OUTER JOIN config.dbDocument doc WITH(NOLOCK) ON doc.docID = b.docID
WHERE b.ErrB = 1 AND b.ESIndexProcessID = @ProcessId
ORDER BY b.docID
