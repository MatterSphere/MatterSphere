CREATE PROCEDURE [dbo].[schSearchDocAll] (
	@CURRENTUSER INT = NULL
	, @UI uUICultureInfo = '{default}'
	, @MAX_RECORDS INT = 50
	, @DESC NVARCHAR(100) = ''
	, @CREATEDBY INT = NULL
	, @DOCTYPE NVARCHAR(15) = NULL
	, @CHECKEDOUTBY INT = NULL
	, @STARTDATE DATETIME = NULL
	, @ENDDATE DATETIME = NULL
	, @DOCWALLET NVARCHAR(15) = ''
	, @DEPT NVARCHAR(15) = ''
	, @FILETYPE NVARCHAR(15) = ''
	, @FEEEARNER INT = NULL
	, @IN BIT = 1
	, @OUT BIT = 1
	, @IncludeDeleted BIT = 0
	, @OrderByUpdated BIT = 0
	, @OrderByOpened BIT = 0

	, @APPTYPE SMALLINT = NULL
	, @UPDATEDSTART DATETIME = NULL
	, @UPDATEDEND DATETIME = NULL
	, @UPDATEDBY INT = NULL
	, @AUTHOREDSTART DATETIME = NULL
	, @AUTHOREDEND DATETIME = NULL
	, @AUTHOREDBY INT = NULL

	--Parameters added 31/10/17
	, @FolderCode NVARCHAR(15) = NULL
	--Parameters added 17/08/20
	, @ORDERBY NVARCHAR(200) = NULL
	--Parameters added 25/08/22
	, @PageNo INT = NULL
	)  
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @SELECT NVARCHAR(MAX), @SELECT_ORDER NVARCHAR(MAX), @SORT_FIELD NVARCHAR(200), @SORT_ORDER NVARCHAR(10)

--IF config.dbDocument CONTAINS MORE THAN 1.5M RECORDS YOU NEED TO PASS A PARAMETER TO CONTINUE.
IF (ISNULL(@DESC,'') = '' AND @CREATEDBY IS NULL AND @DOCTYPE IS NULL AND @CHECKEDOUTBY IS NULL AND ISNULL(@DEPT,'') = '' AND ISNULL(@FILETYPE,'') = ''
	AND @FEEEARNER IS NULL AND ISNULL(@DOCWALLET,'') = '' AND @STARTDATE IS NULL AND @ENDDATE IS NULL 
	AND @APPTYPE IS NULL AND @UPDATEDSTART IS NULL AND @UPDATEDEND IS NULL AND @UPDATEDBY IS NULL 
	AND @AUTHOREDSTART IS NULL AND @AUTHOREDEND IS NULL AND @AUTHOREDBY IS NULL AND @FolderCode IS NULL)
BEGIN
	DECLARE @RowCount BIGINT, @severity TINYINT
	SELECT @RowCount = [dbo].[GetTableRowCount]('config.dbDocument')
	IF (@RowCount >= 1500000)
	BEGIN
		EXECUTE @severity = sprRaiseError 'MSGSEARCHDOCALL', @UI, @SELECT OUT
		IF @SELECT IS NULL SET @SELECT = 'Please provide at least one filter parameter to continue.'
		RAISERROR (@SELECT, @severity, 1)
		RETURN
	END
END

IF @STARTDATE IS NULL
	SET @STARTDATE = '1900-01-01'
IF @ENDDATE IS NULL
	SET @ENDDATE = getutcdate() + 1
ELSE
	SET @ENDDATE = @ENDDATE + 1

-- ORDER BY 
IF ISNULL(@ORDERBY, '') = ''
BEGIN
	IF @OrderByUpdated = 1
		SET @SELECT_ORDER = N'ORDER BY D.Updated DESC'
	ELSE IF @OrderByOpened = 1
		SET @SELECT_ORDER = N'ORDER BY D.Opened DESC'
	ELSE
		SET @SELECT_ORDER = N'ORDER BY D.Created DESC'
END
ELSE
BEGIN
	SET @SORT_FIELD = LTRIM(RTRIM(REPLACE(REPLACE(@ORDERBY,' ASC',''),' DESC','')))
	SET @SORT_ORDER = SUBSTRING(@ORDERBY, LEN(@SORT_FIELD) + 1, LEN(@ORDERBY) - LEN(@SORT_FIELD))

	SET @SORT_FIELD = CASE
		WHEN @SORT_FIELD IN ('docID','docDesc','docType','docExtension','Updated','UpdatedBy','Opened','docDirection','CreatedBy',
							 'docCheckedOut','docCheckedOutBy','docCheckedOutLocation','docAuthored','docFlags','SecurityOptions')
			THEN N'D.' + @SORT_FIELD
		WHEN @SORT_FIELD = 'crfield'
			THEN N'D.Created'
		WHEN @SORT_FIELD = 'Created'
			THEN N'COALESCE(D.docAuthored, D.Created)'
		WHEN @SORT_FIELD = 'docToken'
			THEN N'D.docFileName'
		WHEN @SORT_FIELD = 'CrByFullName'
			THEN N'UC.usrFullName'
		WHEN @SORT_FIELD = 'UpdByFullName'
			THEN N'UU.usrFullName'
		WHEN @SORT_FIELD = 'AuthByInits'
			THEN N'auth.usrInits'
		WHEN @SORT_FIELD = 'AuthByFullName'
			THEN N'auth.usrFullName'
		WHEN @SORT_FIELD = 'docTypeDesc'
			THEN N'COALESCE(cl1.cdDesc, ''~'' + D.docType + ''~'')'
		WHEN @SORT_FIELD = 'docWallet'
			THEN N'COALESCE(cl2.cdDesc, ''~'' + D.docWallet + ''~'')'
		WHEN @SORT_FIELD = 'docFolder'
			THEN N'COALESCE(cl3.cdDesc, ''~'' + NULLIF(fc.FolderCode, '''') + ''~'')'
		ELSE @SORT_FIELD
	END

	IF (@ORDERBY LIKE '%crfield%' OR @ORDERBY LIKE '%Created %' OR @ORDERBY = 'Created')
		SET @SELECT_ORDER = N'ORDER BY ' + @SORT_FIELD + @SORT_ORDER
	ELSE IF (@ORDERBY LIKE '%ClientFileNo%')
		SET @SELECT_ORDER = N'ORDER BY clNo' + @SORT_ORDER + N', fileNo' + @SORT_ORDER
	ELSE
		SET @SELECT_ORDER = N'ORDER BY ' + @SORT_FIELD + @SORT_ORDER + N', D.Created DESC'
END

SET @SELECT = N'
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @OFFSET INT = 0, @TOP INT

IF @MAX_RECORDS > 0
	SET @TOP = @MAX_RECORDS
ELSE
	SET @TOP = 50

IF @PageNo IS NULL
	SET @OFFSET = 0 
ELSE
	SET @OFFSET = @TOP * (@PageNo - 1)

DECLARE @Res TABLE (docID BIGINT
	, Id BIGINT
	, Total INT)

IF OBJECT_ID(N''tempdb..#R1'', N''U'') IS NOT NULL
DROP TABLE #R1

CREATE TABLE #R1 (DocumentID BIGINT) 

DECLARE @USER NVARCHAR(200)
SET @USER = config.GetUserLogin()
DECLARE @Sec INT
SELECT @SEC = config.IsAdministrator(@User)

IF @Sec = 0
BEGIN
	--OBTAIN DENY AND SECURED DocID
	;WITH [DocumentAllowDeny] ( [DocumentID], [Allow], [Deny], [Secure] ) AS
	(
		SELECT 
				RUGD.[DocumentID],
				MAX(CASE WHEN Substring ( PC.AllowMask , 9 , 1 ) & 128 = 128 AND UGM.ID IS NOT NULL THEN 1 ELSE NULL END) as [Allow] ,
				MAX(CASE WHEN Substring ( PC.DenyMask , 9 , 1 ) & 128 = 128 AND UGM.ID IS NOT NULL THEN 1 ELSE NULL END) as [Deny] ,
				CASE WHEN MAX(CASE WHEN UGM.ID IS NOT NULL THEN 1 END) IS NULL THEN 1 END AS Secure
		FROM
				[relationship].[UserGroup_Document] RUGD
		INNER JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID]
		LEFT JOIN
				[config].[GetUserAndGroupMembershipNT_NS] () UGM ON RUGD.[UserGroupID] = UGM.[ID]
		WHERE (PC.IsRemote = 0 OR PC.IsRemote IS NULL)
		GROUP BY [DocumentID]
	)
	INSERT INTO #R1 (DocumentID)
	SELECT [DocumentID]
	FROM [DocumentAllowDeny] DA
		INNER JOIN config.dbDocument D ON D.docID = DA.DocumentID
	WHERE (DA.[Deny] = 1) OR (DA.Secure = 1) 
		AND ISNULL((SELECT TOP 1 ISNULL(PC.IsRemote, 0) from [relationship].[UserGroup_File] RUGF JOIN [config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID] WHERE FileID = d.FileID ORDER BY PC.IsRemote), 1) = 1

	IF EXISTS (SELECT 1 FROM dbRegInfo WHERE regBlockInheritence = 1)
	BEGIN
		INSERT INTO #R1 (DocumentID)
		SELECT 
			D.DocID
		FROM
			[relationship].[UserGroup_File] RUGF
		JOIN
			[config].[dbDocument] D ON RUGF.FileID = D.fileID
		JOIN
			[config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID] 
		JOIN
			[config].[GetUserAndGroupMembershipNT] (@USER) UGM ON RUGF.[UserGroupID] = UGM.[ID]
		LEFT JOIN
			#R1 ED ON ED.DocumentID = D.docID
		WHERE (PC.IsRemote = 0 OR PC.IsRemote IS NULL) 
			AND ED.DocumentID IS NULL
		GROUP BY D.DocID
		HAVING SUM(SUBSTRING(PC.AllowMask , 9 , 1) & 128) = 0 AND SUM(SUBSTRING(PC.DenyMask , 9 , 1) & 128) > 0  
	END
END

CREATE NONCLUSTERED INDEX IX_TEMP_R1 ON #R1(DocumentID)

;WITH Res AS
(
	SELECT
		  D.docID
		'
SET @SELECT = @SELECT + N', ROW_NUMBER() OVER ( ' + @SELECT_ORDER + N' ) AS Id
		, COUNT(1) OVER() AS Total
	FROM config.dbDocument D
		INNER JOIN dbo.dbFile F ON F.FileID = D.FileID
		INNER JOIN dbo.dbClient C ON C.clID = F.clID
		INNER JOIN dbo.dbAssociates A ON A.assocID = D.assocID
		INNER JOIN dbo.dbUser UC ON UC.usrID = D.CreatedBy
		'
IF (@ORDERBY LIKE '%docTypeDesc%')
	SET @SELECT = @SELECT + N'LEFT JOIN dbo.GetCodeLookupDescription(''DOCTYPE'', @UI) cl1 ON cl1.cdCode = D.docType
		'
IF (@ORDERBY LIKE '%docWallet%')
	SET @SELECT = @SELECT + N'LEFT JOIN dbo.GetCodeLookupDescription(''WALLET'', @UI) cl2 ON cl2.cdCode = D.docWallet
		'
IF (@FolderCode IS NOT NULL OR @ORDERBY LIKE '%docFolder%')
	SET @SELECT = @SELECT + N'LEFT JOIN dbo.dbFileFolder fc ON fc.fileID = D.FileID AND fc.FolderGuid = D.docFolderGUID
		LEFT JOIN dbo.GetCodeLookupDescription(''DFLDR_MATTER'', @UI) cl3 ON cl3.cdCode = fc.FolderCode
		'
IF (@ORDERBY LIKE '%verLabel%')
	SET @SELECT = @SELECT + N'LEFT JOIN dbo.dbDocumentVersion v ON v.verID = D.docCurrentVersion AND v.DocID = D.DocID
		'
IF (@ORDERBY LIKE '%UpdByFullName%')
	SET @SELECT = @SELECT + N'LEFT JOIN dbo.dbUser UU ON UU.usrID = D.UpdatedBy
		'
IF (@ORDERBY LIKE '%AuthBy%')
	SET @SELECT = @SELECT + N'LEFT JOIN dbo.dbUser auth ON auth.usrID = D.docAuthoredBy
		'
SET @SELECT = @SELECT + N'LEFT JOIN #R1 r ON r.DocumentID = D.docID
	WHERE r.DocumentID IS NULL
	'

IF @DESC <> '' --and not @DESC is NULL
BEGIN
	SET @SELECT = @SELECT + N'AND D.DocDesc LIKE ''%'' + @DESC + ''%''
	'
END

IF @CREATEDBY IS NOT NULL
BEGIN
	IF @OrderByUpdated = 0
		SET @SELECT = @SELECT + N'AND D.CreatedBy = @CREATEDBY
	'
	ELSE
		SET @SELECT = @SELECT + N'AND D.UpdatedBy = @CREATEDBY
	'
END

IF @DOCTYPE IS NOT NULL
BEGIN
	SET @SELECT = @SELECT + N'AND D.docType = @DOCTYPE
	'
END

IF @CHECKEDOUTBY IS NOT NULL
BEGIN
	SET @SELECT = @SELECT + N'AND D.docCheckedOutBy = @CHECKEDOUTBY
	'
END

IF @IN = 1 AND @OUT = 0 
BEGIN
	SET @SELECT = @SELECT + N'AND D.docDirection = 1
	'
END

IF @IN = 0 and @OUT = 1
BEGIN
	SET @SELECT = @SELECT + N'AND D.docDirection = 0
	'
END

-- New code
IF @IncludeDeleted = 0
BEGIN
	SET @SELECT = @SELECT + N'AND D.docDeleted = 0
	'
END

-- Date Created Range
IF @STARTDATE IS NOT NULL
BEGIN
	SET @SELECT = @SELECT + N'AND D.Created >= @STARTDATE AND D.Created < @ENDDATE
	'
END

-- Wallet
IF @DOCWALLET <> ''
BEGIN
	SET @SELECT = @SELECT + N'AND D.docWallet = @DOCWALLET
	'
END

-- Department for FILE
IF @DEPT <> ''
BEGIN
	SET @SELECT = @SELECT + N'AND F.fileDepartment = @DEPT
	'
END

-- FILE type
IF @FILETYPE <> ''
BEGIN
	SET @SELECT = @SELECT + N'AND F.fileType = @FILETYPE
	'
END

-- FILE handler
IF @FEEEARNER IS NOT NULL
BEGIN
	SET @SELECT = @SELECT + N'AND F.filePrincipleID = @FEEEARNER
	'
END

-- Additional filters applied 26.11.08
-- Date Updated Range
IF @UPDATEDSTART IS NOT NULL
BEGIN
	IF @UPDATEDEND IS NULL
		SET @SELECT = @SELECT + N'AND D.Updated >= @UPDATEDSTART
	'
	ELSE
		SET @SELECT = @SELECT + N'AND D.Updated >= @UPDATEDSTART AND D.Updated < @UPDATEDEND
	'
END
ELSE IF @UPDATEDEND IS NOT NULL
BEGIN
	SET @SELECT = @SELECT + N'AND D.Updated < @UPDATEDEND
	'
END

-- Document Updated By
IF @UPDATEDBY IS NOT NULL
BEGIN
	SET @SELECT = @SELECT + N'AND D.UpdatedBy = @UPDATEDBY
	'
END

-- Date Authored Range
IF @AUTHOREDSTART IS NOT NULL
BEGIN
	IF @AUTHOREDEND IS NULL
		SET @SELECT = @SELECT + N'AND D.docAuthored >= @AUTHOREDSTART
	'
	ELSE
		SET @SELECT = @SELECT + N'AND D.docAuthored >= @AUTHOREDSTART AND D.docAuthored < @AUTHOREDEND
	'
END
ELSE IF @AUTHOREDEND IS NOT NULL
BEGIN
	SET @SELECT = @SELECT + N'AND D.docAuthored < @AUTHOREDEND
	'
END

-- Document Authored By
IF @AUTHOREDBY IS NOT NULL
BEGIN
	SET @SELECT = @SELECT + N'AND D.docAuthoredBy = @AUTHOREDBY
	'
END

-- Application Type of the Document
IF @APPTYPE IS NOT NULL
BEGIN
	SET @SELECT = @SELECT + N'AND D.docAppID = @APPTYPE
	'
END

-- Document FolderCode Filter
--------------------------------------
IF @FolderCode IS NOT NULL
BEGIN
	SET @SELECT = @SELECT + N'AND fc.FolderCode = ''' + @FolderCode + N'''
	'
END

--------------------------------------
SET @SELECT = @SELECT + N'
)
INSERT INTO @Res
SELECT ' + CASE WHEN ISNULL(@MAX_RECORDS, 0) > 0 THEN N'TOP(@MAX_RECORDS)' ELSE N'' END + N'
	R.docID
	, R.Id
	, R.Total
FROM Res R
WHERE Id > @OFFSET
'

--PRINT @SELECT

--------------------------------------

SET @SELECT = @SELECT + N'
OPTION (RECOMPILE)

SELECT
	COALESCE(cl1.cdDesc, ''~'' + DC.docType + ''~'') AS docTypeDesc, 
	DC.docID, 
	DC.docDesc, 
	DC.docType, 
	CASE WHEN SUBSTRING(LTRIM(DC.docExtension), 1, 1) = ''.'' THEN LOWER(STUFF(LTRIM(DC.docExtension), 1, 1, '''')) ELSE LOWER(DC.docExtension) END AS docExtension,
	COALESCE(DC.docAuthored, DC.Created) AS Created, 
	DC.Updated, 
	DC.UpdatedBy,
	DC.Opened,
	DC.Created as crfield,
	DC.docDirection, 
	CL.clNo, 
	FL.fileNo, 
	dbo.GetFileRef(CL.clNo, FL.fileNo) as ClientFileNo,
	CL.clName,
	FL.fileDesc,
	DC.CreatedBy,
	DC.docFileName as docToken,
	DC.docCheckedOut,
	DC.docCheckedOutBy,
	DC.docCheckedOutLocation,
	COALESCE(cl2.cdDesc, ''~'' + DC.docWallet + ''~'') AS docWallet, 
	UC.usrFullName AS CrByFullName,
	UU.usrFullName AS UpdByFullName,
	V.verLabel,
	auth.usrInits AS AuthByInits, 
	auth.usrFullName AS AuthByFullName,
	DC.SecurityOptions,
	DC.docFlags,
	COALESCE(cl3.cdDesc, ''~'' + NULLIF(fc.FolderCode, '''') + ''~'') AS docFolder,
	res.Total
FROM @Res res
	INNER JOIN config.dbDocument DC ON DC.docID = res.docID
	INNER JOIN config.dbFile FL ON FL.fileID = DC.fileID
	INNER JOIN config.dbClient CL ON CL.clID = FL.clID
	INNER JOIN dbo.dbUser UC ON UC.usrID = DC.CreatedBy
	LEFT JOIN dbo.dbUser UU ON UU.usrID = DC.UpdatedBy
--	LEFT JOIN dbo.dbApplication A ON A.appID = DC.docAppID
	LEFT JOIN dbo.dbDocumentVersion V ON V.verID = DC.docCurrentVersion AND V.DocID = DC.DocID
	LEFT JOIN dbo.dbUser auth ON auth.usrID = DC.docAuthoredBy
	LEFT JOIN [dbo].[GetCodeLookupDescription](''DOCTYPE'', @UI) cl1 ON cl1.cdCode = DC.docType
	LEFT JOIN [dbo].[GetCodeLookupDescription](''WALLET'', @UI) cl2 ON cl2.cdCode = DC.docwallet
	LEFT JOIN dbo.dbFileFolder fc ON fc.fileID = DC.FileID AND fc.FolderGuid = DC.docFolderGUID
	LEFT JOIN [dbo].[GetCodeLookupDescription](''DFLDR_MATTER'', @UI) cl3 ON cl3.cdCode = fc.FolderCode
ORDER BY res.Id
'

--PRINT @SELECT

EXEC sp_executesql @SELECT,  N'
	@CURRENTUSER INT
	, @UI uUICultureInfo
	, @MAX_RECORDS INT
	, @DESC NVARCHAR(100)
	, @CREATEDBY INT 
	, @DOCTYPE NVARCHAR(15)
	, @IN BIT
	, @OUT BIT
	, @CHECKEDOUTBY INT 
	, @STARTDATE DATETIME
	, @ENDDATE DATETIME
	, @DOCWALLET NVARCHAR(15)
	, @DEPT NVARCHAR(15)
	, @FILETYPE NVARCHAR(15)
	, @FEEEARNER INT

	, @APPTYPE SMALLINT
	, @UPDATEDSTART DATETIME
	, @UPDATEDEND DATETIME
	, @UPDATEDBY INT
	, @AUTHOREDSTART DATETIME
	, @AUTHOREDEND DATETIME
	, @AUTHOREDBY INT
	, @FolderCode NVARCHAR(15)
	, @PageNo INT'

	, @CURRENTUSER
	, @UI
	, @MAX_RECORDS
	, @DESC
	, @CREATEDBY
	, @DOCTYPE
	, @IN
	, @OUT
	, @CHECKEDOUTBY
	, @STARTDATE
	, @ENDDATE
	, @DOCWALLET
	, @DEPT
	, @FILETYPE
	, @FEEEARNER
	, @APPTYPE
	, @UPDATEDSTART
	, @UPDATEDEND
	, @UPDATEDBY
	, @AUTHOREDSTART
	, @AUTHOREDEND
	, @AUTHOREDBY
	--Parameters added 31/10/2017
	, @FolderCode
	--Parameters added 25/08/22
	, @PageNo

