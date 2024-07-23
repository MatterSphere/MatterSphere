CREATE PROCEDURE dbo.schSearchFileKeyDates
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
	, kdid BIGINT
);

WITH Res AS
(
SELECT
	K.*
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(K.kdtype, '''') + ''~'') AS kdtypedesc
	, CASE WHEN dbo.getenddate(K.kddate) <= dbo.getenddate(getdate()) THEN 13 ELSE 0 END AS Priority
FROM dbo.dbKeyDates K
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''DATEWIZTYPES'', @UI) CL1 ON CL1.cdCode = K.kdtype
WHERE K.fileID = @FILEID
	'

IF @ACTIVE IS NOT NULL
	SET @SELECT = @SELECT + N'AND K.kdactive = @ACTIVE
	'
SET @SELECT = @SELECT + N'
)

INSERT INTO @Res (kdid, Id)
SELECT 
	kdid
	'

IF @ORDERBY IS NULL
	SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY kddate)'
ELSE 
	IF @ORDERBY NOT LIKE '%kddate%'
		SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY ' + @ORDERBY  + N', kddate)'
	ELSE 
		SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY ' + @ORDERBY  + N')'

SET @SELECT = @SELECT + N'
FROM Res

SET @Total = @@ROWCOUNT

SELECT TOP(@TOP)
	K.*
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(K.kdtype, '''') + ''~'') AS kdtypedesc
	, CASE WHEN dbo.getenddate(K.kddate) <= dbo.getenddate(getdate()) THEN 13 ELSE 0 END AS Priority
	, @Total as total
FROM @RES res
INNER JOIN dbo.dbKeyDates K ON K.kdid = res.kdid
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''DATEWIZTYPES'', @UI) CL1 ON CL1.cdCode = K.kdtype
WHERE res.Id > @OFFSET 
ORDER BY res.Id
'

PRINT @SELECT 

EXEC sp_executesql @SELECT,  N'@UI uUICultureInfo, @FILEID BIGINT, @MAX_RECORDS INT, @PageNo INT, @ACTIVE BIT',
	@UI, @FILEID, @MAX_RECORDS, @PageNo, @ACTIVE

SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchFileKeyDates] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchFileKeyDates] TO [OMSAdminRole]
    AS [dbo];