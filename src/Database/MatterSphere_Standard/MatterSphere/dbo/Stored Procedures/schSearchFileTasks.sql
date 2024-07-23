CREATE PROCEDURE dbo.schSearchFileTasks
(
	@UI uUICultureInfo = '{default}'
	, @MAX_RECORDS INT = 0
	, @FILEID BIGINT = NULL
	, @PageNo INT = NULL
	, @FILTER VARCHAR(2) = NULL
	, @ACTIVE BIT = NULL
	, @ORDERBY NVARCHAR(MAX) = NULL
)
AS
SET NOCOUNT ON;

DECLARE @SELECT NVARCHAR(MAX)

SET @SELECT = N'
DECLARE @OFFSET INT = 0
	, @TOP INT
	, @Total INT
	, @DT DATETIME = GETDATE()
	
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
	, usrFullName NVARCHAR(50)
);

WITH Res AS
(
SELECT
	T.*
	, COALESCE(CL.cdDesc, ''~'' + NULLIF(T.tsktype, '''') + ''~'') AS tsktypedesc  
	, U.usrFullName 
FROM dbo.dbTasks T
	INNER JOIN dbo.dbUser U ON U.usrid = T.feeusrid 
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''TASKTYPE'', @UI ) CL ON CL.[cdCode] = T.tsktype
WHERE T.fileID = @FILEID
	'
-- Completed tasks
IF  @FILTER = 'CO'
    SET @SELECT = @SELECT + N'AND T.tskCompleted > 1
	'
-- Future tasks
IF @FILTER = 'DU'
    SET @SELECT = @SELECT + N'AND T.tskCompleted IS NULL 
	AND T.tskdue >= dbo.GetEndDate(@DT)
	'
-- Overdue tasks
IF @FILTER = 'UD'
    SET @SELECT = @SELECT + N'AND T.tskCompleted IS NULL 
	AND T.tskdue < dbo.GetEndDate(@DT)
	'
IF @ACTIVE IS NOT NULL
    SET @SELECT = @SELECT + N'AND T.tskactive = @ACTIVE 
	'

SET @SELECT = @SELECT + N'
)
INSERT INTO @Res (tskid, usrFullName, Id)
SELECT tskid	
	, usrFullName
	'

IF @ORDERBY IS NULL
	SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY tskcompleted DESC, tskdue)'
ELSE 
	IF @ORDERBY NOT LIKE '%tskcompleted%' AND  @ORDERBY NOT LIKE '%tskdue%'
		SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY ' + @ORDERBY  + N', tskcompleted DESC, tskdue)'
	ELSE 
		SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY ' + @ORDERBY  + N')'


SET @SELECT = @SELECT + N'
FROM Res
SET @Total = @@ROWCOUNT


SELECT TOP(@TOP)
	T.*
	, COALESCE(CL.cdDesc, ''~'' + NULLIF(T.tsktype, '''') + ''~'') AS tsktypedesc  
	, res.usrFullName 
	, @Total as total
FROM @RES res
	INNER JOIN dbo.dbTasks T ON T.tskid = res.tskid
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''TASKTYPE'', @UI ) CL ON CL.[cdCode] = T.tsktype
WHERE res.Id > @OFFSET 
ORDER BY res.Id
'
print @SELECT 

EXEC sp_executesql @SELECT,  N'@UI uUICultureInfo, @MAX_RECORDS INT, @FILEID BIGINT, @PageNo INT, @FILTER VARCHAR(2), @ACTIVE BIT',
	@UI, @MAX_RECORDS, @FILEID, @PageNo, @FILTER, @ACTIVE

SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchFileTasks] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchFileTasks] TO [OMSAdminRole]
    AS [dbo];