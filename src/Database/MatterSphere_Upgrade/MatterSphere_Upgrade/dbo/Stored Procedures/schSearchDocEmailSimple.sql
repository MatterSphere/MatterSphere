CREATE PROCEDURE dbo.schSearchDocEmailSimple
(
	@MAX_RECORDS INT = 50
	, @UI uUICultureInfo = '{default}'
	, @FROM NVARCHAR(30)
	, @TO NVARCHAR(30)
	, @TIME NVARCHAR(15)
	, @SUBJECT NVARCHAR(200) = NULL
	, @TIMEOPT NVARCHAR(15)
	--Parameters added 17/08/20
	, @ORDERBY NVARCHAR(MAX) = NULL
	--Parameters added 08/09/22
	, @PageNo INT = NULL
)  

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @SELECT NVARCHAR(MAX)

SET @SELECT = 
N'
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
	, Id int IDENTITY(1, 1)
)
;
WITH Res AS
(	
SELECT 
	CL.clNo
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
FROM dbo.dbDocument D
	INNER JOIN dbo.dbFile F ON F.fileID = D.fileID
	INNER JOIN dbo.dbClient CL ON CL.clID = F.clID
	INNER JOIN dbo.dbUser U ON U.usrID = D.CreatedBy
	INNER JOIN dbo.dbDocumentEmail DE ON DE.docID = D.docID
	INNER JOIN dbo.dbDocumentPreview DP ON DP.docID = D.docID
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''DOCTYPE'', @UI) CL1 ON CL1.cdCode = D.docType
WHERE D.docType = ''EMAIL''
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
	SET  @SELECT =  @SELECT + N'ORDER BY Created DESC'
ELSE 
	IF @ORDERBY NOT LIKE '%Created%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', Created DESC'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

SET @SELECT = @SELECT + N'
SET @Total = @@ROWCOUNT

SELECT ' + CASE WHEN ISNULL(@MAX_RECORDS, 0) > 0 THEN N'TOP(@MAX_RECORDS)' ELSE N'' END + N'
	res.clNo
	, res.fileNo
	, res.docTypedesc
	, res.Created
	, res.CrByFullName
	, res.docID
	, res.docFrom
	, res.docTo
	, res.docCC
	, res.docDesc
	, res.docSent
	, res.docReceived
	, res.docAttachments
	, res.Icon
	, res.docExtension
	, res.DocCheckedOutBy
	, @Total Total 
FROM #Res res
WHERE res.Id > @OFFSET 
'

PRINT @SELECT

-- EXECUTE THE SQL
EXEC sp_executesql @SELECT, 
N'
	@MAX_RECORDS INT
	, @UI uUICultureInfo
	, @FROM NVARCHAR(30)
	, @TO NVARCHAR(30)
	, @TIME NVARCHAR(15)
	, @TIMEOPT NVARCHAR(15) 
	, @SUBJECT NVARCHAR(200) 
	, @PageNo INT'
	, @MAX_RECORDS
	, @UI
	, @FROM
	, @TO
	, @TIME
	, @TIMEOPT
	, @SUBJECT
	, @PageNo

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchDocEmailSimple] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchDocEmailSimple] TO [OMSAdminRole]
    AS [dbo];

