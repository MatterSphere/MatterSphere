CREATE PROCEDURE dbo.schSearchFileUndertakings
(
	@UI uUICultureInfo = '{default}'
	, @FILEID BIGINT = NULL
	, @MAX_RECORDS INT = 0
	, @PageNo INT = NULL
	, @ACTIVE BIT = NULL
	, @ORDERBY NVARCHAR(MAX) = NULL
)
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @SELECT NVARCHAR(MAX)

SET @SELECT = N'
DECLARE @OFFSET INT = 0
	, @TOP INT
	, @Total INT
	
IF @MAX_RECORDS > 0
	SET @TOP = @MAX_RECORDS
ELSE
	SET @TOP = 50

IF @PageNo IS NULL
	SET @OFFSET = 0
ELSE
	SET @OFFSET = @TOP * (@PageNo- 1)

DECLARE @Res TABLE(
	Id BIGINT PRIMARY KEY
	, undid BIGINT
	, UndTypeDesc NVARCHAR(1000)
);

WITH Res AS
(
SELECT
	U.*
	, COALESCE(CL.cdDesc, ''~'' + NULLIF(U.undType, '''') + ''~'') AS UndTypeDesc  
FROM dbo.dbUndertakings U
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''UNDERTYPE'', @UI ) CL ON CL.[cdCode] = U.undType
WHERE U.fileID = @FILEID
	'

IF @ACTIVE IS NOT NULL
    SET @SELECT = @SELECT + N'AND U.undActive = @ACTIVE
	'
SET @SELECT = @SELECT + N'
)
INSERT INTO @Res (undid, UndTypeDesc, Id)
SELECT 
	undid
	, UndTypeDesc
'

IF @ORDERBY IS NULL
	SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY undid)'
ELSE 
	IF @ORDERBY NOT LIKE '%undid%'
		SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY ' + @ORDERBY  + N', undid)'
	ELSE 
		SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY ' + @ORDERBY  + N')'


SET @SELECT = @SELECT + N'
FROM Res
SET @Total = @@ROWCOUNT

SELECT TOP(@TOP)
	U.*
	, res.UndTypeDesc  
	, @Total AS total
FROM @RES res
	INNER JOIN dbo.dbUndertakings U ON U.undid = res.undid
WHERE res.Id > @OFFSET 
ORDER BY res.Id
'

PRINT @SELECT 

EXEC sp_executesql @SELECT,  N'@UI uUICultureInfo, @FILEID BIGINT, @MAX_RECORDS INT, @PageNo INT, @ACTIVE BIT',
	@UI, @FILEID, @MAX_RECORDS, @PageNo, @ACTIVE

SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchFileUndertakings] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchFileUndertakings] TO [OMSAdminRole]
    AS [dbo];