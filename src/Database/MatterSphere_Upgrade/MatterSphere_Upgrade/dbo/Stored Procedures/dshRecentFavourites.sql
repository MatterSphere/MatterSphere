CREATE PROCEDURE dbo.dshRecentFavourites( @MAX_RECORDS INT = 50, @PageNo INT = 1, @Filter VARCHAR(2), @Query NVARCHAR(MAX) = NULL, @OrderBY NVARCHAR(MAX) = NULL)
AS
SET NOCOUNT ON
SET TRAN ISOLATION LEVEL READ UNCOMMITTED

DECLARE @Res TABLE(
	ID INT IDENTITY(1, 1) PRIMARY KEY
	, FavID INT 
	, usrFavType NVARCHAR(50) 
	, clNo NVARCHAR(12) 
	, clName NVARCHAR(128) 
	, fileNo NVARCHAR(20) 
	, fileDesc NVARCHAR(255) 
	, precDesc NVARCHAR(1000) 
	, precExtension NVARCHAR(1000) 
	, Updated DATETIME 
	, clId BIGINT 
	, fileID BIGINT 
	, PrecID BIGINT
	, combo NVARCHAR(MAX)
	, OrderForRecent INT
	) 

DECLARE @USERID NVARCHAR(200) = (SELECT config.GetUserLogin())
	, @usrID INT 
	, @Total BIGINT
	, @OFFSET INT = 0
	, @TOP INT
	, @SQL NVARCHAR(MAX)

IF ISNULL(@OrderBY, '') = ''
	SET @OrderBY = N'ORDER BY FavID DESC'
ELSE 
	SET @OrderBY = N'ORDER BY ' + @OrderBY

IF @MAX_RECORDS > 0
	SET @TOP = @MAX_RECORDS
ELSE
	SET @TOP = 50

IF @PageNo IS NULL
	SET @OFFSET = 0
ELSE
	SET @OFFSET = @TOP * (@PageNo- 1)

SET @usrID = (SELECT usrID FROM dbo.dbUser WHERE usrADID = @USERID)

IF @Filter = 'FA'
BEGIN
	SET @SQL = N'
WITH Res AS
(
SELECT uf.FavID
	, uf.usrFavType
	, c.clNo
	, c.clName
	, NULL AS fileNo
	, NULL AS fileDesc
	, NULL AS precDesc
	, NULL AS precExtension
	, c.Updated AS Updated
	, c.clId
	, NULL AS fileID
	, NULL AS PrecID
	, c.clNo + '' : '' + c.clName AS combo
	, uf.usrFavObjParam4 AS OrderForRecent
FROM dbo.dbUserFavourites uf
	INNER JOIN dbo.dbClient c ON c.clNo = uf.usrFavObjParam2 
WHERE uf.usrID = @usrID
	AND uf.usrFavType = ''CLINETFT''
	AND uf.usrFavDesc IN (''MYFAV'', ''PRECFAVTAB'')
UNION ALL
SELECT uf.FavID
	, uf.usrFavType
	, c.clNo
	, c.clName
	, f.fileNo
	, f.fileDesc
	, NULL AS precDesc
	, NULL AS precExtension
	, f.Updated AS Updated
	, c.clId
	, f.fileID
	, NULL AS PrecID
	, c.clNo + ''/'' + f.fileNo +  '' : '' + f.fileDesc AS combo
	, uf.usrFavObjParam4 AS OrderForRecent
FROM dbo.dbUserFavourites uf
	INNER JOIN dbo.dbClient c ON c.clNo = uf.usrFavObjParam2
	INNER JOIN dbo.dbFile f ON f.fileNo = uf.usrFavObjParam3 AND f.clId = c.clId
WHERE uf.usrID = @usrID
	AND uf.usrFavType = ''CLINETFILEFT''
	AND uf.usrFavDesc IN (''MYFAV'', ''PRECFAVTAB'')
UNION ALL
SELECT uf.FavID
	, uf.usrFavType
	, NULL AS clNo
	, NULL AS clName
	, NULL AS fileNo
	, NULL AS fileDesc
	, p.precDesc
	, p.precExtension
	, p.Updated AS Updated
	, NULL AS clId
	, NULL AS fileID
	, p.PrecID
	, p.precDesc + ISNULL(''.'' + p.precExtension, '''') AS combo
	, uf.usrFavObjParam4 AS OrderForRecent
FROM dbo.dbUserFavourites uf
	LEFT JOIN dbo.dbPrecedents p ON CAST(p.precId AS NVARCHAR(1000)) = uf.usrFavObjParam1
WHERE uf.usrID = @usrID
	AND uf.usrFavType = ''PRECFAV''
	AND uf.usrFavDesc IN (''MYFAV'', ''PRECFAVTAB'')
)
SELECT * FROM Res
'
	IF @Query IS NOT NULL
		SET @SQL = @SQL + N'WHERE (clNo LIKE @Query OR clName LIKE @Query OR fileNo LIKE @Query OR fileDesc LIKE @Query OR precDesc LIKE @Query)'
END

IF @Filter = 'RE'
BEGIN
	SET @SQL = N'
WITH Res AS
(
SELECT uf.FavID
	, uf.usrFavType
	, c.clNo
	, c.clName
	, NULL AS fileNo
	, NULL AS fileDesc
	, CAST(NULL AS NVARCHAR(1000)) AS precDesc
	, CAST(NULL AS NVARCHAR(1000)) AS precExtension
	, c.Updated AS Updated
	, c.clId
	, NULL AS fileID
	, CAST(NULL AS BIGINT) AS PrecID
	, c.clNo + '' : '' + c.clName AS combo
	, uf.usrFavObjParam4 AS OrderForRecent
FROM dbo.dbUserFavourites uf
	INNER JOIN dbo.dbClient c ON c.clNo = uf.usrFavObjParam2 
WHERE uf.usrID = @usrID
	AND uf.usrFavType = ''CLINETFT''
	AND uf.usrFavDesc = ''LAST10''
UNION ALL
SELECT uf.FavID
	, uf.usrFavType
	, c.clNo
	, c.clName
	, f.fileNo
	, f.fileDesc
	, CAST(NULL AS NVARCHAR(1000)) AS precDesc
	, CAST(NULL AS NVARCHAR(1000)) AS precExtension
	, CASE uf.usrFavType WHEN ''CLINETFILEFT'' THEN f.Updated WHEN ''CLINETFT'' THEN c.Updated END AS Updated
	, c.clId
	, f.fileID
	, CAST(NULL AS BIGINT) AS PrecID
	, CASE uf.usrFavType WHEN ''CLINETFILEFT'' THEN c.clNo + ''/'' + f.fileNo +  '' : '' + f.fileDesc WHEN ''CLINETFT'' THEN c.clNo + '' : '' + c.clName END AS combo
	, uf.usrFavObjParam4 AS OrderForRecent
FROM dbo.dbUserFavourites uf
	INNER JOIN dbo.dbClient c ON c.clNo = uf.usrFavObjParam2
	INNER JOIN dbo.dbFile f ON f.fileNo = uf.usrFavObjParam3 AND f.clId = c.clId
WHERE uf.usrID = @usrID
	AND uf.usrFavType = ''CLINETFILEFT''
	AND uf.usrFavDesc = ''LAST10''
)
SELECT * FROM Res

'

	IF @Query IS NOT NULL
		SET @SQL = @SQL + N'WHERE (clNo LIKE @Query OR clName LIKE @Query OR fileNo LIKE @Query OR fileDesc LIKE @Query)'
END

SET @SQL = @SQL + @OrderBY

INSERT INTO @Res
EXEC sp_executesql @SQL, N'@usrID INT, @Query NVARCHAR(MAX)', @usrID, @Query

SET @Total = @@ROWCOUNT

SELECT TOP(@TOP)
	FavID
	, usrFavType
	, clNo
	, clName
	, fileNo
	, fileDesc
	, precDesc
	, precExtension
	, Updated
	, clId
	, fileID
	, PrecID
	, combo
	, @Total AS Total
FROM @Res
WHERE ID > @OFFSET
ORDER BY ID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dshRecentFavourites] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dshRecentFavourites] TO [OMSAdminRole]
    AS [dbo];
