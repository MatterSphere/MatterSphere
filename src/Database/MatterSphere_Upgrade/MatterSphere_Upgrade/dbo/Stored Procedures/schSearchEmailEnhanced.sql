CREATE PROCEDURE dbo.schSearchEmailEnhanced 
(
	@MAX_RECORDS INT = 50
	, @UI uUICultureInfo = '{default}'
	, @FILEID BIGINT
	, @FROM NVARCHAR(30)
	, @TO NVARCHAR(30)
	, @CONTENT NVARCHAR(200) = NULL
	, @STARTDATE DATETIME = NULL
	, @ENDDATE DATETIME = NULL
	, @SENTREC NVARCHAR(15)
	, @ATTACHMENTS NVARCHAR(15) = NULL
	, @OR BIT = 0
	, @NEAR BIT = 0
	, @FULLTEXT BIT = 0
	--Parameters added 17/08/20
	, @ORDERBY NVARCHAR(MAX) = NULL
	--Parameters added 12/09/22
	, @PageNo INT = NULL
)  

AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @SELECT NVARCHAR(MAX)

-- SET THE FULLTEXT PARAMETER TO BE TRUE IF THE FULL-TEXT INDEX IS TURNED ON
IF EXISTS(SELECT * FROM dbo.SysFullTextCatalogs WHERE name = 'docPreview')
	SET @FULLTEXT = 1

-- IF FULLTEXT IS TRUE SET THE CONTENT PARAMETER
IF(@FULLTEXT = 1)
BEGIN
	IF(@CONTENT <> '')
	BEGIN
		IF(@CONTENT NOT LIKE '% near %') AND (@CONTENT NOT LIKE '% or %')
			SET @CONTENT = REPLACE(@CONTENT, ' ', ' AND ')
		ELSE
			SET @CONTENT = '(' + @CONTENT + ')'
	END
END

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
	clNo NVARCHAR(12)
	, fileNo NVARCHAR(20)
	, docTypedesc NVARCHAR(1000)
	, Created DATETIME
	, CrByFullName NVARCHAR(50)
	, docID BIGINT
	, docFrom NVARCHAR(255)
	, docTo NVARCHAR(1000)
	, docCC NVARCHAR(1000)
	, docDesc NVARCHAR(150)
	, docSent DATETIME
	, docReceived DATETIME
	, docAttachments INT
	, Icon INT
	, docExtension NVARCHAR(15)
	, DocCheckedOutBy INT
	, DocAuthored DATETIME
	, Id int IDENTITY(1, 1)
)

SELECT DocumentID INTO #R1 FROM config.searchDocumentAccess (''FILE'',@FILEID) r
CREATE UNIQUE CLUSTERED INDEX AF1 ON #R1 ([DocumentID]);

WITH Res AS
(
SELECT  CL.clNo
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
	INNER JOIN config.dbDocumentPreview DP ON D.docID = DP.docID 
	INNER JOIN  #R1 r on r.DocumentID = D.docid
	LEFT JOIN [dbo].[GetCodeLookupDescription] ( ''DOCTYPE'', @ui ) CL1 ON CL1.[cdCode] = D.docType
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

--- EMAIL SUBJECT CLAUSE
IF @CONTENT <> ''
	IF @FULLTEXT = 0
		SET @SELECT = @SELECT + N'AND (D.docDesc LIKE ''%'' + @CONTENT + ''%'' OR DP.docPreview LIKE ''%'' + @CONTENT + ''%'')
	'
	ELSE
		SET @SELECT = @SELECT + N'AND (CONTAINS(docDesc, @CONTENT) OR CONTAINS(docPreview, @CONTENT))
	'

-- EMAIL ATTACHMENTS
IF @ATTACHMENTS = 'NONE'
	SET @SELECT = @SELECT + N'AND DE.docAttachments = 0
	'
ELSE IF @ATTACHMENTS = 'ONEPLUS'
	SET @SELECT = @SELECT + N'AND DE.docAttachments > 0
	'

--- SENT/RECEIVED START DATE AND END DATE
IF @STARTDATE IS NOT NULL
	IF @SENTREC = 'REC'
		SET @SELECT = @SELECT + N'AND DE.docReceived >= @STARTDATE 
	AND DE.docReceived < @ENDDATE
	'
	ELSE IF @SENTREC = 'SENT'
		SET @SELECT = @SELECT + N'AND DE.docSent >= @STARTDATE 
	AND DE.docSent < @ENDDATE
	'

SET @SELECT = @SELECT + N'
)

INSERT INTO #Res
SELECT * FROM Res
'

IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY DocAuthored DESC'
ELSE 
	IF @ORDERBY NOT LIKE '%DocAuthored%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', DocAuthored DESC'
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
	N'@MAX_RECORDS INT, @UI uUICultureInfo, @FILEID BIGINT, @FROM NVARCHAR(30), @TO NVARCHAR(30), @CONTENT NVARCHAR(200), @STARTDATE DATETIME, @ENDDATE DATETIME, @SENTREC NVARCHAR(15), @ATTACHMENTS NVARCHAR(15), @PageNo INT'
	, @MAX_RECORDS, @UI, @FILEID, @FROM, @TO, @CONTENT, @STARTDATE, @ENDDATE, @SENTREC, @ATTACHMENTS, @PageNo
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchEmailEnhanced] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchEmailEnhanced] TO [OMSAdminRole]
    AS [dbo];

