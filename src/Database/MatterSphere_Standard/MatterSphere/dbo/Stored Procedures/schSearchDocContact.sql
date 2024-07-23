CREATE PROCEDURE [dbo].[schSearchDocContact]
(
	  @zDateStart DATETIME = NULL
	, @zDateEnd DATETIME = NULL
	, @zContID bigint = NULL
	, @zCreatedBy INT = NULL
	, @zDocType NVARCHAR(15) = NULL
	, @DESC NVARCHAR(100) = ''
	, @DOCWALLET NVARCHAR(15) = NULL
	, @DEPT NVARCHAR(15) = NULL
	, @FILETYPE NVARCHAR(15) = NULL
	, @FEEEARNER INT = NULL
	, @UI uUICultureInfo = '{default}'
	, @MAX_RECORDS INT = 50
	, @IncludeDeleted BIT = 0

	-- Parameters added 07/01/2009
	, @APPTYPE NVARCHAR(15) = NULL 
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
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @SELECT NVARCHAR(MAX)

IF @zDateStart IS NULL
	SET @zDateStart = '1900-01-01'
IF @zDateEnd IS NULL
	SET @zDateEnd = getutcdate() + 1


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

SELECT DocumentID INTO #R1 FROM config.searchDocumentAccess (''CONTACT'',@zContID) r

CREATE UNIQUE CLUSTERED INDEX AF1 ON #R1 ([DocumentID]);

IF OBJECT_ID(N''tempdb..#Res'', N''U'') IS NOT NULL
DROP TABLE #Res

CREATE TABLE #Res(
	  Id INT IDENTITY(1, 1) PRIMARY KEY
	, assocAddressee NVARCHAR(100)
	, clNo NVARCHAR(12)
	, fileNo NVARCHAR(20)
	, ClientFileNo NVARCHAR(50)
	, fileDesc NVARCHAR(255)
	, DocID BIGINT
	, DocDesc NVARCHAR(150)
	, DocType NVARCHAR(15)
	, Created DATETIME
	, docExtension NVARCHAR(15)
	, CrByFullName NVARCHAR(50)
	, doctypedesc NVARCHAR(50)
	, doccheckedout DATETIME
	, doccheckedoutby INT
	, doccheckedoutlocation NVARCHAR(255)
	, CrByInits NVARCHAR(30)
	, docdirection BIT
	, verLabel NVARCHAR(50)
	, docwallet NVARCHAR(50)
	, Updated DATETIME
	, UpdatedBy INT
	, UpdByInits NVARCHAR(30)
	, UpdByFullName NVARCHAR(50)
	, AuthByInits NVARCHAR(30)
	, AuthByFullName NVARCHAR(50)
	, SecurityOptions BIGINT
	, DocFlags SMALLINT
	, docFolder NVARCHAR(1000)
);

WITH Res AS
(	
SELECT 
	A.assocAddressee 
	, CL.clNo
	, F.fileNo
	, dbo.GetFileRef(CL.clNo, F.fileNo) as ClientFileNo
	, F.fileDesc
	, D.DocID
	, D.DocDesc
	, D.DocType
	, D.Created
	, D.docExtension
	, U.usrFullName as CrByFullName
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(D.docType, '''') + ''~'') AS doctypedesc
	, D.doccheckedout
	, D.doccheckedoutby
	, D.doccheckedoutlocation
	, U.usrInits AS CrByInits
	, D.docdirection
	, V.verLabel
	, COALESCE(CL2.cdDesc, ''~'' + NULLIF(D.docwallet, '''') + ''~'') AS docwallet
	, D.Updated
	, D.UpdatedBy
	, Up.usrInits AS UpdByInits 
	, Up.usrFullName AS UpdByFullName
	, Ua.usrInits AS AuthByInits 
	, Ua.usrFullName AS AuthByFullName
	, D.SecurityOptions
	, D.DocFlags
	, COALESCE(CL3.cdDesc, ''~'' + NULLIF(fc.FolderCode, '''') + ''~'') AS docFolder
FROM config.dbdocument D
	INNER JOIN config.dbfile F ON D.fileID = F.fileID
	INNER JOIN config.dbAssociates A ON A.assocID = D.assocID
	INNER JOIN config.dbClient CL ON CL.clID = D.clID
	INNER JOIN dbo.dbUser U ON D.CreatedBy = U.usrID
	LEFT OUTER JOIN dbo.dbUser Up ON D.UpdatedBy = Up.usrID
	LEFT OUTER JOIN dbo.dbApplication App ON App.appID = D.docAppID
	LEFT OUTER JOIN	dbo.dbDocumentVersion V ON v.verID = D.docCurrentVersion and V.DocID = D.DocID
	LEFT OUTER JOIN	dbo.dbUser Ua ON D.docAuthoredBy = Ua.usrID
	INNER JOIN  #R1 r on r.DocumentID = D.docid
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''DOCTYPE'', @UI) CL1 ON CL1.cdCode = D.docType
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''WALLET'', @UI) CL2 ON CL2.cdCode = D.docwallet
	LEFT OUTER JOIN dbo.dbFileFolder fc ON fc.fileID = D.FileID AND fc.FolderGuid = D.docFolderGUID
    LEFT JOIN dbo.GetCodeLookupDescription(''DFLDR_MATTER'', @UI) CL3 ON cl3.cdCode = fc.FolderCode
WHERE A.contID = @zContID
	'
-- Document Type
IF @zDocType IS NOT NULL
	SET @SELECT = @SELECT + N'AND D.docType = @zDocType
	'

-- Description
IF ISNULL(@DESC, '') <> ''
	SET @SELECT = @SELECT + N'AND D.DocDesc LIKE ''%'' + @DESC + ''%''
	'

-- Document Created By
IF @zCreatedBy IS NOT NULL
	SET @SELECT = @SELECT + N'AND D.CreatedBy = @zCreatedBy
	'

-- Wallet
IF @DOCWALLET IS NOT NULL
	SET @SELECT = @SELECT + N'AND D.docWallet = @DOCWALLET
	'

-- Department for FILE
IF @DEPT IS NOT NULL
	SET @SELECT = @SELECT + N'AND F.filedepartment = @DEPT
	'

-- FILE type
IF @FILETYPE IS NOT NULL
	SET @SELECT = @SELECT + N'AND F.fileType = @FILETYPE
	'

-- FILE handler
IF @FEEEARNER IS NOT NULL
	SET @SELECT = @SELECT + N'AND F.filePrincipleID = @FEEEARNER
	'

-- Date Created Range
IF @zDateStart IS NOT NULL
	SET @SELECT = @SELECT + N'AND D.Created >= @zDateStart 
	AND D.Created < @zDateEnd
	'

--------------------------------------
-- Additional filters applied to WHERE clasue 07/01/2009

-- Application Type of the Document
IF @APPTYPE IS NOT NULL
	SET @SELECT = @SELECT + N'AND D.docAppID = @APPTYPE
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

-- Document FolderCode Filter
--------------------------------------
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
	SET  @SELECT =  @SELECT + N'ORDER BY Updated DESC'
ELSE 
	IF @ORDERBY NOT LIKE '%Updated%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', Updated DESC'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

SET @SELECT = @SELECT + N'

SET @Total = @@ROWCOUNT

SELECT ' + CASE WHEN ISNULL(@MAX_RECORDS, 0) > 0 THEN N'TOP(@MAX_RECORDS)' ELSE N'' END + N'
	res.assocAddressee 
	, res.clNo
	, res.fileNo
	, res.ClientFileNo
	, res.fileDesc
	, res.DocID
	, res.DocDesc
	, res.DocType
	, res.Created
	, res.docExtension
	, res.CrByFullName
	, res.doctypedesc
	, res.doccheckedout
	, res.doccheckedoutby
	, res.doccheckedoutlocation
	, res.CrByInits
	, res.docdirection
	, res.verLabel
	, res.docwallet
	, res.Updated
	, res.UpdatedBy
	, res.UpdByInits 
	, res.UpdByFullName
	, res.AuthByInits 
	, res.AuthByFullName
	, res.SecurityOptions
	, res.DocFlags
	, res.docFolder
	, @Total AS Total
FROM #Res res
WHERE res.Id > @OFFSET 
'

PRINT @SELECT

EXEC sp_executesql @SELECT,  N'
	  @zDateStart DATETIME
	, @zDateEnd DATETIME
	, @zContID bigint
	, @zCreatedBy INT
	, @zDocType NVARCHAR(15)
	, @DESC NVARCHAR(100)
	, @DOCWALLET NVARCHAR(15)
	, @DEPT NVARCHAR(15)
	, @FILETYPE NVARCHAR(15)
	, @FEEEARNER INT
	, @UI uUICultureInfo
	, @MAX_RECORDS INT

	, @APPTYPE NVARCHAR(15)
	, @UPDATEDSTART DATETIME
	, @UPDATEDEND DATETIME
	, @UPDATEDBY INT
	, @AUTHOREDSTART DATETIME
	, @AUTHOREDEND DATETIME
	, @AUTHOREDBY INT
	, @FolderCode NVARCHAR(15)
	, @PageNo INT',

	  @zDateStart
	, @zDateEnd
	, @zContID
	, @zCreatedBy
	, @zDocType
	, @DESC
	, @DOCWALLET
	, @DEPT
	, @FILETYPE
	, @FEEEARNER
	, @UI
	, @MAX_RECORDS
	-- Parameters added 07/01/2009
	, @APPTYPE 
	, @UPDATEDSTART
	, @UPDATEDEND
	, @UPDATEDBY 
	, @AUTHOREDSTART
	, @AUTHOREDEND
	, @AUTHOREDBY
	--Parameters added 31/10/2017
	, @FolderCode
	, @PageNo