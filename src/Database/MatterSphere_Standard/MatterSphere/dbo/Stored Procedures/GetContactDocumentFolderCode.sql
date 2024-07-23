CREATE PROCEDURE dbo.GetContactDocumentFolderCode
	@UI NVARCHAR(15) = NULL
	, @contID BIGINT = NULL
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

SELECT NULL
	, dbo.GetCodeLookupDesc('RESOURCE','RESNOTSET', @UI)
UNION
SELECT  fc.FolderCode
	, CAST(COALESCE(cl1.cdDesc, '~' + NULLIF(fc.FolderCode, '') + '~') AS NVARCHAR(100)) AS FolderDescription 
FROM dbo.dbFileFolder fc
	LEFT JOIN dbo.GetCodeLookupDescription('DFLDR_MATTER', @UI) cl1 ON cl1.cdCode = fc.FolderCode
WHERE EXISTS(SELECT 1 FROM dbo.dbfile f INNER JOIN dbo.dbAssociates a ON f.fileID = a.fileID WHERE a.contID = @contID AND f.fileID = fc.fileID)