CREATE PROCEDURE dbo.schSearchTasks
(
	@UI uUICultureInfo = '{default}'
	, @MAX_RECORDS INT = 0
	, @FEEUSRID BIGINT = NULL
	, @FEEALLOCATEDTO BIGINT = NULL
	, @FILESTATUS uCodeLookup = NULL
	, @DEPT uCodeLookup = NULL
	, @FILETYPE uCodeLookup = NULL
	, @DATERANGE uCodeLookup = NULL
	, @TASKTYPE uCodeLookup = NULL
	, @ORDERBY NVARCHAR(MAX) = NULL
)

AS

SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @SELECT NVARCHAR(MAX)

--- SET THE SELECT CLAUSE
SET @SELECT = N' WITH Res AS
(
SELECT 
	CASE
		WHEN DATEDIFF(d, T.tskDue, GETUTCDATE()) < 1 THEN 0 
		WHEN DATEDIFF(d, T.tskDue, GETUTCDATE()) < 3 THEN 13
		ELSE 14
	END AS Priority
	, C.clName
	, F.fileDesc
	, T.*
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') AS fileTypeDesc
	, C.clNo + ''/'' + F.fileNo AS clfileno
	, TU.usrFullName as UsrTaskAllocatedTo
	, FU.usrFullName as fileHandler
	, COALESCE(CL2.cdDesc, ''~'' + NULLIF(F.fileDepartment, '''') + ''~'') AS fileDeptDesc
	, COALESCE(CL3.cdDesc, ''~'' + NULLIF(F.fileStatus, '''') + ''~'') AS fileStatDesc
	, COALESCE(CL4.cdDesc, ''~'' + NULLIF(T.tskType, '''') + ''~'') AS tsktypedesc
	, C.clNo
	, F.fileNo
FROM dbo.dbClient C
	INNER JOIN dbo.dbFile F ON C.clID = F.clID 
	INNER JOIN dbo.dbTasks T ON F.fileID = T.fileID 
	INNER JOIN dbo.dbUser as TU ON T.feeusrID = TU.usrID 
	INNER JOIN dbo.dbUser as FU ON F.fileprincipleID = FU.usrID
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''FILETYPE'', @UI) CL1 ON CL1.cdCode = F.fileType
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''DEPT'', @UI) CL2 ON CL2.cdCode = F.fileDepartment
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''FILESTATUS'', @UI) CL3 ON CL3.cdCode = F.fileStatus
	LEFT JOIN dbo.GetCodeLookupDescription (''TASKTYPE'', @UI) CL4 ON CL4.cdCode = T.tskType 
WHERE T.tskComplete = 0 
	AND T.tskActive = 1 
	'

--- TASK TYPE CLAUSE
IF(@TASKTYPE IS NOT NULL)
	SET @SELECT = @SELECT + 'AND T.tskType = @TASKTYPE
	'

--- TASK ALLOCATED TO CLAUSE
IF(@FEEALLOCATEDTO IS NOT NULL)
	SET @SELECT = @SELECT + 'AND T.feeusrid = @FEEALLOCATEDTO
	'

--- FILE STATUS CLAUSE
IF(@FILESTATUS IS NOT NULL)
	SET @SELECT = @SELECT + 'AND F.fileStatus = @FILESTATUS
	'

--- FILE FEE EARNER CLAUSE
IF(@FEEUSRID IS NOT NULL)
	SET @SELECT = @SELECT + 'AND F.filePrincipleID = @FEEUSRID
	'

--- DEPARTMENT CLAUSE
IF(@DEPT IS NOT NULL)
	SET @SELECT = @SELECT + 'AND F.fileDepartment = @DEPT
	'
--- FILE TYPE CLAUSE
IF(@FILETYPE IS NOT NULL)
	SET @SELECT = @SELECT + 'AND F.fileType = @FILETYPE
	'
--- DATE RANGE CLAUSE
IF @DATERANGE = 'WITHIN7' 
	SET @SELECT = @SELECT + N'  AND (T.tskDue > GETUTCDATE() AND T.tskDue < GETUTCDATE() + 7 )
	'
ELSE 
IF @DATERANGE = 'OVER7'
	SET @SELECT = @SELECT + N'  AND (T.tskDue > GETUTCDATE() + 7)
	'
ELSE
IF @DATERANGE = 'OD'
	SET @SELECT = @SELECT + N'  AND (T.tskDue < GETUTCDATE())
	'
ELSE
	SET @SELECT = @SELECT + N'  AND (T.tskDue is not NULL)
	'
SET @SELECT = @SELECT + N'
)'

IF @MAX_RECORDS > 0
	SET @SELECT =  @SELECT + N'
SELECT TOP (@MAX_RECORDS) *
FROM Res
'
ELSE
	SET @SELECT =  @SELECT + N'
SELECT *
FROM Res
'
--- SET THE ORDERBY CLAUSE
IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY tskDue'
ELSE 
	IF @ORDERBY NOT LIKE '%tskDue%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', tskDue'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

-- debug
--print @sql

EXEC sp_executesql @SELECT, N'@UI uUICultureInfo, @MAX_RECORDS INT, @FEEUSRID BIGINT, @FILESTATUS uCodeLookup, @DEPT uCodeLookup, @FILETYPE uCodeLookup, @DATERANGE uCodeLookup, @FEEALLOCATEDTO BIGINT, @TASKTYPE uCodeLookup'
	, @UI, @MAX_RECORDS, @FEEUSRID, @FILESTATUS, @DEPT, @FILETYPE, @DATERANGE, @FEEALLOCATEDTO, @TASKTYPE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchTasks] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchTasks] TO [OMSAdminRole]
    AS [dbo];

