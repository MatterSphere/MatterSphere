CREATE PROCEDURE [dbo].[srepDailyTasks]
(
	@UI uUICultureInfo = '{default}'
	, @DEPT nvarchar(15) = NULL
 	, @FEEALLOCATEDTO bigint = NULL
	, @FILESTATUS nvarchar(15) = NULL
	, @FILETYPE nvarchar(15) = NULL
	, @TASKTYPE nvarchar(15) = NULL
	, @FEEUSRID bigint = NULL
	, @DATERANGE nvarchar(30) = NULL
)

AS 

DECLARE @SELECT nvarchar(1900)
DECLARE @WHERE nvarchar(2000)
DECLARE @ORDERBY nvarchar(100)

--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

SELECT
	CL.clNo
	, F.fileNo
	, CL.clName
	, F.fileDesc
	, T.tskDesc
	, T.tskDue
	, T.Created
	, U.usrFullName
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(T.tskType, '''') + ''~'') AS TaskTypeDesc
FROM    
	dbo.dbTasks T
INNER JOIN
	dbFile F ON F.fileID = T.fileID
INNER JOIN
	dbClient CL ON CL.clID = F.clID
INNER JOIN
	dbo.dbUser U ON T.feeusrID = U.usrID
LEFT JOIN dbo.GetCodeLookupDescription ( ''TASKTYPE'', @UI ) CL1 ON CL1.[cdCode] = T.tskType'

---- SET THE WHERE CLAUSE
SET @WHERE = ' 
WHERE
	T.tskComplete = 0 AND
	T.tskActive = 1 '

--- DEPARTMENT CLAUSE
IF(@DEPT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.fileDepartment = @DEPT '
END

--- TASK ALLOCATED TO CLAUSE
IF(@FEEALLOCATEDTO IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND T.feeusrid = @FEEALLOCATEDTO '
END

--- FILE STATUS CLAUSE
IF(@FILESTATUS IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.fileStatus = @FILESTATUS '
END

--- FILE TYPE CLAUSE
IF(@FILETYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.fileType = @FILETYPE '
END

--- TASK TYPE CLAUSE
IF(@TASKTYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND T.tskType = @TASKTYPE '
END

--- FILE FEE EARNER CLAUSE
IF(@FEEUSRID IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.filePrincipleID = @FEEUSRID '
END

--- DATE RANGE CLAUSE
IF @DATERANGE = 'WITHIN7' 
	BEGIN 
		SET @WHERE = @WHERE + N'  AND (T.tskDue > getutcdate() AND T.tskDue < getutcdate() + 7 ) '
	END
ELSE 
IF @DATERANGE = 'OVER7'
	BEGIN
		SET @WHERE = @WHERE + N'  AND (T.tskDue > getutcdate() + 7) '
	END	                      
ELSE
IF @DATERANGE = 'OD'
	BEGIN
		SET @WHERE = @WHERE + N'  AND (T.tskDue < getutcdate()) '
	END	                      
ELSE
	BEGIN
		SET @WHERE = @WHERE + N'  AND (T.tskDue is not null) '
	END

--- BUILD THE ORDER BY CLAUSE
SET @ORDERBY = N' 
ORDER BY
	T.tskDue, CL.clName '

DECLARE @SQL nvarchar(4000)
--- ADD CLAUSES TOGETHER
SET @SQL = @SELECT + @WHERE + @ORDERBY

--- DEBUG PRINT
--PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI uUICultureInfo
	, @DEPT nvarchar(15)
 	, @FEEALLOCATEDTO bigint
	, @FILESTATUS nvarchar(15)
	, @FILETYPE nvarchar(15)
	, @TASKTYPE nvarchar(15)
	, @FEEUSRID bigint' 
	, @UI
	, @DEPT
	, @FEEALLOCATEDTO
	, @FILESTATUS
	, @FILETYPE
	, @TASKTYPE
	, @FEEUSRID

