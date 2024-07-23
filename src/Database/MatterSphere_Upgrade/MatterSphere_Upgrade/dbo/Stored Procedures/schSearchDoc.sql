CREATE PROCEDURE dbo.schSearchDoc
(
	@CURRENTUSER INT = NULL
	, @UI uUICultureInfo = '{default}'
	, @FILEID BIGINT = 0
	, @MAX_RECORDS INT = 50
	, @DESC NVARCHAR(100) = ''
	, @CREATEDBY INT = NULL
	, @DOCTYPE uCodeLookup = NULL
	, @IN BIT = 1
	, @OUT BIT = 1
	, @ASSOCID BIGINT = NULL
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
	
	--Parameters added 24/03/17
	, @FolderGUIDList NVARCHAR(MAX) = NULL

	--Parameters added 27/10/17
	, @FolderGUID UNIQUEIDENTIFIER = NULL

	--Parameters added 04/03/19
	, @PageNo INT = NULL

	, @ORDERBY NVARCHAR(MAX) = NULL

)  
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @SELECT NVARCHAR(MAX)

IF @CURRENTUSER IS NULL
	SET @CURRENTUSER = dbo.GetCurrentUser()

IF @FILEID IS NULL 
	SET @FILEID = 0

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

DECLARE @R1 TABLE (DocumentID BIGINT PRIMARY KEY)
INSERT INTO @R1
SELECT DocumentID FROM config.searchDocumentAccess(''FILE'',@FILEID) r

DECLARE @Res TABLE(
	Id INT IDENTITY(1, 1) PRIMARY KEY
	, docID BIGINT
	, fileNo NVARCHAR(20)
	, contName NVARCHAR(128)
	, clNo NVARCHAR(12)
);

WITH Res AS(
SELECT 
	dbUser.usrInits AS CrByInits
	, dbUser.usrFullName AS CrByFullName
	, dbUser_1.usrInits AS UpdByInits
	, dbUser_1.usrFullName AS UpdByFullName
	, dbUser_2.usrInits AS AuthByInits
	, dbUser_2.usrFullName AS AuthByFullName
	, COALESCE(CL.cdDesc, ''~'' + NULLIF(D.docType, '''') + ''~'') AS doctypedesc
	, D.docID
	, D.created as crfield
	, D.docDesc
	, D.docType
	, COALESCE(D.docAuthored, D.Created) AS Created
	, D.Updated
	, D.docDirection
	, CASE WHEN SUBSTRING(LTRIM(D.docExtension), 1, 1) = ''.'' THEN LOWER(STUFF(LTRIM(D.docExtension), 1, 1, '''')) ELSE LOWER(D.docExtension) END AS docExtension
	, CLIENT.clNo
	, F.fileNo
	, D.docstyledesc
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(D.docwallet, '''') + ''~'') AS docwallet
	, CONT.contName
	, CONVERT(NVARCHAR(20), D.docid) as docidtxt
	, D.docFileName as doctoken
	, D.doccheckedout
	, D.doccheckedoutby
	, D.doccheckedoutlocation
	, V.verLabel
	, D.SecurityOptions
	, D.DocFlags
	, COALESCE(CL3.cdDesc, ''~'' + NULLIF(fc.FolderCode, '''') + ''~'') AS docFolder
	, D.docFolderGuid
FROM config.dbDocument D
	INNER JOIN config.dbFile F ON D.fileID = f.fileID 
	INNER JOIN dbo.dbAssociates ASS on ASS.associd = D.associd
	INNER JOIN dbo.dbContact CONT on CONT.contid = ASS.contid
	INNER JOIN @R1 r on r.DocumentID = D.docid
	INNER JOIN config.dbClient CLIENT ON CLIENT.clID = D.clID 
	LEFT OUTER JOIN dbo.dbUser ON dbUser.usrID = D.Createdby
	LEFT OUTER JOIN dbo.dbUser dbUser_1 ON D.UpdatedBy = dbUser_1.usrID 
	LEFT OUTER JOIN dbo.dbUser dbUser_2 ON D.docAuthoredBy = dbUser_2.usrID
	LEFT JOIN dbo.dbApplication A ON A.appID = D.docAppID
	LEFT JOIN dbo.dbDocumentVersion V ON v.verID = D.docCurrentVersion and V.DocID = D.DocID
	LEFT JOIN [dbo].[GetCodeLookupDescription] ( ''DOCTYPE'', @ui ) CL ON CL.[cdCode] = D.docType
	LEFT JOIN [dbo].[GetCodeLookupDescription] ( ''WALLET'', @ui ) CL1 ON CL1.[cdCode] = D.docwallet
    LEFT OUTER JOIN dbo.dbFileFolder fc ON fc.fileID = D.FileID AND fc.FolderGuid = D.docFolderGUID 
    LEFT JOIN dbo.GetCodeLookupDescription(''DFLDR_MATTER'', @UI) CL3 ON cl3.cdCode = fc.FolderCode
WHERE D.fileID = @FILEID
	'

IF @FolderGUIDList IS NOT NULL
	SET @SELECT = @SELECT + N'AND D.docFolderGUID in (select items from dbo.split(@FolderGUIDList, '',''))
	'

IF ISNULL(@DESC, '') <> ''
	SET @SELECT = @SELECT + N' AND D.DocDesc LIKE ''%'' + @DESC + ''%''
	'

IF @CREATEDBY IS NOT NULL
	SET @SELECT = @SELECT + N' AND D.CreatedBy = @CREATEDBY
	'

IF @DOCTYPE IS NOT NULL
	SET @SELECT = @SELECT + N' AND D.doctype = @DOCTYPE
	'

IF @IN = 1 AND @OUT = 0 
	SET @SELECT = @SELECT + N' AND D.docdirection = 1
	'

IF @IN = 0 AND @OUT = 1
	SET @SELECT = @SELECT + N' AND D.docdirection = 0
	'

IF @ASSOCID IS NOT NULL
	SET @SELECT = @SELECT + N' AND ASS.AssocID = @AssocID
	'

--------------------------------------
-- Additional filters applied to WHERE clasue 07/01/2009

-- Application Type of the Document
IF @APPTYPE IS NOT NULL
	SET @SELECT = @SELECT + N' AND D.docAppID = @APPTYPE
	'

-- Wallet
IF @WALLET IS NOT NULL
	SET @SELECT = @SELECT + N' AND D.docWallet = @WALLET
	'

-- Date Created Range
IF @CREATEDSTART IS NOT NULL
	SET @SELECT = @SELECT + N' AND D.Created >= @CREATEDSTART AND D.Created < @CREATEDEND
	'

-- Date Updated Range
IF @UPDATEDSTART IS NOT NULL
	SET @SELECT = @SELECT + N' AND D.Updated >= @UPDATEDSTART AND D.Updated < @UPDATEDEND
	'

-- Document Updated By
IF @UPDATEDBY IS NOT NULL
	SET @SELECT = @SELECT + N' AND D.UpdatedBy = @UPDATEDBY
	'

-- Date Authored Range
IF @AUTHOREDSTART IS NOT NULL
	SET @SELECT = @SELECT + N' AND D.docAuthored >= @AUTHOREDSTART AND D.docAuthored < @AUTHOREDEND
	'

-- Document Authored By
IF @AUTHOREDBY IS NOT NULL
	SET @SELECT = @SELECT + N' AND D.docAuthoredBy = @AUTHOREDBY
	'

--FolderGUID has been supplied
IF(@FolderGUID IS NOT NULL)
	SET @SELECT = @SELECT + N' AND D.docFolderGUID = @FolderGUID
	'

--------------------------------------

-- New code
IF EXISTS (SELECT C.[name] FROM syscolumns C JOIN sysobjects O ON C.[id] = O.[id] WHERE O.[name] = 'dbDocument'  and C.[name] = 'docdeleted' )
	IF @IncludeDeleted = 0
		SET @SELECT = @SELECT + N' AND COALESCE(D.docdeleted, 0) = 0
		'

SET @SELECT = @SELECT + N'
)

INSERT INTO @Res (docID, fileNo, contName, clNo)
SELECT 
	docID
	, fileNo
	, contName
	, clNo
FROM Res
'

IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY crfield DESC'
ELSE 
	IF @ORDERBY NOT LIKE '%crfield%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', crfield DESC'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY


SET @SELECT = @SELECT +  N'

SET @Total = @@ROWCOUNT

SELECT TOP(@TOP)
	dbUser.usrInits AS CrByInits
	, dbUser.usrFullName AS CrByFullName
	, dbUser_1.usrInits AS UpdByInits
	, dbUser_1.usrFullName AS UpdByFullName
	, dbUser_2.usrInits AS AuthByInits
	, dbUser_2.usrFullName AS AuthByFullName
	,  COALESCE(CL.cdDesc, ''~'' + NULLIF(D.docType, '''') + ''~'') AS doctypedesc
	, D.docID
	, D.created as crfield
	, D.docDesc
	, D.docType
	, COALESCE(D.docAuthored, D.Created) AS Created
	, D.Updated
	, D.docDirection
	, CASE WHEN SUBSTRING(LTRIM(D.docExtension), 1, 1) = ''.'' THEN LOWER(STUFF(LTRIM(D.docExtension), 1, 1, '''')) ELSE LOWER(D.docExtension) END AS docExtension
	, res.clNo
	, res.fileNo
	, D.docstyledesc
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(D.docwallet, '''') + ''~'') as docwallet
	, res.contName
	, CONVERT(NVARCHAR(20), D.docid) AS docidtxt
	, D.docFileName AS doctoken
	, D.doccheckedout
	, D.doccheckedoutby
	, D.doccheckedoutlocation
	, V.verLabel
	, D.SecurityOptions
	, D.DocFlags
	, COALESCE(CL3.cdDesc, ''~'' + NULLIF(fc.FolderCode, '''') + ''~'') AS docFolder
	, D.docFolderGuid
	, @Total AS Total'
	
DECLARE @HIGHQExists BIT = 0
IF EXISTS(SELECT 1 FROM dbo.dbPackages WHERE pkgCode = N'HIGHQ')
BEGIN
	SET @HIGHQExists = 1

	SET  @SELECT =  @SELECT + N'
	, HQ.UploadDate AS HighQUploadDate
	, HQ.UploadDate AS #highq#'
END
	
SET  @SELECT =  @SELECT + N'
FROM  @Res res
	INNER JOIN config.dbDocument D ON D.docID = res.docID
	LEFT OUTER JOIN dbo.dbUser ON dbUser.usrID = D.Createdby
	LEFT OUTER JOIN dbo.dbUser dbUser_1 ON D.UpdatedBy = dbUser_1.usrID 
	LEFT OUTER JOIN dbo.dbUser dbUser_2 ON D.docAuthoredBy = dbUser_2.usrID
	LEFT JOIN dbo.dbApplication A ON A.appID = D.docAppID
	LEFT JOIN dbo.dbDocumentVersion V ON v.verID = D.docCurrentVersion and V.DocID = D.DocID
	LEFT JOIN [dbo].[GetCodeLookupDescription] ( ''DOCTYPE'', @ui ) CL ON CL.[cdCode] = D.docType
	LEFT JOIN [dbo].[GetCodeLookupDescription] ( ''WALLET'', @ui ) CL1 ON CL1.[cdCode] = D.docwallet
    LEFT OUTER JOIN dbo.dbFileFolder fc ON fc.fileID = D.FileID AND fc.FolderGuid = D.docFolderGUID 
    LEFT JOIN dbo.GetCodeLookupDescription(''DFLDR_MATTER'', @UI) CL3 ON cl3.cdCode = fc.FolderCode'
	
IF @HIGHQExists = 1
	SET  @SELECT =  @SELECT + N'
	LEFT JOIN [highq].[dbDocumentUpload] HQ ON D.docID = HQ.docID'	
	
SET  @SELECT =  @SELECT + N'
WHERE res.Id > @OFFSET 
ORDER BY res.Id
'
PRINT @SELECT
--RETURN
exec sp_executesql @SELECT,  N'
	@CURRENTUSER int, 
	@UI uUICultureInfo, 
	@FILEID bigint,
	@MAX_RECORDS int,  
	@DESC nvarchar(100), 
	@CREATEDBY int, 
	@DOCTYPE uCodeLookup, 
	@IN bit, 
	@OUT bit, 
	@ASSOCID bigint,

	@APPTYPE nvarchar(15), 
	@WALLET nvarchar(15), 
	@CREATEDSTART datetime, 
	@CREATEDEND datetime, 
	@UPDATEDSTART datetime, 
	@UPDATEDEND datetime, 
	@UPDATEDBY int, 
	@AUTHOREDSTART datetime, 
	@AUTHOREDEND datetime, 
	@AUTHOREDBY int, 
	@FolderGUIDList nvarchar(max),
	@FolderGUID uniqueidentifier,	
	@PageNo INT',
	
	@CURRENTUSER, 
	@UI, 
	@FILEID, 
	@MAX_RECORDS, 
	@DESC, 
	@CREATEDBY,
	@DOCTYPE,
	@IN,
	@OUT,
	@ASSOCID,

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
	@FolderGUIDList,
	@FolderGUID,
	@PageNo