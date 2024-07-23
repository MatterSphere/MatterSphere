

CREATE PROCEDURE [dbo].[srepTimeRecords]
(
	@UI uUICultureInfo = '{default}'
	, @FILEID bigint = NULL
)

AS
 
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT     
	CL.clNo, 
	F.fileNo, 
	T.timeActivityCode, 
	T.timeDesc, 
	T.timeRecorded, 
	T.timeUnits, 
	T.timeCharge, 
	T.timeBilled, 
	T.timeBillNo, 
	U.usrInits, 
	U.usrFullName,
	X.cdDesc AS TimeActivityCodeDesc, 
	CL.clName, 
	F.fileDesc,
	CASE WHEN T.timeBilled = 1 THEN 1 ELSE NULL END AS Billed,
	CASE WHEN T.timeBilled = 0 THEN 1 ELSE NULL END AS Unbilled
FROM
	dbTimeLedger T
INNER JOIN
	dbFile F ON F.fileID = T.fileID
INNER JOIN
	dbClient CL ON CL.clID = F.clID
INNER JOIN
	dbUser U ON U.usrID = T.feeusrid
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription('TIMEACTCODE', @UI) AS X ON X.cdCode = T.timeActivityCode
WHERE     
	F.fileID = @FILEID
ORDER BY 
	T.timeRecorded

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepTimeRecords] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepTimeRecords] TO [OMSAdminRole]
    AS [dbo];

