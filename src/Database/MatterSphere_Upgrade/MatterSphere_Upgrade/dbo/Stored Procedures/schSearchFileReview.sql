CREATE PROCEDURE dbo.schSearchFileReview(
	@UI uUICultureInfo = '{default}'
	, @MAX_RECORDS INT = 0
	, @FEEUSRID BIGINT
	, @FILESTATUS uCodeLookup = NULL
	, @DEPT uCodeLookup = NULL
	, @FILETYPE uCodeLookup = NULL
	, @DATERANGE uCodeLookup = NULL	
	, @ORDERBY NVARCHAR(MAX) = NULL
	) 
AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @Select NVARCHAR(MAX)

SET @Select = N'
WITH Res AS(
SELECT C.clID
	, C.clNo
	, C.clName
	, F.fileNo
	, F.filetype
	, F.fileid
	, F.fileFundcode
--	, F.clid
	, F.filedesc
	, F.created
	, F.filestatus
	, U.usrfullname
	, C.clNo + ''/'' + F.fileNo AS clfileno
	, F.fileacccode
	, F.fileDepartment
	, COALESCE(CL4.cdDesc, ''~'' + NULLIF(F.fileDepartment, '''') + ''~'') AS DeptDesc
	, F.fileCreditLimit
	, F.filecurISOCode
	, F.filewip
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileFundCode, '''') + ''~'') AS FundTypeDesc
	, COALESCE(CL2.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') AS FileTypeDesc
	, COALESCE(CL3.cdDesc, ''~'' + NULLIF(F.fileStatus, '''') + ''~'') AS FileStatusDesc
	, F.fileno + '' : '' + F.filedesc AS fileJointDesc
	, F.filereviewdate
FROM  dbo.dbClient C 
	INNER JOIN dbo.dbFile F ON F.clID = C.clID 
	INNER JOIN dbo.dbuser U ON  U.usrid = F.fileprincipleid
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''FUNDTYPE'', @UI) CL1 ON CL1.cdCode =  F.fileFundCode
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''FILETYPE'', @UI) CL2 ON CL2.cdCode = F.fileType
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''FILESTATUS'', @UI) CL3 ON CL3.cdCode = F.fileStatus
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''DEPT'', @UI) CL4 ON CL4.cdCode = F.fileDepartment
WHERE 1= 1
	'
IF @FILESTATUS IS NOT NULL
	SET @Select = @Select + N'AND F.filestatus = @FILESTATUS
	'
IF @FEEUSRID IS NOT NULL
	SET @Select = @Select + N'AND F.fileprincipleID = @FEEUSRID
	'
IF @DEPT IS NOT NULL
	SET @Select = @Select + N'AND F.filedepartment = @DEPT
	'
IF @FILETYPE IS NOT NULL
	SET @Select = @Select + N'AND F.filetype =@FILETYPE
	'

IF @DATERANGE = 'WITHIN7' 
	SET @Select = @Select + N'AND F.filereviewdate > GETUTCDATE() AND F.filereviewdate < GETUTCDATE() + 7
	'
ELSE 
IF @DATERANGE = 'OVER7'
	SET @Select = @Select + N'AND F.filereviewdate > GETUTCDATE() + 7
	'
ELSE
IF @DATERANGE = 'OD'
	SET @Select = @Select + N' AND F.filereviewdate < GETUTCDATE()
	'
ELSE
	SET @Select = @Select + N' AND F.filereviewdate IS NOT NULL
	'

SET @Select = @Select + N'
)'

IF @MAX_RECORDS > 0
	SET @Select =  @Select + N'
SELECT TOP (@MAX_RECORDS) *
FROM Res
'
ELSE
	SET @Select =  @Select + N'
SELECT *
FROM Res
'

IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY filereviewdate'
ELSE 
	IF @ORDERBY NOT LIKE '%filereviewdate%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', filereviewdate'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY


EXEC sp_executesql @Select,  N'@UI uUICultureInfo, @MAX_RECORDS INT, @FEEUSRID BIGINT, @FILESTATUS uCodeLookup, @DEPT uCodeLookup, @FILETYPE uCodeLookup, @DATERANGE uCodeLookup', @UI, @MAX_RECORDS, @FEEUSRID, @FILESTATUS, @DEPT, @FILETYPE, @DATERANGE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchFileReview] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchFileReview] TO [OMSAdminRole]
    AS [dbo];

