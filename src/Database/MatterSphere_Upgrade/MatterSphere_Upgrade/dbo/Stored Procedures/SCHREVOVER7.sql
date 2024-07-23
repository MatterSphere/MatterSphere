CREATE PROCEDURE dbo.SCHREVOVER7
(
	@UI uUICultureInfo = '{default}'
	, @FEEID BIGINT
	, @Department uCodeLookup = NULL
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
	C.clID
	, dbo.GetFileRef(C.clNo, F.fileNo) AS fileRef
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
	, F.fileno + '' : '' + F.filedesc AS [fileJointDesc]
	, F.filereviewdate
FROM dbo.dbClient C
	INNER JOIN dbo.dbfile F ON F.clID = C.clID
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''FUNDTYPE'', @UI) CL1 ON CL1.cdCode = F.fileFundCode
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''FILETYPE'', @UI) CL2 ON CL2.cdCode = F.fileType
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''FILESTATUS'', @UI) CL3 ON CL3.cdCode = F.fileStatus
WHERE F.filereviewdate > GETDATE() + 7
	AND F.filePrincipleID = @FEEID
'

IF(@Department IS NOT NULL)
	SET @SELECT = @SELECT + 'AND F.fileDepartment = @Department
	'
SET @SELECT = @SELECT + N'
)
SELECT *
FROM Res
'
--- SET THE ORDERBY CLAUSE
IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY filereviewdate'
ELSE 
	IF @ORDERBY NOT LIKE '%filereviewdate%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', filereviewdate'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @SELECT, N'@UI uUICultureInfo, @FEEID BIGINT, @Department uCodeLookup', @UI, @FEEID, Department

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHREVOVER7] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHREVOVER7] TO [OMSAdminRole]
    AS [dbo];
