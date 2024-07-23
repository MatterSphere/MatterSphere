CREATE PROCEDURE search.AddExtensionToReindex
	@ExtensionsList NVARCHAR(MAX)
	, @Delimiter CHAR(1)
AS
SET NOCOUNT ON
	
INSERT INTO search.ExtensionToReindex(Extension)
SELECT CAST(items AS NVARCHAR(15))
FROM dbo.SplitStringToTable(@ExtensionsList, @Delimiter) i
WHERE NOT EXISTS(SELECT 1 FROM search.ExtensionToReindex WHERE Extension = CAST(items AS NVARCHAR(15)))
