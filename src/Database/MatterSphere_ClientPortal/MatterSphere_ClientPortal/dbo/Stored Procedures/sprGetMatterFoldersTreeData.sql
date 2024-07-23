CREATE PROCEDURE [dbo].[sprGetMatterFoldersTreeData]( @Id BIGINT, @UI uUICultureInfo = N'en-gb')
AS
SET NOCOUNT ON
DECLARE @TXML VARCHAR(MAX)
	, @XML XML
	, @FolderCode NVARCHAR(MAX) = ''
	, @FolderL NVARCHAR(MAX) = ''
	, @FolderGUID NVARCHAR(MAX)
	, @DocCount INT
	, @I INT

SET @TXML = (SELECT treeXML FROM dbFileFolderTreeData WHERE id = @Id);
SET @XML = @TXML;

DECLARE @Repl TABLE (I INT IDENTITY(1, 1) PRIMARY KEY, FolderCode NVARCHAR(15), FolderGUID UNIQUEIDENTIFIER, DocCount INT)

INSERT INTO @Repl (FolderCode, FolderGUID)
SELECT DISTINCT
	xmlTable.xmlCol.value('@FolderCode', 'varchar(15)') as FolderCode
	, xmlTable.xmlCol.value('@FolderGUID', 'uniqueidentifier') as FolderGUID
FROM @XML.nodes('//node') xmlTable(xmlCol)
WHERE xmlTable.xmlCol.value('@FolderCode', 'varchar(15)') IS NOT NULL;

UPDATE r
	SET DocCount = ISNULL(C.DocCount, 0)
FROM @Repl r
	LEFT JOIN (SELECT docFolderGUID, COUNT(*) AS DocCount FROM [external].dbDocument WHERE fileId = @Id GROUP BY docFolderGUID) C ON c.docFolderGUID = r.FolderGUID

SET @I = (SELECT TOP 1 I FROM @Repl ORDER BY I)

WHILE @I IS NOT NULL
BEGIN
	SELECT @FolderCode = FolderCode, @FolderGUID = FolderGUID, @DocCount = DocCount FROM @Repl WHERE I = @I
	SET @FolderL = dbo.GetCodeLookupDesc(N'DFLDR_MATTER', @FolderCode, @UI);
	SET @TXML = REPLACE(@TXML, @FolderCode, @FolderL);
	SET @TXML = REPLACE(@TXML, @FolderGUID, CAST(@FolderGUID AS NVARCHAR(MAX)) + N'" DocCount="' + CAST(@DocCount AS NVARCHAR(MAX)));
	SET @I = (SELECT TOP 1 I FROM @Repl WHERE I > @I ORDER BY I)
END
SET @TXML = REPLACE(@TXML, 'FolderCode', 'FolderName');
SELECT CAST(@TXML AS XML) AS Tree
