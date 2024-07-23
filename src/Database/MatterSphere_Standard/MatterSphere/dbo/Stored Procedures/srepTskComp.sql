

CREATE PROCEDURE [dbo].[srepTskComp] 

(
	@UI uUICultureInfo = '{default}'
	, @FILEID bigint 
)

AS 
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT     
	T.tskType, 
	T.tskDesc, 
	T.tskDue, 
	CASE
		WHEN DATALENGTH(T.tskNotes) <> 0 THEN 'Notes: ' + Convert(nvarchar(4000), T.tskNotes)
	END AS Notes,
	T.tskCompleted, 
	T.tskComplete, 
	T.Created, 
	T.Updated, 
	U.usrFullName, 
	X.cdDesc AS TaskType,  
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
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription('TASKTYPE', @UI) AS X ON X.cdCode = T.tskType 
WHERE
	T.tskComplete = 1 AND
	T.tskActive = 1 AND
	F.fileID = @FILEID 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepTskComp] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepTskComp] TO [OMSAdminRole]
    AS [dbo];

