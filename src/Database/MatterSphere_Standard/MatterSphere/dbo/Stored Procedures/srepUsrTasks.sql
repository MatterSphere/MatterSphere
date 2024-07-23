

CREATE PROCEDURE [dbo].[srepUsrTasks]
(
	@UI uUICultureInfo='{default}',
	@FEEEARNER BigInt = null,
	@STARTDATE DateTime = null,
	@ENDDATE DateTime = null
)

AS 

declare @SQL nvarchar(4000)

--- Select Statement for the base Query
set @SQL = N'
SELECT     
	T.tskDesc,
	T.tskDue,
	T.Created,
	T.Updated,	
	U.usrInits AS FeeInits,
	U.usrFullName AS FeeFullName,
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(T.tskType, '''') + ''~'') AS TaskTypeDesc,
	CASE
	WHEN T.tskNotes is not null THEN ''Notes: '' + Convert(nvarchar(4000), T.tskNotes)
	END AS Notes,
	dbo.GetFileRef(CL.clNo, F.fileNo) AS Ref,
	CL.clName,
	F.fileDesc,
	T.tskNotes
FROM
	dbo.dbTasks T
INNER JOIN
	dbo.dbFile F ON F.fileID = T.fileID
INNER JOIN
	dbo.dbClient CL ON CL.clID = F.clID
INNER JOIN
	dbo.dbUser U ON T.feeusrID = U.usrID
LEFT JOIN 
	dbo.GetCodeLookupDescription ( ''TASKTYPE'' , @UI ) CL1 ON CL1.cdCode = T.tskType
WHERE
	U.usrID = COALESCE(@FEEEARNER, U.usrID) AND
	(T.tskDue >= COALESCE(@STARTDATE, T.tskDue) AND
	T.tskDue <= COALESCE(@ENDDATE, T.tskDue))'

--- Debug Print
print @sql

exec sp_executesql @sql, N'@UI nvarchar(10), @FEEEARNER BigInt, @STARTDATE DateTime, @ENDDATE DateTime', @UI, @FEEEARNER, @STARTDATE, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepUsrTasks] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepUsrTasks] TO [OMSAdminRole]
    AS [dbo];

