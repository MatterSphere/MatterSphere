CREATE PROCEDURE dbo.schSearchTeamTasks
(
	@UI uUICultureInfo = '{default}'
	, @MAX_RECORDS INT = 0
	, @FILETYPE NVARCHAR(15) = NULL
	, @DEPT NVARCHAR(15) = NULL
	, @FILE BIGINT = NULL
	--@DATERANGE uCodeLookup = NULL, 
	, @TASKTYPE uCodeLookup = NULL
	, @TEAMID INT = NULL
	, @FEEEARNER INT = NULL
	, @TEAMLEADER INT = NULL
	, @ASSIGNEDTO INT = NULL
	, @COMPLETED BIT = 0
	-- Parameters added 09.01.09
	, @FILESTATUS NVARCHAR(15) = NULL
	, @DUEDATESTART DATETIME = NULL
	, @DUEDATEEND DATETIME = NULL
	, @USER INT = NULL
	-- Parameters added 21.05.20
	, @PageNo INT = NULL
	, @ORDERBY NVARCHAR(MAX) = NULL
)
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @EDITION NVARCHAR(1000)
	, @SELECT NVARCHAR(MAX)

SET @EDITION = (SELECT REGEDITION FROM DBREGINFO)

SET @EDITION = dbo.GetCodeLookupDesc( @EDITION, '%FILE%' , @UI)

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
	SET @OFFSET = @TOP * (@PageNo- 1)

DECLARE @Res TABLE(
	Id BIGINT PRIMARY KEY
	, tskid BIGINT
	, clid BIGINT
	, clNo NVARCHAR(12)
	, clName NVARCHAR(128)
	, tskDueOrder DATETIME
);

WITH Res AS
(
SELECT
	T.*
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') AS fileTypeDesc
	, C.clID
	, C.clNo + ''/'' + F.fileNo AS clfileno
	, C.clName
	, F.filedesc
	, F.fileStatus
	, TU.usrFullName AS UsrTaskAllocatedTo
	, FU.usrFullName AS fileHandler
	, COALESCE(CL2.cdDesc, ''~'' + NULLIF(F.fileDepartment, '''') + ''~'') AS fileDeptDesc
	, COALESCE(CL3.cdDesc, ''~'' + NULLIF(F.fileStatus, '''') + ''~'') AS fileStatDesc
	, COALESCE(CL4.cdDesc, ''~'' + NULLIF(T.tskType, '''') + ''~'') AS tsktypedesc
	, C.clNo
	, F.fileNo
	, TM.tmCode
	, COALESCE(CL5.cdDesc, ''~'' + NULLIF(TM.tmCode, '''') + ''~'') AS tskTeamName
	, U.usrFullName AS AssignedTo
	, TC.usrFullName AS CreatedByUser
	, CU.usrFullName AS CompletedByUser
	, CASE WHEN T.tskCompleted = N''1900/01/01'' THEN NULL ELSE T.tskCompleted END AS tskCompletedDate
	, COALESCE(T.tskDue, N''9999/12/31'') AS tskDueOrder
	, CASE
		WHEN T.tskcomplete = 0 AND dbo.CalculateDays(GETUTCDATE(), T.tskdue) < 0 THEN 66 
		WHEN T.tskcomplete = 0 AND dbo.CalculateDays(GETUTCDATE(), T.tskdue) < 7 THEN 68
		WHEN T.tskcomplete = 1 AND dbo.CalculateDays(GETUTCDATE(), T.tskdue) < 0 THEN 66 
		ELSE NULL 
	END AS IconIndex
	, CASE
		WHEN T.tskcomplete = 0 AND tskdue > N''1900/01/01'' THEN dbo.CalculateDays(GETUTCDATE(), tskdue)
		WHEN T.tskcomplete = 1 AND tskcompleted > N''1900/01/01''THEN dbo.CalculateDays(GETUTCDATE(), tskdue)
		ELSE NULL 
	END AS DueDays
	, CASE 
		WHEN t.feeusrid = @FEEEARNER THEN ''Task''
		WHEN f.fileprincipleid = @FEEEARNER THEN @EDITION
		ELSE @EDITION + '' ''
	END AS HowResponsible
	FROM dbo.dbTasks T
		INNER JOIN dbo.dbFile F ON F.fileID = T.fileID
		INNER JOIN dbo.dbClient C ON C.clID = F.clID
		INNER JOIN dbo.dbUser TU ON TU.usrID = T.feeusrID
		INNER JOIN dbo.dbUser FU ON FU.usrID = F.filePrincipleID
		INNER JOIN dbo.dbUser TC ON TC.usrID = T.CreatedBy
		LEFT OUTER JOIN dbo.dbUser CU ON CU.usrID = T.tskCompletedBy
		LEFT OUTER JOIN dbo.dbTeam TM ON TM.tmID = T.tmID
		LEFT OUTER JOIN dbo.dbUser U ON U.usrID = T.usrID
		LEFT OUTER JOIN dbo.GetCodeLookupDescription(''FILETYPE'', @UI) CL1 ON CL1.cdCode = F.fileType
		LEFT OUTER JOIN dbo.GetCodeLookupDescription(''DEPT'', @UI) CL2 ON CL2.cdCode = F.fileDepartment
		LEFT OUTER JOIN dbo.GetCodeLookupDescription(''FILESTATUS'', @UI) CL3 ON CL3.cdCode = F.fileStatus
		LEFT OUTER JOIN dbo.GetCodeLookupDescription(''TASKTYPE'', @UI) CL4 ON CL4.cdCode = T.tskType
		LEFT OUTER JOIN dbo.GetCodeLookupDescription (''TEAM'', @UI) CL5 ON CL5.cdCode = TM.tmCode
WHERE T.tskActive = 1
	'
IF @TASKTYPE IS NOT NULL
    SET @SELECT = @SELECT + N'AND T.tskType = @TASKTYPE
	'

IF @COMPLETED IS NOT NULL
    SET @SELECT = @SELECT + N'AND T.tskComplete = @COMPLETED
	'

IF @TEAMID IS NOT NULL
    SET @SELECT = @SELECT + N'AND T.tmid = @TEAMID
	'

IF  @DEPT IS NOT NULL
    SET @SELECT = @SELECT + N'AND F.fileDepartment = @DEPT
	'

IF @FILETYPE IS NOT NULL
    SET @SELECT = @SELECT + N'AND F.filetype = @FILETYPE
	'

IF @FILE IS NOT NULL
    SET @SELECT = @SELECT + N'AND T.fileid = @FILE
	'

IF @FEEEARNER IS NOT NULL
    SET @SELECT = @SELECT + N'AND ((T.feeusrid = @FEEEARNER AND T.USRID IS NOT NULL) OR ((T.USRID IS NULL OR T.USRID <> @FEEEARNER) AND T.FEEUSRID <> @FEEEARNER AND (F.FILEPRINCIPLEID = @FEEEARNER OR F.FILERESPONSIBLEID = @FEEEARNER)))
	'

IF @ASSIGNEDTO IS NOT NULL
    SET @SELECT = @SELECT + N'AND (T.usrid = @ASSIGNEDTO OR (T.USRID IS NULL AND T.FEEUSRID = @ASSIGNEDTO))
	'

IF @TEAMLEADER IS NOT NULL
    SET @SELECT = @SELECT + N'AND TM.tmleader = @TEAMLEADER
	'

----------------------------------------------------------------------------------
-- Code added 09.01.2008
-- Filter by Task Due Date Range
-- Within supplied date range (or automated within seven days)
IF (@DUEDATESTART IS NOT NULL AND @DUEDATEEND IS NOT NULL)
BEGIN
	SET @SELECT = @SELECT + 'AND T.tskDue >= @DUEDATESTART AND T.tskDue < @DUEDATEEND
	' 
	GOTO NextStep
END

-- Overdue
IF (@DUEDATESTART IS NULL AND @DUEDATEEND IS NOT NULL)
BEGIN
	SET @SELECT = @SELECT + 'AND T.tskDue < @DUEDATEEND
	' 
	GOTO NextStep
END

-- Over Seven Days
IF (@DUEDATESTART IS NOT NULL AND @DUEDATEEND IS NULL)
	SET @SELECT = @SELECT + 'AND T.tskDue >= @DUEDATESTART
	' 

NextStep:

-- Filter by file status
IF @FILESTATUS IS NOT NULL
	SET @SELECT = @SELECT + N'AND F.fileStatus = @FILESTATUS
	  '

-- Filter by user
IF @USER IS NOT NULL
	SET @SELECT = @SELECT + N' AND T.usrID = @USER
	'


SET @SELECT = @SELECT + N'
)

INSERT INTO @Res (tskid, clID, clNo, clName, tskDueOrder, Id)
SELECT 
	tskid
	, clID
	, clNo
	, clName
	, tskDueOrder
	'

IF @COMPLETED = 0
	IF @ORDERBY IS NULL
		SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY tskDueOrder)'
	ELSE 
		IF @ORDERBY NOT LIKE '%tskDueOrder%'
			SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY ' + @ORDERBY  + N', tskDueOrder)'
		ELSE 
			SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY ' + @ORDERBY  + N')'
ELSE
	IF @ORDERBY IS NULL
		SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY tskcompleted DESC)'
	ELSE 
		IF @ORDERBY NOT LIKE '%tskcompleted%'
			SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY ' + @ORDERBY  + N', tskcompleted DESC)'
		ELSE 
			SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY ' + @ORDERBY  + N')'

SET @SELECT = @SELECT + N'
FROM Res
OPTION (RECOMPILE)

SET @Total = @@ROWCOUNT

SELECT TOP(@TOP)
	T.*
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') AS fileTypeDesc
	, res.clid
	, res.clNo + ''/'' + F.fileNo AS clfileno
	, res.clname
	, F.filedesc
	, F.fileStatus
	, TU.usrFullName AS UsrTaskAllocatedTo
	, FU.usrFullName AS fileHandler
	, COALESCE(CL2.cdDesc, ''~'' + NULLIF(F.fileDepartment, '''') + ''~'') AS fileDeptDesc
	, COALESCE(CL3.cdDesc, ''~'' + NULLIF(F.fileStatus, '''') + ''~'') AS fileStatDesc
	, COALESCE(CL4.cdDesc, ''~'' + NULLIF(T.tskType, '''') + ''~'') AS tsktypedesc
	, res.clNo
	, F.fileNo
	, TM.tmCode
	, COALESCE(CL5.cdDesc, ''~'' + NULLIF(TM.tmCode, '''') + ''~'') AS tskTeamName
	, U.usrFullName AS AssignedTo
	, TC.usrFullName AS CreatedByUser
	, CU.usrFullName AS CompletedByUser
	, CASE WHEN T.tskCompleted = N''1900/01/01'' THEN NULL ELSE T.tskCompleted END AS tskCompletedDate
	, res.tskDueOrder
	, CASE
		WHEN T.tskcomplete = 0 AND dbo.CalculateDays(GETUTCDATE(), T.tskdue) < 0 THEN 66 
		WHEN T.tskcomplete = 0 AND dbo.CalculateDays(GETUTCDATE(), T.tskdue) < 7 THEN 68
		WHEN T.tskcomplete = 1 AND dbo.CalculateDays(GETUTCDATE(), T.tskdue) < 0 THEN 66 
		ELSE NULL 
	END AS IconIndex
	, CASE
		WHEN T.tskcomplete = 0 AND tskdue > N''1900/01/01'' THEN dbo.CalculateDays(GETUTCDATE(), tskdue)
		WHEN T.tskcomplete = 1 AND tskcompleted > N''1900/01/01''THEN dbo.CalculateDays(GETUTCDATE(), tskdue)
		ELSE NULL 
	END AS DueDays
	, CASE 
		WHEN t.feeusrid = @FEEEARNER THEN ''Task''
		WHEN f.fileprincipleid = @FEEEARNER THEN @EDITION
		ELSE @EDITION + '' ''
	END AS HowResponsible
	, @Total AS Total
	FROM @RES res
		INNER JOIN dbo.dbTasks T ON res.tskID = T.tskID
		INNER JOIN config.dbFile F ON F.fileID = T.fileID
		INNER JOIN dbo.dbUser TU ON TU.usrID = T.feeusrID
		INNER JOIN dbo.dbUser FU ON FU.usrID = F.filePrincipleID
		INNER JOIN dbo.dbUser TC ON TC.usrID = T.CreatedBy
		LEFT OUTER JOIN dbo.dbUser CU ON CU.usrID = T.tskCompletedBy
		LEFT OUTER JOIN dbo.dbTeam TM ON TM.tmID = T.tmID
		LEFT OUTER JOIN dbo.dbUser U ON U.usrID = T.usrID
		LEFT OUTER JOIN dbo.GetCodeLookupDescription(''FILETYPE'', @UI) CL1 ON CL1.cdCode = F.fileType
		LEFT OUTER JOIN dbo.GetCodeLookupDescription(''DEPT'', @UI) CL2 ON CL2.cdCode = F.fileDepartment
		LEFT OUTER JOIN dbo.GetCodeLookupDescription(''FILESTATUS'', @UI) CL3 ON CL3.cdCode = F.fileStatus
		LEFT OUTER JOIN dbo.GetCodeLookupDescription(''TASKTYPE'', @UI) CL4 ON CL4.cdCode = T.tskType
		LEFT OUTER JOIN dbo.GetCodeLookupDescription (''TEAM'', @UI) CL5 ON CL5.cdCode = TM.tmCode
	WHERE res.Id > @OFFSET
	ORDER BY res.Id
'

PRINT @SELECT 

EXEC sp_executesql @SELECT,  N'
	@UI uUICultureInfo, 
	@MAX_RECORDS INT, 
	@DEPT NVARCHAR(15), 
	@FILETYPE NVARCHAR(15), 
	@FILE BIGINT, 
	@TEAMID INT, 
	@EDITION NVARCHAR ( 10 ) , 
	@TASKTYPE uCodeLookup, 
	@FEEEARNER INT, 
	@TEAMLEADER INT, 
	@ASSIGNEDTO INT, 
	@COMPLETED bit,
	@FILESTATUS NVARCHAR(15),
	@DUEDATESTART DATETIME,
	@DUEDATEEND DATETIME,
	@USER INT,
	@PageNo INT', 

	@UI, 
	@MAX_RECORDS, 
	@DEPT,	
	@FILETYPE, 
	@FILE, 
	@TEAMID, 
	@EDITION, 
	@TASKTYPE, 
	@FEEEARNER, 
	@TEAMLEADER, 
	@ASSIGNEDTO, 
	@COMPLETED,
	@FILESTATUS,
	
	-- Parameters added 09.01.2009
	@DUEDATESTART,
	@DUEDATEEND,
	@USER,
	-- Parameters added 21.05.20
	@PageNo

SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchTeamTasks] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchTeamTasks] TO [OMSAdminRole]
    AS [dbo];