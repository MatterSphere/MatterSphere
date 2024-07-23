CREATE PROCEDURE dbo.GetClientDocumentFolderCode
	@UI NVARCHAR(15) = NULL,
	@clientID BIGINT = NULL
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
WHERE EXISTS(SELECT 1 FROM dbo.dbfile WHERE clID = @clientID AND fileID = fc.fileID)
