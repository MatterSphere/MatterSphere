CREATE PROCEDURE [dbo].[srepFilEventChronology] 
(
	@UI uUICultureInfo = '{default}', 
	@FILEID bigint = null
) 

AS

SELECT
 	'Completed Tasks' AS Item,
 	7 AS Image,
 	COALESCE(CL1.cdDesc, '~' + NULLIF(T.tskType, '') + '~') AS Info1,
 	T.tskDesc AS Info2,
 	'Created By : ' + U.usrInits AS Info3,
 	T.Created AS Info4,
 	T.tskCompleted AS Info5
FROM
 	dbTasks T
INNER JOIN
 	dbUser U ON U.usrID = T.CreatedBy
LEFT JOIN dbo.GetCodeLookupDescription ( 'TASKTYPE', @UI ) CL1 ON CL1.[cdCode] =  T.tskType
WHERE
 	T.tskComplete = 1 AND
 	T.fileID = @FILEID
UNION SELECT
 	'Milestones' AS Item,
 	4 AS Image,
 	MSC.msDescription AS Info1,
 	null AS Info2,
 	'Next Due Stage : ' + Convert(nvarchar(10), MSD.MSNextDueStage) AS Info3,
 	null AS Info4,
 	MSD.MSNextDueDate AS Info5
FROM
 	dbMSData_OMS2K MSD
INNER JOIN
 	dbMSConfig_OMS2K MSC ON MSC.msCode = MSD.msCode
WHERE
 	MSD.fileID = @FILEID
UNION SELECT
 	'Documents' AS Item,
 	6 AS Image,
 	'Case No : ' + F.fileNo AS Info1,
 	DOC.docDesc AS Info2,
 	'Created By : ' + U.usrInits AS Info3,
 	DOC.Created AS Info4,
 	DOC.Updated AS Info5
FROM
 	dbDocument DOC
INNER JOIN
 	dbFile F ON F.fileID = DOC.fileID
INNER JOIN
 	dbUser U ON U.usrID = DOC.CreatedBy
WHERE
 	F.fileID = @FILEID
