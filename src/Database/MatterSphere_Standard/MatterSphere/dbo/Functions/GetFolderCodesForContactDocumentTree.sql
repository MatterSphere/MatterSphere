CREATE FUNCTION [dbo].[GetFolderCodesForContactDocumentTree] (@contID BIGINT, @UI NVARCHAR(15))
RETURNS TABLE
AS
RETURN
(
SELECT
	CAST(fc.FolderCode AS NVARCHAR(15)) AS FolderCode
	, CAST(fc.FolderGuid AS UNIQUEIDENTIFIER) AS FolderGUID
	, CAST(COALESCE(cl1.cdDesc, '~' + NULLIF(fc.FolderCode, '') + '~') AS NVARCHAR(100)) AS FolderDescription 
FROM dbo.dbFileFolder fc WITH(NOLOCK) 
	LEFT JOIN dbo.GetCodeLookupDescription('DFLDR_MATTER', @UI) cl1 ON cl1.cdCode = fc.FolderCode
WHERE EXISTS(SELECT 1 FROM dbo.dbfile f INNER JOIN dbo.dbAssociates a ON f.fileID = a.fileID WHERE a.contID = @contID AND f.fileID = fc.fileID)
)
GO