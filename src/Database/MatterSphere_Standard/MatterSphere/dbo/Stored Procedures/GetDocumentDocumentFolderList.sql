CREATE PROCEDURE [dbo].[GetDocumentDocumentFolderList]
	@UI NVARCHAR(15) = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

WITH List AS
(
	SELECT
		FolderCode
		, STUFF((SELECT N''',' + CAST(FolderGUID AS NVARCHAR(MAX)) + N''''  FROM dbFileFolder g WHERE g.FolderCode = fc.FolderCode FOR XML PATH(''), TYPE).value('.','NVARCHAR(MAX)'), 1, 2, '') AS FolderGUIDs
	FROM dbo.dbFileFolder fc
	GROUP BY FolderCode
)
SELECT NULL
	, dbo.GetCodeLookupDesc('RESOURCE','RESNOTSET',@UI)
UNION ALL
SELECT	
	l.FolderGUIDs
	, CAST(COALESCE(cl1.cdDesc, '~' + NULLIF(l.FolderCode, '') + '~') AS NVARCHAR(100)) AS FolderDescription 
FROM List l
	LEFT OUTER JOIN dbo.GetCodeLookupDescription('DFLDR_MATTER', @UI) cl1 ON cl1.cdCode = l.FolderCode
