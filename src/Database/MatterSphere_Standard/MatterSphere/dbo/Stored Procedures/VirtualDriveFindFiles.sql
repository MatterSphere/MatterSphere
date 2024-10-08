CREATE PROCEDURE [dbo].[VirtualDriveFindFiles]
(
	@folderPath NVARCHAR(255),
	@clNo NVARCHAR(12),
	@fileNo NVARCHAR(20),
	@version BIT = 0
)
AS
SET NOCOUNT ON

CREATE TABLE #TEMP
(
	FileName NVARCHAR(255),
	IsDirectory BIT,
	DocumentID BIGINT,
	LastAccessTime DATETIME,
	LastWriteTime DATETIME,
	CreationTime DATETIME,
	FilePath NVARCHAR(255)
)

IF (ISNULL(@clNo,'') = '' AND ISNULL(@fileNo,'') = '' AND ISNULL(@folderPath,'') <> '')
BEGIN
	DECLARE @favourites TABLE(clNo NVARCHAR(12), fileNo NVARCHAR(20))

	IF (@folderPath = '\ALL')
	BEGIN
		IF (@version = 1)
		BEGIN
			INSERT INTO #TEMP (FileName, IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime)
			SELECT CAST(C.clNo  + '_' + (CASE WHEN RIGHT(RTRIM(C.clName), 1) = '.' THEN LEFT(RTRIM(C.clName), LEN(RTRIM(C.clName))-1) ELSE RTRIM(C.clName) END) AS NVARCHAR(255)) AS FileName,
					CAST(1 AS BIT) AS IsDirectory, CAST(0 AS BIGINT) AS DocumentID,
					GETUTCDATE() AS LastAccessTime, C.Updated AS LastWriteTime, C.Created AS CreationTime
			FROM dbClient C
		END
		ELSE
		BEGIN
			INSERT INTO #TEMP (FileName, IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime)
			SELECT CAST(C.clNo AS NVARCHAR(255)) AS FileName, CAST(1 AS BIT) AS IsDirectory, CAST(0 AS BIGINT) AS DocumentID,
					GETUTCDATE() AS LastAccessTime, C.Updated AS LastWriteTime, C.Created AS CreationTime
			FROM dbClient C
		END
	END
	ELSE IF (@folderPath = '\CLIENT FAVOURITES')
	BEGIN
		INSERT INTO @favourites (clNo)
		SELECT usrFavObjParam2
		FROM dbUserFavourites UF
			INNER JOIN dbUser U ON UF.usrID = U.usrID AND U.usrADID = config.GetUserLogin()
		WHERE UF.usrFavType = 'CLINETFT' AND UF.usrFavDesc = 'MYFAV'

		IF (@version = 1)
		BEGIN
			INSERT INTO #TEMP (FileName, IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime)
			SELECT CAST(C.clNo  + '_' + (CASE WHEN RIGHT(RTRIM(C.clName), 1) = '.' THEN LEFT(RTRIM(C.clName), LEN(RTRIM(C.clName))-1) ELSE RTRIM(C.clName) END) AS NVARCHAR(255)) AS FileName,
					CAST(1 AS BIT) AS IsDirectory, CAST(0 AS BIGINT) AS DocumentID,
					GETUTCDATE() AS LastAccessTime, C.Updated AS LastWriteTime, C.Created AS CreationTime
			FROM dbClient C
				INNER JOIN @favourites FV ON FV.clNo = C.clNo
		END
		ELSE
		BEGIN
			INSERT INTO #TEMP (FileName, IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime)
			SELECT CAST(C.clNo  AS NVARCHAR(255)) AS FileName, CAST(1 AS BIT) AS IsDirectory, CAST(0 AS BIGINT) AS DocumentID,
					GETUTCDATE() AS LastAccessTime, C.Updated AS LastWriteTime, C.Created AS CreationTime
			FROM dbClient C
				INNER JOIN @favourites FV ON FV.clNo = C.clNo
		END
	END
	ELSE IF (@folderPath = '\MATTER FAVOURITES')
	BEGIN
		INSERT INTO @favourites (clNo, fileNo)
		SELECT usrFavObjParam2, usrFavObjParam3
		FROM dbUserFavourites UF
			INNER JOIN dbUser U ON UF.usrID = U.usrID AND U.usrADID = config.GetUserLogin()
		WHERE UF.usrFavType = 'CLINETFILEFT' AND UF.usrFavDesc = 'MYFAV'

		IF (@version = 1)
		BEGIN
			INSERT INTO #TEMP (FileName, IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime)
			SELECT CAST(LEFT(C.clNo + '-' + F.FileNo  + '_' + (CASE WHEN RIGHT(RTRIM(F.fileDesc), 1) = '.' THEN LEFT(RTRIM(F.fileDesc), LEN(RTRIM(F.fileDesc))-1) ELSE RTRIM(F.fileDesc) END), 240) AS NVARCHAR(255)) AS FileName,
					CAST(1 AS BIT) AS IsDirectory, CAST(0 AS BIGINT) AS DocumentID,
					GETUTCDATE() AS LastAccessTime, F.Updated AS LastWriteTime, F.Created AS CreationTime
			FROM dbFile F
				INNER JOIN dbClient C ON C.clID = F.clID
				INNER JOIN @favourites FV ON FV.clNo = C.clNo AND FV.fileNo = F.fileNo
		END
		ELSE
		BEGIN
			INSERT INTO #TEMP (FileName, IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime)
			SELECT CAST(LEFT(C.clNo + '-' + F.FileNo, 240) AS NVARCHAR(255)) AS FileName, CAST(1 AS BIT) AS IsDirectory, CAST(0 AS BIGINT) AS DocumentID,
					GETUTCDATE() AS LastAccessTime, F.Updated AS LastWriteTime, F.Created AS CreationTime
			FROM dbFile F
				INNER JOIN dbClient C ON C.clID = F.clID
				INNER JOIN @favourites FV ON FV.clNo = C.clNo AND FV.fileNo = F.fileNo
		END
	END
	ELSE IF (@folderPath = '\CLIENT LAST10')
	BEGIN
		INSERT INTO @favourites (clNo)
		SELECT usrFavObjParam2
		FROM dbUserFavourites UF
			INNER JOIN dbUser U ON UF.usrID = U.usrID AND U.usrADID = config.GetUserLogin()
		WHERE UF.usrFavType = 'CLINETFT' AND UF.usrFavDesc = 'LAST10'

		IF (@version = 1)
		BEGIN
			INSERT INTO #TEMP (FileName, IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime)
			SELECT CAST(C.clNo  + '_' + (CASE WHEN RIGHT(RTRIM(C.clName), 1) = '.' THEN LEFT(RTRIM(C.clName), LEN(RTRIM(C.clName))-1) ELSE RTRIM(C.clName) END) AS NVARCHAR(255)) AS FileName,
					CAST(1 AS BIT) AS IsDirectory, CAST(0 AS BIGINT) AS DocumentID,
					GETUTCDATE() AS LastAccessTime, C.Updated AS LastWriteTime, C.Created AS CreationTime
			FROM dbClient C
				INNER JOIN @favourites FV ON FV.clNo = C.clNo
		END
		ELSE
		BEGIN
			INSERT INTO #TEMP (FileName, IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime)
			SELECT CAST(C.clNo AS NVARCHAR(255)) AS FileName, CAST(1 AS BIT) AS IsDirectory, CAST(0 AS BIGINT) AS DocumentID,
					GETUTCDATE() AS LastAccessTime, C.Updated AS LastWriteTime, C.Created AS CreationTime
			FROM dbClient C
				INNER JOIN @favourites FV ON FV.clNo = C.clNo
		END
	END
	ELSE IF (@folderPath = '\MATTER LAST10')
	BEGIN
		INSERT INTO @favourites (clNo, fileNo)
		SELECT usrFavObjParam2, usrFavObjParam3
		FROM dbUserFavourites UF
			INNER JOIN dbUser U ON UF.usrID = U.usrID AND U.usrADID = config.GetUserLogin()
		WHERE UF.usrFavType = 'CLINETFILEFT' AND UF.usrFavDesc = 'LAST10'

		IF (@version = 1)
		BEGIN
			INSERT INTO #TEMP (FileName, IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime)
			SELECT CAST(LEFT(C.clNo + '-' + F.FileNo  + '_' + (CASE WHEN RIGHT(RTRIM(F.fileDesc), 1) = '.' THEN LEFT(RTRIM(F.fileDesc), LEN(RTRIM(F.fileDesc))-1) ELSE RTRIM(F.fileDesc) END), 240) AS NVARCHAR(255)) AS FileName,
					CAST(1 AS BIT) AS IsDirectory, CAST(0 AS BIGINT) AS DocumentID,
					GETUTCDATE() AS LastAccessTime, F.Updated AS LastWriteTime, F.Created AS CreationTime
			FROM dbFile F
				INNER JOIN dbClient C ON C.clID = F.clID
				INNER JOIN @favourites FV ON FV.clNo = C.clNo AND FV.fileNo = F.fileNo
		END
		ELSE
		BEGIN
			INSERT INTO #TEMP (FileName, IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime)
			SELECT CAST(LEFT(C.clNo + '-' + F.FileNo, 240) AS NVARCHAR(255)) AS FileName, CAST(1 AS BIT) AS IsDirectory, CAST(0 AS BIGINT) AS DocumentID,
					GETUTCDATE() AS LastAccessTime, F.Updated AS LastWriteTime, F.Created AS CreationTime
			FROM dbFile F
				INNER JOIN dbClient C ON C.clID = F.clID
				INNER JOIN @favourites FV ON FV.clNo = C.clNo AND FV.fileNo = F.fileNo
		END
	END
END
ELSE IF (ISNULL(@clNo,'') <> '' AND ISNULL(@fileNo,'') = '' AND ISNULL(@folderPath,'') = '')
BEGIN
	IF (@version = 1)
	BEGIN
		INSERT INTO #TEMP (FileName, IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime)
		SELECT CAST(LEFT(F.fileNo  + '_' + (CASE WHEN RIGHT(RTRIM(F.fileDesc), 1) = '.' THEN LEFT(RTRIM(F.fileDesc), LEN(RTRIM(F.fileDesc))-1) ELSE RTRIM(F.fileDesc) END), 240) AS NVARCHAR(255)) AS FileName,
				CAST(1 AS BIT) AS IsDirectory, CAST(0 AS BIGINT) AS DocumentID,
				GETUTCDATE() AS LastAccessTime, F.Updated AS LastWriteTime, F.Created AS CreationTime
		FROM dbFile F
			INNER JOIN dbClient C ON C.clID = F.clID
		WHERE C.clNo = @clNo
	END
	ELSE
	BEGIN
		INSERT INTO #TEMP (FileName, IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime)
		SELECT CAST(LEFT(F.fileNo, 240) AS NVARCHAR(255)) AS FileName, CAST(1 AS BIT) AS IsDirectory, CAST(0 AS BIGINT) AS DocumentID,
				GETUTCDATE() AS LastAccessTime, F.Updated AS LastWriteTime, F.Created AS CreationTime
		FROM dbFile F
			INNER JOIN dbClient C ON C.clID = F.clID
		WHERE C.clNo = @clNo
	END
END
ELSE IF (ISNULL(@clNo,'') <> '' AND ISNULL(@fileNo,'') <> '' AND ISNULL(@folderPath,'') = '')
BEGIN
	DECLARE @clID BIGINT, @fileID BIGINT
	SELECT TOP 1 @clID = C.clID, @fileID = F.fileID
	FROM dbClient C INNER JOIN dbFile F ON F.clID = C.clID
	WHERE C.clNo = @clNo AND F.fileNo = @fileNo

	INSERT INTO #TEMP (FileName, IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime, FilePath)
	SELECT CL.cdDesc AS FileName, CAST(1 AS BIT) AS IsDirectory, CAST(0 AS BIGINT) AS DocumentID,
			GETUTCDATE() AS LastAccessTime, GETUTCDATE() AS LastWriteTime, GETUTCDATE() AS CreationTime, RTRIM(LTRIM(CL.cdDesc))
	FROM
	(
		SELECT
		TR.id,
		F.C.value('@FolderCode', 'NVARCHAR(15)') AS FolderCode,
		F.C.value('@FolderGUID', 'UNIQUEIDENTIFIER') AS FolderGUID
		FROM
		(
			SELECT FT.id, CAST(FT.treeXML AS XML) AS xml
			FROM dbFileFolderTreeData FT
			WHERE FT.id = @fileID
		) TR
		OUTER APPLY TR.xml.nodes('TreeView/node/node') AS F(C)
	) T
	INNER JOIN dbCodeLookup CL ON T.FolderCode = CL.cdCode AND CL.cdUICultureInfo = '{default}' AND CL.cdType = 'DFLDR_MATTER'

	INSERT INTO #TEMP (FileName, IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime, FilePath)
	SELECT REPLACE(RTRIM(LTRIM(D.docDesc)),'.' + REPLACE(D.docExtension, '.' , ''), '') + '.' + REPLACE(D.docExtension, '.' , '') AS FileName,
			CAST(0 AS BIT) AS IsDirectory, D.docID AS DocumentID,
			D.Updated AS LastAccessTime, D.Updated AS LastWriteTime, D.Created AS CreationTime, R.dirPath + '\' + D.docFileName AS FilePath
	FROM dbDocument D
		INNER JOIN dbDirectory R ON R.dirID = d.docdirID
	WHERE D.fileID = @fileID AND D.clID = @clID AND D.docDeleted = 0 AND D.docFolderGUID IS NULL AND (RTRIM(LTRIM(ISNULL(D.docFileName,''))) <> '') AND (RTRIM(LTRIM(ISNULL(D.docDesc, ''))) <> '')
END
ELSE IF (ISNULL(@clNo,'') <> '' AND ISNULL(@fileNo,'') <> '' AND ISNULL(@folderPath,'') <> '')
BEGIN
	DECLARE @xml XML
	SELECT TOP 1 @clID = C.clID, @fileID = F.fileID, @xml = CAST(FT.treeXML AS XML)
	FROM dbFileFolderTreeData FT
		INNER JOIN dbFile F ON F.fileID = FT.id
		INNER JOIN dbClient C ON F.clID = C.clID
	WHERE C.clNo = @clNo AND F.fileNo = @fileNo
	;
	WITH CTE_ITEMS (Folders, [FolderGUID], [Path], Level, Parent, RawPath)
	AS
	(
		SELECT
			VIRT.node.query('./*') AS [Folders],
			VIRT.node.value('@FolderGUID', 'UNIQUEIDENTIFIER') AS [FolderGUID],
			CAST('\' AS nvarchar(max)) AS [Path],
			0 AS [Level],
			CAST('' AS nvarchar(max)) AS [Parent],
			CAST('' AS nvarchar(1000)) AS [RawPath]
			FROM @xml.nodes('/TreeView/node') AS VIRT(node)
		UNION ALL
		SELECT
			VIRT.node.query('./*') AS [Folders],
			VIRT.node.value('@FolderGUID', 'UNIQUEIDENTIFIER') AS [FolderGUID],
			[Path] +
				CASE [Path] WHEN '\' THEN '' ELSE + '\' END +
				dbo.GetCodeLookupDesc('DFLDR_MATTER', VIRT.node.value('@FolderCode', 'nvarchar(15)'),'{default}') AS [Path],
			Level + 1 AS [Level],
			[Path] AS [Parent],
			dbo.GetCodeLookupDesc('DFLDR_MATTER', VIRT.node.value('@FolderCode', 'nvarchar(15)'),'{default}') AS [RawPath]
			FROM
		CTE_ITEMS CROSS APPLY Folders.nodes('./node') AS VIRT(node)
	)
	SELECT C.Parent, C.Path, C.FolderGUID, C.RawPath
	INTO #TempFoldersTable
	FROM CTE_ITEMS C ORDER BY [Path]

	INSERT INTO #TEMP (FileName, IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime, FilePath)
	SELECT T.RawPath AS FileName, CAST(1 AS BIT) AS IsDirectory, CAST(0 AS BIGINT) AS DocumentID,
			GETUTCDATE() AS LastAccessTime, GETUTCDATE() AS LastWriteTime, GETUTCDATE() AS CreationTime, RTRIM(LTRIM(T.RawPath)) AS FilePath
	FROM #TempFoldersTable T
	WHERE Parent = @folderPath

	INSERT INTO #TEMP (FileName, IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime, FilePath)
	SELECT REPLACE(RTRIM(LTRIM(D.docDesc)),'.' + REPLACE(D.docExtension, '.' , ''), '') + '.' + REPLACE(D.docExtension, '.' , '') AS FileName,
			CAST(0 AS BIT) AS IsDirectory, D.docID AS DocumentID,
			D.Updated AS LastAccessTime, D.Updated AS LastWriteTime, D.Created AS CreationTime, R.dirPath + '\' + D.docFileName AS FilePath
	FROM dbDocument D
		INNER JOIN dbDirectory R ON R.dirID = d.docdirID
		INNER JOIN #TempFoldersTable T ON T.FolderGUID = D.docFolderGUID AND Path = @folderPath
	WHERE D.fileID = @fileID AND D.clID = @clID AND D.docDeleted = 0 AND (RTRIM(LTRIM(ISNULL(D.docFileName,''))) <> '') AND (RTRIM(LTRIM(ISNULL(D.docDesc, ''))) <> '')

	DROP TABLE #TempFoldersTable
END

SELECT [dbo].[VirtualDriveReplaceInvalidCharsInFileName](FileName) AS [FileName], IsDirectory, DocumentID, LastAccessTime, LastWriteTime, CreationTime, FilePath
FROM #TEMP ORDER BY [FileName]

DROP TABLE #TEMP
GO

GRANT EXECUTE
	ON OBJECT::[dbo].[VirtualDriveFindFiles] TO [OMSRole]
	AS [dbo];

GO
GRANT EXECUTE
	ON OBJECT::[dbo].[VirtualDriveFindFiles] TO [OMSAdminRole]
	AS [dbo];
