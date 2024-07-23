CREATE PROCEDURE dbo.dshMatterList(@FeeEarnerID INT, @MAX_RECORDS INT = 50, @PageNo INT = 1, @Query NVARCHAR(MAX) = null, @Filter uCodeLookup = null, @OrderBY NVARCHAR(MAX) = NULL)
AS
DECLARE @OFFSET INT = 0
	, @TOP INT
	, @Total INT
	, @dtStart DATETIME
	, @dtEnd DATETIME
	, @SQL NVARCHAR(MAX)

IF ISNULL(@OrderBY, '') = ''
	SET @OrderBY = N'ORDER BY C.clID, F.fileID'
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

DECLARE @Res TABLE(
	Id INT IDENTITY(1, 1) PRIMARY KEY
	, fileID BIGINT
	, clNo NVARCHAR(12)
)

SET @SQL = N'
SELECT 
	 F.fileID
	, C.clNo
FROM dbo.dbClient C 
	INNER JOIN dbo.dbFile F ON C.clID = F.clID 
WHERE (F.filePrincipleID = @FeeEarnerID)
'

IF @Filter IS NOT NULL
BEGIN
IF @Filter = 'WITHIN7' 
	BEGIN 
		SET @dtStart = GETUTCDATE() 
		SET @dtEnd = @dtStart + 7 
	END
ELSE 
IF @Filter = 'OVER7'
	BEGIN
		SET @dtStart = GETUTCDATE() + 7
		SET @dtEnd = '9999-12-31' 
	END	                      
ELSE
IF @Filter = 'OD'
	BEGIN
		SET @dtStart = '1900-01-01'
		SET @dtEnd = GETUTCDATE()
	END
ELSE
	BEGIN
		SET @dtStart = '1900-01-01'
		SET @dtEnd = '9999-12-31'
	END

	SET @SQL = @SQL + N'
	AND (F.fileReviewDate > @dtStart AND F.fileReviewDate < @dtEnd)
'
END

IF @Query IS NOT NULL
	SET @SQL = @SQL + N'
	AND (F.fileDesc LIKE ''%'' + @Query + ''%'' OR F.fileNo LIKE ''%'' + @Query + ''%'' OR C.clNo LIKE ''%'' + @Query + ''%'')
'
SET @SQL = @SQL + @OrderBY

INSERT INTO @Res(fileID, clNo)
EXEC sp_executesql @SQL, N'@dtStart DATETIME, @dtEnd DATETIME, @Query NVARCHAR(MAX), @FeeEarnerID INT', @dtStart, @dtEnd, @Query, @FeeEarnerID

SET @Total = @@ROWCOUNT

SELECT TOP(@TOP)
	F.clID
	, R.fileID
	, R.clNo
	, F.fileNo
	, F.fileDesc
	, F.fileReviewDate
	, @Total AS Total
FROM @Res R 
	INNER JOIN config.dbFile F ON R.fileID = F.fileID 
WHERE R.Id > @OFFSET
ORDER BY R.Id
GO

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dshMatterList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dshMatterList] TO [OMSAdminRole]
    AS [dbo];

