CREATE PROCEDURE [dbo].[schSearchDocClient]
(
	@CURRENTUSER INT = NULL
	, @UI uUICultureInfo = '{default}'
	, @CLIENTID BIGINT = 0
	, @MAX_RECORDS INT = 50
	, @DESC NVARCHAR(100) = ''
	, @CREATEDBY INT = NULL
	, @DOCTYPE uCodeLookup = NULL
	, @IN BIT = 1
	, @OUT BIT = 1
	, @IncludeDeleted BIT = 0

	-- Parameters added 07/01/2009
	, @APPTYPE NVARCHAR(15) = NULL
	, @WALLET NVARCHAR(15) = NULL
	, @CREATEDSTART DATETIME = NULL
	, @CREATEDEND DATETIME = NULL
	, @UPDATEDSTART DATETIME = NULL
	, @UPDATEDEND DATETIME = NULL
	, @UPDATEDBY INT = NULL
	, @AUTHOREDSTART DATETIME = NULL
	, @AUTHOREDEND DATETIME = NULL
	, @AUTHOREDBY INT = NULL

	--Parameters added 31/10/17
	, @FolderCode NVARCHAR(15) = NULL
	--Parameters added 17/08/20
	, @ORDERBY NVARCHAR(MAX) = NULL
	--Parameters added 31/08/22
	, @PageNo INT = NULL
)  
AS

SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @SELECT NVARCHAR(MAX)

IF @CURRENTUSER is NULL
	SET @CURRENTUSER = dbo.GetCurrentUser()

IF @CLIENTID IS NULL 
	SET @CLIENTID = 0


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
	SET @OFFSET = @TOP * (@PageNo - 1)

SELECT DocumentID INTO #R1 FROM config.searchDocumentAccess(''CLIENT'', @CLIENTID) r
	
CREATE UNIQUE CLUSTERED INDEX AF1 ON #R1 (DocumentID);

IF OBJECT_ID(N''tempdb..#Res'', N''U'') IS NOT NULL
DROP TABLE #Res

CREATE TABLE #Res(
	  Id INT IDENTITY(1, 1) PRIMARY KEY
	, CrByInits NVARCHAR(30)
	, CrByFullName NVARCHAR(50)
	, UpdByInits NVARCHAR(30)
	, UpdByFullName NVARCHAR(50)
	, AuthByInits NVARCHAR(30)
	, AuthByFullName NVARCHAR(50)
	, doctypedesc NVARCHAR(50)
	, docID BIGINT
	, docDesc NVARCHAR(150)
	, docType NVARCHAR(15)
	, Created DATETIME
	, Updated DATETIME
	, crfield DATETIME
	, docExtension NVARCHAR(15)
	, docDirection BIT
	, clNo NVARCHAR(12)
	, fileNo NVARCHAR(20)
	, ClientFileNo NVARCHAR(50)
	, doctoken NVARCHAR(255)
	, doccheckedout DATETIME
	, doccheckedoutby INT
	, doccheckedoutlocation NVARCHAR(255)
	, docwallet NVARCHAR(50)
	, verLabel NVARCHAR(50)
	, SecurityOptions BIGINT
	, DocFlags SMALLINT
	, docFolder NVARCHAR(1000)
);

WITH Res AS
(
SELECT 
	U.usrInits AS CrByInits
	, U.usrFullName AS CrByFullName
	, U1.usrInits AS UpdByInits
	, U1.usrFullName AS UpdByFullName
	, U2.usrInits AS AuthByInits
	, U2.usrFullName AS AuthByFullName
	, COALESCE(CL.cdDesc, ''~'' + NULLIF(D.docType, '''') + ''~'') AS doctypedesc
	, D.docID
	, D.docDesc
	, D.docType
	, COALESCE(D.docAuthored, D.Created) AS Created
	, D.Updated
	, D.created as crfield
	, CASE WHEN SUBSTRING(LTRIM(D.docExtension), 1, 1) = ''.'' THEN LOWER(STUFF(LTRIM(D.docExtension), 1, 1, '''')) ELSE LOWER(D.docExtension) END AS docExtension
	, D.docDirection
	, C.clNo
	, F.fileNo
	, dbo.GetFileRef(C.clNo, F.fileNo) AS ClientFileNo
	, D.docFileName AS doctoken
	, D.doccheckedout
	, D.doccheckedoutby
	, D.doccheckedoutlocation
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(D.docwallet, '''') + ''~'') AS docwallet
	, V.verLabel
	, D.SecurityOptions
	, D.DocFlags
	, COALESCE(CL3.cdDesc, ''~'' + NULLIF(fc.FolderCode, '''') + ''~'') AS docFolder
FROM config.dbDocument D
	INNER JOIN config.dbFile F ON F.fileID = D.fileID 
	INNER JOIN config.dbClient C ON C.clID = D.clID
	INNER JOIN dbo.dbAssociates ASS on ASS.associd = D.associd
	LEFT OUTER JOIN dbo.dbUser U ON U.usrID = D.Createdby
	LEFT OUTER JOIN dbo.dbUser U1 ON U1.usrID = D.UpdatedBy
	LEFT OUTER JOIN dbo.dbApplication A ON A.appID = D.docAppID
	LEFT JOIN dbo.dbDocumentVersion V ON v.verID = D.docCurrentVersion
	LEFT OUTER JOIN dbo.dbUser U2 ON U2.usrID = D.docAuthoredBy
	INNER JOIN  #R1 r on r.DocumentID = D.docid
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''DOCTYPE'', @UI ) CL ON CL.cdCode = D.docType
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''WALLET'', @UI ) CL1 ON CL1.cdCode = D.docwallet
	LEFT OUTER JOIN dbo.dbFileFolder fc ON fc.fileID = D.FileID AND fc.FolderGuid = D.docFolderGUID 
    LEFT JOIN dbo.GetCodeLookupDescription(''DFLDR_MATTER'', @UI) CL3 ON cl3.cdCode = fc.FolderCode
WHERE D.clID = @CLIENTID
	'

IF ISNULL(@DESC, '') <> ''
	SET @SELECT = @SELECT + N'AND D.DocDesc LIKE ''%'' + @DESC + ''%''
	'

IF @CREATEDBY IS NOT NULL
	SET @SELECT = @SELECT + N'AND D.CreatedBy = @CREATEDBY
	'

IF @DOCTYPE IS NOT NULL
	SET @SELECT = @SELECT + N'AND D.doctype = @DOCTYPE
	'

IF @IN = 1 AND @OUT = 0 
	SET @SELECT = @SELECT + N'AND D.docdirection = 1
	'

IF @IN = 0 AND @OUT = 1
	SET @SELECT = @SELECT + N'AND D.docdirection = 0
	'

--------------------------------------
-- Additional filters applied to WHERE clasue 07/01/2009

-- Application Type of the Document
IF @APPTYPE IS NOT NULL
	SET @SELECT = @SELECT + N'AND D.docAppID = @APPTYPE
	'

-- Wallet
IF @WALLET IS NOT NULL
	SET @SELECT = @SELECT + N'AND dbDocument.docWallet = @WALLET
	'

-- Date Created Range
IF @CREATEDSTART IS NOT NULL
	SET @SELECT = @SELECT + N'AND D.Created >= @CREATEDSTART 
	AND D.Created < @CREATEDEND
	'

-- Date Updated Range
IF @UPDATEDSTART IS NOT NULL
	SET @SELECT = @SELECT + N'AND D.Updated >= @UPDATEDSTART 
	AND D.Updated < @UPDATEDEND
	'

-- Document Updated By
IF @UPDATEDBY IS NOT NULL
	SET @SELECT = @SELECT + N'AND D.UpdatedBy = @UPDATEDBY
	'

-- Date Authored Range
IF @AUTHOREDSTART IS NOT NULL
	SET @SELECT = @SELECT + N'AND D.docAuthored >= @AUTHOREDSTART 
	AND D.docAuthored < @AUTHOREDEND
	'

-- Document Authored By
IF @AUTHOREDBY IS NOT NULL
	SET @SELECT = @SELECT + N'AND D.docAuthoredBy = @AUTHOREDBY
	'

-- Document FolderGUID Filter
IF @FolderCode IS NOT NULL
	SET @SELECT = @SELECT + N'AND fc.FolderCode = ''' + @FolderCode + N'''
	'

-- New code
IF EXISTS(SELECT C.name FROM sys.columns C JOIN sys.objects O ON C.object_id = O.object_id WHERE O.name = 'dbDocument' AND C.name = 'docdeleted')
	IF @IncludeDeleted = 0
		SET @SELECT = @SELECT + N'AND D.docdeleted = 0
	'

SET @SELECT = @SELECT + N'
)

INSERT INTO #Res
SELECT * FROM Res
'

IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY crfield DESC'
ELSE 
	IF @ORDERBY NOT LIKE '%crfield%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', crfield DESC'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

SET @SELECT = @SELECT + N'
	SET @Total = @@ROWCOUNT
	
	SELECT ' + CASE WHEN ISNULL(@MAX_RECORDS, 0) > 0 THEN N'TOP(@MAX_RECORDS)' ELSE N'' END + N'
		res.CrByInits 
		, res.CrByFullName
		, res.UpdByInits 
		, res.UpdByFullName
		, res.AuthByInits 
		, res.AuthByFullName
		, res.doctypedesc
		, res.docID
		, res.docDesc
		, res.docType
		, res.Created
		, res.Updated
		, res.crfield
		, res.docExtension
		, res.docDirection 
		, res.clNo
		, res.fileNo
		, res.ClientFileNo
		, res.doctoken
		, res.doccheckedout
		, res.doccheckedoutby 
		, res.doccheckedoutlocation
		, res.docwallet
		, res.verLabel
		, res.SecurityOptions
		, res.DocFlags 
		, res.docFolder
		, @Total AS Total
	FROM #Res res
	WHERE res.Id > @OFFSET 
'

PRINT @SELECT

EXEC sp_executesql @SELECT,  N'
	@CURRENTUSER INT, 
	@UI uUICultureInfo, 
	@CLIENTID bigint,
	@MAX_RECORDS INT,  
	@DESC NVARCHAR(100), 
	@CREATEDBY INT, 
	@DOCTYPE uCodeLookup, 
	@IN BIT, 
	@OUT BIT, 

	@APPTYPE NVARCHAR(15), 
	@WALLET NVARCHAR(15), 
	@CREATEDSTART DATETIME, 
	@CREATEDEND DATETIME, 
	@UPDATEDSTART DATETIME, 
	@UPDATEDEND DATETIME, 
	@UPDATEDBY INT, 
	@AUTHOREDSTART DATETIME, 
	@AUTHOREDEND DATETIME, 
	@AUTHOREDBY INT, 
	@FolderCode NVARCHAR(15),
	@PageNo INT',

	@CURRENTUSER, 
	@UI, 
	@CLIENTID, 
	@MAX_RECORDS, 
	@DESC, 
	@CREATEDBY,
	@DOCTYPE,
	@IN,
	@OUT,

	-- Parameters added 07/01/2009
	@APPTYPE, 
	@WALLET, 
	@CREATEDSTART, 
	@CREATEDEND, 
	@UPDATEDSTART, 
	@UPDATEDEND, 
	@UPDATEDBY, 
	@AUTHOREDSTART, 
	@AUTHOREDEND, 
	@AUTHOREDBY,
	--Parameters added 31/10/2017
	@FolderCode,
	--Parameters added 31/08/2022
	@PageNo
	
