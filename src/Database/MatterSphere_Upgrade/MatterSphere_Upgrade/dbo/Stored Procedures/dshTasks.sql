CREATE PROCEDURE [dbo].[dshTasks](@UI uUICultureInfo = '{default}', @MAX_RECORDS INT = 50, @PageNo INT = 1, @Query NVARCHAR(MAX) = null, @USER INT = NULL, @Filter VARCHAR(2), @OrderBY NVARCHAR(MAX) = NULL)
AS
SET NOCOUNT ON
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
DECLARE @OFFSET INT = 0
	, @TOP INT
	, @Total INT
	, @tskDue DATETIME = CONVERT(VARCHAR(10), GETUTCDATE() + 1, 121)
	, @SQL NVARCHAR(MAX)

IF ISNULL(@OrderBY, '') = ''
	SET @OrderBY = N'ORDER BY tskDue'
ELSE 
	SET @OrderBY = N'ORDER BY ' + @OrderBY

IF @MAX_RECORDS > 0
	SET @TOP = @MAX_RECORDS
ELSE
	SET @TOP = 50

IF @PageNo IS NULL
	SET @OFFSET = 0
ELSE
	SET @OFFSET = @TOP * (@PageNo- 1)

DECLARE @Res TABLE(
	Id INT IDENTITY(1, 1) PRIMARY KEY
	, tskID BIGINT
)

SET @SQL = N'
WITH tsk AS (
SELECT T.tskID
	, COALESCE(T.tskDue, N''9999/12/31'') AS tskDue
	, UC.usrFullName AS CreatedBy
	, C.clNo
	, F.fileNo
	, T.tskDesc
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(T.tskType, '''') + ''~'') AS tsktypedesc
	, COALESCE(CL2.cdDesc, ''~'' + NULLIF(TM.tmCode, '''') + ''~'') as tskTeamName
	, U.usrFullName AS AssignedTo
	, T.tskActive
	, T.tskComplete
	, T.feeusrID
	, T.usrID
	, F.filePrincipleID
	, F.fileResponsibleID
	, TM.tmleader
	, COALESCE(CL3.cdDesc, ''~'' + CASE WHEN T.tskCompleted IS NOT NULL THEN ''CMPLT'' ELSE CASE WHEN T.tskDue < GETUTCDATE() THEN ''PASTDUE'' ELSE CASE WHEN DATEDIFF(dd, tskDue, GETUTCDATE()) = 0 THEN ''DUESOON'' ELSE ''ONTIME'' END END END + ''~'') AS tskStatusDesc
	, CASE WHEN T.tskCompleted IS NOT NULL THEN ''CMPLT'' ELSE CASE WHEN T.tskDue < GETUTCDATE() THEN ''PASTDUE'' ELSE CASE WHEN DATEDIFF(dd, tskDue, GETUTCDATE()) = 0 THEN ''DUESOON'' ELSE ''ONTIME'' END END END AS tskStatusCode
FROM dbo.dbClient C
	INNER JOIN dbo.dbFile F ON C.clID = F.clID 
	INNER JOIN dbo.dbTasks T ON F.fileID = T.fileID 
	INNER JOIN dbo.dbUser as TU ON T.feeusrID = TU.usrID 
	INNER JOIN dbo.dbUser TF ON TF.USRID = T.FEEUSRID
	LEFT JOIN dbo.dbTeam TM ON TM.tmid = T.tmid
	LEFT JOIN dbo.GetCodeLookupDescription ( ''TASKTYPE'', @UI ) CL1 ON CL1.[cdCode] =  T.tskType
	LEFT JOIN dbo.GetCodeLookupDescription ( ''TEAM'', @UI ) CL2 ON CL2.[cdCode] =  TM.tmCode
	LEFT JOIN dbo.dbUser as U ON T.usrID = U.usrID 
	LEFT JOIN dbo.GetCodeLookupDescription ( ''DASHBOARD'', @UI ) CL3 ON CL3.[cdCode] = CASE WHEN T.tskCompleted IS NOT NULL THEN ''CMPLT'' ELSE CASE WHEN T.tskDue < GETUTCDATE() THEN ''PASTDUE'' ELSE CASE WHEN DATEDIFF(dd, tskDue, GETUTCDATE()) = 0 THEN ''DUESOON'' ELSE ''ONTIME'' END END END 
	INNER JOIN dbo.dbUser as UC ON T.CreatedBy = UC.usrID 
)
SELECT tskID
FROM tsk
WHERE tskActive = 1 
	AND tskComplete = 0
'

IF @Filter ='MT'
	SET @SQL = @SQL + N'
	AND (usrID = @USER OR ( usrID IS NULL AND feeusrID = @USER ))
	AND (@Query IS NULL OR tskDesc LIKE ''%'' + @Query + ''%'' OR fileNo LIKE ''%'' + @Query + ''%'' OR clNo LIKE ''%'' + @Query + ''%'')
'

IF @Filter ='RT'
	SET @SQL = @SQL + N'
	AND ((feeusrID = @USER AND usrID IS NOT NULL) OR ((usrID IS NULL OR usrID != @USER) AND feeusrID != @USER AND (filePrincipleID = @USER OR fileResponsibleID = @USER)))
	AND (@Query IS NULL OR tskDesc LIKE ''%'' + @Query + ''%'' OR fileNo LIKE ''%'' + @Query + ''%'' OR clNo LIKE ''%'' + @Query + ''%'')
'

IF @Filter ='TT'
	SET @SQL = @SQL + N'
	AND tmleader = @USER
	AND (@Query IS NULL OR tskDesc LIKE ''%'' + @Query + ''%'' OR fileNo LIKE ''%'' + @Query + ''%'' OR clNo LIKE ''%'' + @Query + ''%'')
'
IF @Filter ='AT'
	SET @SQL = @SQL + N'
	AND tskDue < @tskDue
	AND (@Query IS NULL OR tskDesc LIKE ''%'' + @Query + ''%'' OR fileNo LIKE ''%'' + @Query + ''%'' OR clNo LIKE ''%'' + @Query + ''%'')
'

SET @SQL = @SQL + @OrderBY

INSERT INTO @Res(tskID)
EXEC sp_executesql @SQL, N'@USER INT, @tskDue DATETIME, @Query NVARCHAR(MAX), @UI uUICultureInfo', @USER, @tskDue, @Query, @UI

SET @Total = @@ROWCOUNT

SELECT TOP(@TOP)
	T.tskID
	, T.tskDue --Due
	, T.tskDesc -- Description
	, COALESCE(CL1.cdDesc, '~' + NULLIF(T.tskType, '') + '~') AS tskTypeDesc --Type
	, C.clNo  -- For Natter No
	, F.fileNo -- For Natter No
	, COALESCE(CL2.cdDesc, '~' + NULLIF(TM.tmCode, '') + '~') as tskTeamName -- TeamName
	, UC.usrFullName AS CreatedBy -- CreatedBy
	, U.usrFullName AS AssignedTo -- AssignedTo
	, T.tskCompleted
	, C.clID
	, F.fileID
	, T.feeusrID
	, T.usrID
--	, T.docID
--	, TM.tmCode 
	, T.CreatedBy AS tskCreatedBy
	, COALESCE(CL3.cdDesc, '~' + CASE WHEN T.tskCompleted IS NOT NULL THEN 'CMPLT' ELSE CASE WHEN T.tskDue < GETUTCDATE() THEN 'PASTDUE' ELSE CASE WHEN DATEDIFF(dd, tskDue, GETUTCDATE()) = 0 THEN 'DUESOON' ELSE 'ONTIME' END END END + '~') AS tskStatusDesc
	, CASE WHEN T.tskCompleted IS NOT NULL THEN 'CMPLT' ELSE CASE WHEN T.tskDue < GETUTCDATE() THEN 'PASTDUE' ELSE CASE WHEN DATEDIFF(dd, tskDue, GETUTCDATE()) = 0 THEN 'DUESOON' ELSE 'ONTIME' END END END AS tskStatusCode
	, @Total AS Total
FROM dbo.dbClient C
	INNER JOIN dbo.dbFile F ON C.clID = F.clID 
	INNER JOIN dbo.dbTasks T ON F.fileID = T.fileID 
	INNER JOIN @Res R ON R.tskID = T.tskID
	LEFT JOIN dbo.dbUser as U ON T.usrID = U.usrID 
	INNER JOIN dbo.dbUser as UC ON T.CreatedBy = UC.usrID 
	LEFT JOIN dbTeam TM ON TM.tmid = T.tmid
	LEFT JOIN dbo.GetCodeLookupDescription ( 'TASKTYPE', @UI ) CL1 ON CL1.[cdCode] =  T.tskType
	LEFT JOIN dbo.GetCodeLookupDescription ( 'TEAM', @UI ) CL2 ON CL2.[cdCode] =  TM.tmCode
	LEFT JOIN dbo.GetCodeLookupDescription ( 'DASHBOARD', @UI ) CL3 ON CL3.[cdCode] = CASE WHEN T.tskCompleted IS NOT NULL THEN 'CMPLT' ELSE CASE WHEN T.tskDue < GETUTCDATE() THEN 'PASTDUE' ELSE CASE WHEN DATEDIFF(dd, tskDue, GETUTCDATE()) = 0 THEN 'DUESOON' ELSE 'ONTIME' END END END 
WHERE R.Id > @OFFSET
ORDER BY R.Id
GO

GRANT EXECUTE
    ON OBJECT::[dbo].[dshTasks] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dshTasks] TO [OMSAdminRole]
    AS [dbo];

