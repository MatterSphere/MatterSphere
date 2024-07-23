CREATE PROCEDURE [dbo].[srepFilTaskManagement]
(
	@UI uUICultureInfo = '{default}'
	, @FILEID bigint
)
AS

DECLARE 
	  @x tinyint 
	, @v nvarchar(100)
	, @z nvarchar(30)
	, @y nvarchar(30)

CREATE TABLE #sortTable
( 
	ID tinyint identity 
	, MSStageDesc nvarchar(500)
	, MSCode nvarchar(30)
	, MSDescription nvarchar(50)
	, MSStageDue datetime
	, MSStageAchieved datetime
)

SET @x = 0
WHILE @x < 31
	BEGIN
		SET @x= @x + 1
		SET @v = 'MSStage' + convert(nvarchar(10), @x) + 'Desc'
		SET @z = 'MSStage' + convert(nvarchar(10), @x) + 'Due'
		SET @y = 'MSStage' + convert(nvarchar(10), @x) + 'Achieved'
		EXEC 
		('
			INSERT #sortTable 
			(
				MSStageDesc
				, MSCode
				, MSDescription
				, MSStageDue
				, MSStageAchieved 
			) 
			SELECT 
				' + @v + '
				, MSC.MSCode
				, MSC.MSDescription
				, ' + @z + '
				, ' + @y + ' 
			FROM 
				dbMSConfig_OMS2k MSC
			JOIN 
				dbMSData_OMS2K MSD ON MSD.MSCode = MSC.MSCode 
			WHERE 
				MSD.fileID = ''' + @FILEID + '''' 
		)

IF @x = 30
	BREAK
ELSE 
	CONTINUE
END

SELECT
	ST.*
	, T.fileID
	, T.tskDesc
	, T.tskComplete
	, T.tskCompleted
	, T.tskDue
	, U.usrFullName AS AssignedTo
	, CASE WHEN T.tskDue < GETDATE() AND T.tskCompleted IS NULL THEN DATEDIFF(d, T.tskDue, GETDATE()) ELSE NULL END AS TaskOverdue
	, COALESCE(CL1.cdDesc, '~' + NULLIF(TM.tmCode, '') + '~') AS TeamDesc
FROM
	#sortTable ST
LEFT OUTER JOIN
	dbTasks T ON T.tskMSStage = ST.ID AND
	T.tskMSPlan = ST.MSCode  COLLATE Latin1_General_CI_AS AND
	T.fileID = @FILEID
LEFT OUTER JOIN
	dbTeam TM ON TM.tmID = T.tmID
LEFT OUTER JOIN
	dbUser U ON U.usrID = T.usrID
LEFT JOIN dbo.GetCodeLookupDescription ( 'TEAM', @UI ) CL1 ON CL1.[cdCode] =  TM.tmCode
WHERE
	ST.MSStageDesc IS NOT NULL
ORDER BY 
	ST.[ID] , T.tskFilter
