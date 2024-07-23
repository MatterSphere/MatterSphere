

CREATE PROCEDURE [dbo].[srepFilTasksAll]
(
	@UI uUICultureInfo='{default}',
	@CLNO nvarchar(50) = null,
	@FILENO nvarchar(50) = null
)

AS 

DECLARE @SQL nvarchar(max)

--- Select Statement for the base Query
SET @SQL = N'
SELECT     
	T.tskType, 
	T.tskDesc, 
	T.tskDue, 
	CASE
	WHEN T.tskNotes > '''' THEN ''Notes: '' + Convert(nvarchar(4000), T.tskNotes)
	END AS Notes,
	T.tskCompleted, 
	T.tskComplete, 
	T.Created, 
	T.Updated, 
	U.usrFullName, 
    COALESCE(CL1.cdDesc, ''~'' + NULLIF(T.tskType, '''') + ''~'') as TaskType,  
	CL.clNo, 
	F.fileNo, 
	CL.clName, 
	F.fileDesc
FROM         
	dbo.dbTasks T
INNER JOIN
	dbo.dbUser U ON T.feeusrID = U.usrID 
INNER JOIN
	dbo.dbFile F ON T.fileID = F.fileID 
INNER JOIN
	dbo.dbClient CL ON F.clID = CL.clID
LEFT JOIN
	dbo.GetCodeLookupDescription ( ''TASKTYPE'' , @UI ) CL1 ON CL1.cdCode = T.tskType
WHERE
	T.tskActive = 1 AND
	CL.clNo = COALESCE(@CLNO, CL.clNo) AND
	F.fileNo = COALESCE(@FILENO, F.fileNo)'

--- Debug Print
PRINT @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @CLNO nvarchar(50), @FILENO nvarchar(50)', @UI, @CLNO, @FILENO

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilTasksAll] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilTasksAll] TO [OMSAdminRole]
    AS [dbo];

