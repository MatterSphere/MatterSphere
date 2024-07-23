CREATE FUNCTION [dbo].[GetFolderCodesForMatterDocumentTree] (@fileID BIGINT)
RETURNS TABLE
AS
RETURN
(
SELECT
	CAST(fc.FolderCode AS NVARCHAR(15)) AS FolderCode
	, CAST(fc.FolderGuid AS UNIQUEIDENTIFIER) AS FolderGUID
	, NULL AS FolderDescription 
FROM dbo.dbFileFolder fc WITH(NOLOCK) 
WHERE fileID = @fileID
)
GO