CREATE PROCEDURE dbo.schSearchEmailSimple
(
	@MAX_RECORDS INT = 50
	, @UI uUICultureInfo = '{default}'
	, @FILEID BIGINT
	, @FROM NVARCHAR(30)
	, @TO NVARCHAR(30)
	, @TIME NVARCHAR(15)
	, @SUBJECT NVARCHAR(200) = NULL
	, @TIMEOPT NVARCHAR(15)
	--Parameters added 17/08/20
	, @ORDERBY NVARCHAR(MAX) = NULL
	--Parameters added 13/09/22
	, @PageNo INT = NULL
)  

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
DECLARE @SELECT NVARCHAR(MAX)

-- SET THE SELECT STATEMENT
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

IF OBJECT_ID(N''tempdb..#Res'', N''U'') IS NOT NULL
DROP TABLE #Res

CREATE TABLE #Res (
	clNo nvarchar(12)
	, fileNo nvarchar(20)
	, docTypedesc nvarchar(1000)
	, Created datetime
	, CrByFullName nvarchar(50)
	, docID bigint
	, docFrom nvarchar(255)
	, docTo nvarchar(1000)
	, docCC nvarchar(1000)
	, docDesc nvarchar(150)
	, docSent datetime
	, docReceived datetime
	, docAttachments int
	, Icon int
	, docExtension NVARCHAR(15)
	, DocCheckedOutBy INT
	, DocAuthored datetime
	, Id int IDENTITY(1, 1)
)

SELECT DocumentID INTO #R1 FROM config.searchDocumentAccess(''FILE'', @FILEID) r
CREATE UNIQUE CLUSTERED INDEX AF1 ON #R1 (DocumentID);

WITH Res AS
(
SELECT CL.clNo
	, F.fileNo
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(D.docType, '''') + ''~'') AS docTypedesc
	, D.Created
	, U.usrFullName AS CrByFullName
	, D.docID
	, DE.docFrom
	, DE.docTo
	, DE.docCC
	, D.docDesc
	, DE.docSent
	, DE.docReceived
	, DE.docAttachments
	, CASE
		WHEN DE.docAttachments > 0 THEN 34
		ELSE 3
	END AS Icon
	, D.docExtension
	, D.DocCheckedOutBy
	, D.DocAuthored
FROM config.dbDocument D
	INNER JOIN config.dbFile F ON F.fileID = D.fileID
	INNER JOIN config.dbClient CL ON CL.clID = F.clID
	INNER JOIN dbo.dbUser U ON U.usrID = D.CreatedBy
	INNER JOIN dbo.dbDocumentEmail DE ON DE.docID = D.docID
	INNER JOIN dbo.dbDocumentPreview DP ON D.docID = DP.docID 
	INNER JOIN #R1 r on r.DocumentID = D.docid
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''DOCTYPE'', @UI) CL1 ON CL1.cdCode = D.docType
WHERE D.fileID = @FILEID 
	AND D.docType = ''EMAIL''
	'

--- EMAIL FROM CLAUSE
IF @FROM IS NOT NULL
	SET @SELECT = @SELECT + N'AND DE.docFrom LIKE ''%'' + @FROM + ''%''
	'

--- EMAIL TO CLAUSE
IF @TO IS NOT NULL
	SET @SELECT = @SELECT + N'AND DE.docTo LIKE ''%'' + @TO + ''%''
	'

-- SUBJECT SEARCH
IF ISNULL(@SUBJECT, '') <> ''
	SET @SELECT = @SELECT + N'AND D.docDesc LIKE ''%'' + @SUBJECT + ''%''
	'

-- DATE RANGE CLAUSE
IF @TIME = 'SENT' -- USE THE EMAIL SENT DATE
	SET @SELECT = @SELECT +
	CASE @TIMEOPT 
		WHEN 'TOD' THEN N'AND DE.docSent = GETUTCDATE()'
		WHEN 'YEST' THEN N'AND DE.docSent = GETUTCDATE() - 1'
		WHEN 'LAST7' THEN N'AND DE.docSent > GETUTCDATE() - 7 AND DE.docSent < GETUTCDATE()'
		ELSE N''
	END + N'
	'
IF @TIME = 'REC' -- USE THE EMAIL RECEIVED DATE
	SET @SELECT = @SELECT +
	CASE @TIMEOPT 
		WHEN 'TOD' THEN N'AND DE.docReceived = GETUTCDATE()'
		WHEN 'YEST' THEN N'AND DE.docReceived = GETUTCDATE() - 1'
		WHEN 'LAST7' THEN N'AND DE.docReceived > GETUTCDATE() - 7 AND DE.docReceived < GETUTCDATE()'
		ELSE N''
	END + N'
	'

SET @SELECT = @SELECT + N'
)

INSERT INTO #Res 
SELECT * FROM Res
'
IF @ORDERBY IS NULL
	SET @SELECT = @SELECT + N'ORDER BY DocAuthored DESC'
ELSE 
	IF @ORDERBY NOT LIKE '%DocAuthored%'
		SET @SELECT = @SELECT + N'ORDER BY ' + @ORDERBY  + N', DocAuthored DESC'
	ELSE 
		SET @SELECT = @SELECT + N'ORDER BY ' + @ORDERBY

SET @SELECT = @SELECT + N'
SET @Total = @@ROWCOUNT

SELECT ' + CASE WHEN ISNULL(@MAX_RECORDS, 0) > 0 THEN N'TOP(@MAX_RECORDS)' ELSE N'' END + N'
	clNo
	, fileNo
	, docTypedesc
	, Created
	, CrByFullName
	, docID
	, docFrom
	, docTo
	, docCC
	, docDesc
	, docSent
	, docReceived
	, docAttachments
	, Icon
	, docExtension
	, DocCheckedOutBy
	, @Total Total
FROM #Res res
WHERE res.Id > @OFFSET 
'

PRINT @SELECT

-- EXECUTE THE SQL
EXEC sp_executesql @SELECT, 
	N'@MAX_RECORDS INT, @UI uUICultureInfo, @FILEID BIGINT, @FROM NVARCHAR(30), @TO NVARCHAR(30), @TIME NVARCHAR(15), @TIMEOPT NVARCHAR(15), @SUBJECT NVARCHAR(200), @PageNo INT'
	, @MAX_RECORDS, @UI, @FILEID, @FROM, @TO, @TIME, @TIMEOPT, @SUBJECT, @PageNo

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchEmailSimple] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchEmailSimple] TO [OMSAdminRole]
    AS [dbo];

