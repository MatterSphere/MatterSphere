CREATE PROCEDURE [dbo].[GetMatterDocumentFolderList]
	@UI NVARCHAR(15) = NULL
	, @fileID BIGINT = NULL
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED

SELECT NULL
	, dbo.GetCodeLookupDesc('RESOURCE','RESNOTSET', @UI)
	, NULL
UNION ALL
SELECT fc.FolderGUID
	, COALESCE(CL.cdDesc, '~' +  NULLIF(fc.FolderCode, '') + '~') 
	, fc.FolderCode
FROM dbo.dbFileFolder fc
	LEFT JOIN [dbo].[GetCodeLookupDescription] ( 'DFLDR_MATTER', @ui ) CL ON CL.[cdCode] = fc.FolderCode
WHERE fileID = @fileID
