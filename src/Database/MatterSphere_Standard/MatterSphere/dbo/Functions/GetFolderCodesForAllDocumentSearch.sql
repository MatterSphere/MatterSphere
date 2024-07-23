CREATE FUNCTION [dbo].[GetFolderCodesForAllDocumentSearch] (@UI NVARCHAR(15))
RETURNS TABLE
AS
RETURN
(
SELECT
	CAST(fc.FolderCode AS NVARCHAR(15)) AS FolderCode
	, CAST(fc.FolderGuid AS UNIQUEIDENTIFIER) AS FolderGUID
	, CAST(COALESCE(cl1.cdDesc, '~' + NULLIF(fc.FolderCode, '') + '~') AS NVARCHAR(100)) AS FolderDescription 
FROM dbo.dbFileFolder fc WITH(NOLOCK) 
	LEFT JOIN [dbo].[GetCodeLookupDescription]('DFLDR_MATTER', @UI) cl1 ON cl1.cdCode = fc.FolderCode
)

