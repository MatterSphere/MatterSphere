CREATE PROCEDURE dbo.SCHFEEFILELIST
(
	@UI uUICultureInfo = '{default}'
	, @FEEID BIGINT = NULL
	, @FILEDEPT uCodeLookup = NULL
	, @FILETYPE uCodeLookup = NULL
	, @ORDERBY NVARCHAR(MAX) = NULL
)

AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @SELECT NVARCHAR(MAX)

--- SET THE SELECT CLAUSE
SET @SELECT = N'WITH Res AS
(
SELECT     
	C.clID, 
	dbo.GetFileRef(C.clNo, F.fileNo) AS fileRef
	, C.clName
	, F.filetype
	, F.fileid
	, F.fileFundcode
	, F.filedesc
	, F.created
	, F.filestatus
	, F.fileacccode
	, F.fileCreditLimit
	, F.filecurISOCode
	, F.filewip
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileFundCode, '''') + ''~'') AS FundTypeDesc
	, COALESCE(CL2.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') AS FileTypeDesc
	, COALESCE(CL3.cdDesc, ''~'' + NULLIF(F.fileStatus, '''') + ''~'') AS FileStatusDesc
	, F.fileno + '' : '' + F.filedesc AS fileJointDesc
FROM dbo.dbClient C
	INNER JOIN dbo.dbFile F ON F.clID = C.clID
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''FUNDTYPE'', @UI) CL1 ON CL1.cdCode = F.fileFundCode
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''FILETYPE'', @UI) CL2 ON CL2.cdCode = F.fileType
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''FILESTATUS'', @UI) CL3 ON CL3.cdCode = F.fileStatus
WHERE 1 = 1 
'
--- FILE FEE EARNER CLAUSE
IF(@FEEID IS NOT NULL)
	SET @SELECT = @SELECT + N'AND F.filePrincipleID = @FEEID
	'

--- DEPARTMENT CLAUSE
IF(@FILEDEPT IS NOT NULL)
	SET @SELECT = @SELECT + N'AND F.fileDepartment = @FILEDEPT
	'
--- FILE TYPE CLAUSE
IF(@FILETYPE IS NOT NULL)
	SET @SELECT = @SELECT + N'AND F.fileType = @FILETYPE
	'
SET @SELECT = @SELECT + N'
)
SELECT *
FROM Res
'
--- SET THE ORDERBY CLAUSE
IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY fileRef'
ELSE 
	IF @ORDERBY NOT LIKE '%fileRef%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', fileRef'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @SELECT, N'@UI uUICultureInfo, @FEEID BIGINT, @FILEDEPT uCodeLookup, @FILETYPE uCodeLookup', @UI, @FEEID, @FILEDEPT, @FILETYPE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFEEFILELIST] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFEEFILELIST] TO [OMSAdminRole]
    AS [dbo];
