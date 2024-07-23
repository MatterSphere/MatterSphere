CREATE PROCEDURE dbo.fdSCHCONLINKAFS
(
	@UI uUICultureInfo = '{default}'
	, @clID BIGINT
	, @ORDERBY NVARCHAR(MAX) = NULL
)

AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @SELECT NVARCHAR(MAX)

--- SET THE SELECT CLAUSE
SET @SELECT = N'
WITH Res AS
(
SELECT     
	CL.clID
	, dbo.GetFileRef(CL.clNo, F.fileNo) AS fileRef
	, CL.clName
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
	, F.fileNo
FROM dbo.dbClient CL
	INNER JOIN dbo.dbFile F ON F.clID = CL.clID
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''FUNDTYPE'', @UI) CL1 ON CL1.cdCode = F.fileFundCode
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''FILETYPE'', @UI) CL2 ON CL2.cdCode = F.fileType
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''FILESTATUS'', @UI) CL3 ON CL3.cdCode = F.fileStatus
WHERE CL.clID = @clID
)'

SET @SELECT =  @SELECT + N'
SELECT clID
	, fileRef
	, clName
	, filetype
	, fileid
	, fileFundcode
	, filedesc
	, created
	, filestatus
	, fileacccode
	, fileCreditLimit
	, filecurISOCode
	, filewip
	, FundTypeDesc
	, FileTypeDesc
	, FileStatusDesc
	, fileJointDesc
FROM Res
'
IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY fileNo'
ELSE 
	IF @ORDERBY NOT LIKE '%fileNo%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', fileNo'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @SELECT, N'@UI uUICultureInfo,  @clID BIGINT', @UI,  @clID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdSCHCONLINKAFS] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdSCHCONLINKAFS] TO [OMSAdminRole]
    AS [dbo];

