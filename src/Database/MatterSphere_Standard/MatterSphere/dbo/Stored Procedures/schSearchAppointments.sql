CREATE PROCEDURE dbo.schSearchAppointments(
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
WITH Res AS
(
SELECT 
	A.*
	, U.usrFullName
	, CASE A.appAllDay WHEN 0 THEN A.appDate ELSE NULL END AS appTime
	, COALESCE(CL.cdDesc, ''~'' + NULLIF(A.appType, '''') + ''~'') AS apptypedesc
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.filestatus, '''') + ''~'') AS fileStatDesc
	, COALESCE(CL2.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') AS fileTypeDesc
	, COALESCE(CL3.cdDesc, ''~'' + NULLIF(F.fileDepartment, '''') + ''~'') AS filedeptdesc
	, C.clNo + ''/'' + F.fileNo AS clfileno
	, F.fileDesc
	, C.clName
FROM dbo.dbAppointments A
	INNER JOIN dbo.dbFile F ON A.fileID = F.fileID 
	INNER JOIN dbo.dbUser U ON U.usrID = A.feeusrID
	INNER JOIN dbo.dbClient C ON C.clID = F.clID
	LEFT JOIN dbo.GetCodeLookupDescription(''APPTYPE'', @UI) CL ON CL.cdCode = A.appType
	LEFT JOIN dbo.GetCodeLookupDescription(''FILESTATUS'', @UI) CL1 ON CL1.cdCode = F.filestatus
	LEFT JOIN dbo.GetCodeLookupDescription(''FILETYPE'', @UI) CL2 ON CL2.cdCode = F.fileType
	LEFT JOIN dbo.GetCodeLookupDescription(''DEPT'', @UI) CL3 ON CL3.cdCode = F.fileDepartment
WHERE 1= 1
	'
IF @FILESTATUS IS NOT NULL
	SET @Select = @Select + N'AND F.filestatus = @FILESTATUS
	'

IF @FEEUSRID IS NOT NULL
	SET @Select = @Select + N'AND A.feeusrid = @FEEUSRID
	'
IF @DEPT IS NOT NULL
	SET @Select = @Select + N'AND F.filedepartment = @DEPT
	'
IF @FILETYPE IS NOT NULL
	SET @Select = @Select + N'AND F.filetype =@FILETYPE
	'

IF @DATERANGE = 'WITHIN7' 
	SET @Select = @Select + N'AND (A.appdate > GETUTCDATE() AND A.appdate < GETUTCDATE() + 7 )
	'
IF @DATERANGE = 'OVER7'
	SET @Select = @Select + N' AND (A.appdate > GETUTCDATE() + 7)
	'
IF @DATERANGE = 'OD'
	SET @Select = @Select + N'AND (A.appdate < GETUTCDATE() )
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
	SET  @Select =  @Select + N'ORDER BY appdate'
ELSE 
	IF @ORDERBY NOT LIKE '%appdate%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', appdate'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY
	
EXEC sp_executesql @Select,  N'@UI uUICultureInfo, @MAX_RECORDS INT, @FEEUSRID BIGINT, @FILESTATUS uCodeLookup, @DEPT uCodeLookup, @FILETYPE uCodeLookup', @UI, @MAX_RECORDS, @FEEUSRID, @FILESTATUS, @DEPT, @FILETYPE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchAppointments] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchAppointments] TO [OMSAdminRole]
    AS [dbo];

