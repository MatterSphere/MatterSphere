CREATE PROCEDURE search.SaveIndexingInfo(@ProcName NVARCHAR(128), @ii search.IndexingInfo READONLY)
AS
SET NOCOUNT ON

DECLARE @LastIndexProcessID INT 

SELECT TOP 1 @LastIndexProcessID = Id
FROM search.ESIndexProcess 
ORDER BY Id DESC

IF @ProcName = 'Document'
	MERGE INTO search.ESIndexDocumentLog AS Target 
	USING @ii AS Source 
	ON Target.docID = Source.EntityID AND Target.ESIndexProcessID = @LastIndexProcessID
	-- update matched rows 
	WHEN MATCHED THEN 
	UPDATE SET Sys_FileName = Source.Sys_FileName
		, Sys_FileSize = Source.Sys_FileSize
		, Sys_ProcessTime = Source.Sys_ProcessTime
		, Sys_DocIndexingError = Source.Sys_DocIndexingError
		, Sys_ErrorCode = Source.Sys_ErrorCode
		, ErrB = CASE WHEN ISNULL(Source.Sys_ErrorCode, '') <> '' THEN 1 ELSE 0 END
		, docExtension = Source.docExtension
		, EmptyContent = Source.EmptyContent
		, LastProcessDate = GETDATE()
	-- insert new rows 
	WHEN NOT MATCHED BY TARGET THEN 
	INSERT (docID, ESIndexProcessID, Sys_FileName, Sys_FileSize, Sys_ProcessTime, Sys_DocIndexingError, Sys_ErrorCode, ErrB, docExtension, EmptyContent, LastProcessDate) 
	VALUES (Source.EntityID, @LastIndexProcessID, Source.Sys_FileName, Source.Sys_FileSize, Source.Sys_ProcessTime, Source.Sys_DocIndexingError, Source.Sys_ErrorCode, CASE WHEN ISNULL(Source.Sys_ErrorCode, '') <> '' THEN 1 ELSE 0 END, Source.docExtension, Source.EmptyContent, GETDATE());
ELSE IF @ProcName = 'Precedent' 
	MERGE INTO search.ESIndexPrecedentLog AS Target 
	USING @ii AS Source 
	ON Target.precID = Source.EntityID AND Target.ESIndexProcessID = @LastIndexProcessID
	-- update matched rows 
	WHEN MATCHED THEN 
	UPDATE SET Sys_FileName = Source.Sys_FileName
		, Sys_FileSize = Source.Sys_FileSize
		, Sys_ProcessTime = Source.Sys_ProcessTime
		, Sys_DocIndexingError = Source.Sys_DocIndexingError
		, Sys_ErrorCode = Source.Sys_ErrorCode
		, ErrB = CASE WHEN ISNULL(Source.Sys_ErrorCode, '') <> '' THEN 1 ELSE 0 END
		, docExtension = Source.docExtension
		, EmptyContent = Source.EmptyContent
		, LastProcessDate = GETDATE()
	-- insert new rows 
	WHEN NOT MATCHED BY TARGET THEN 
	INSERT (precID, ESIndexProcessID, Sys_FileName, Sys_FileSize, Sys_ProcessTime, Sys_DocIndexingError, Sys_ErrorCode, ErrB, docExtension, EmptyContent, LastProcessDate) 
	VALUES (Source.EntityID, @LastIndexProcessID, Source.Sys_FileName, Source.Sys_FileSize, Source.Sys_ProcessTime, Source.Sys_DocIndexingError, Source.Sys_ErrorCode, CASE WHEN ISNULL(Source.Sys_ErrorCode, '') <> '' THEN 1 ELSE 0 END, Source.docExtension, Source.EmptyContent, GETDATE());
