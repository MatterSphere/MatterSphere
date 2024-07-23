CREATE PROCEDURE [dbo].[srepFilCompTaskList]
(
	@UI uUICultureInfo='{default}',
	@TASKTYPE uCodeLookup = null,
	@ALLOCATEDTO BigInt = null,
	@FILESTATUS uCodeLookup = null,
	@FEEEARNER BigInt = null,
	@DEPT uCodeLookup = null,
	@FILETYPE uCodeLookup = null,
	@DATERANGE uCodeLookup = null
)

AS 

declare @SQL nvarchar(MAX)

--- Select Statement for the base Query
set @SQL = N'
SELECT 	
	T.*, 
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') AS fileTypeDesc, 
	dbo.GetFileRef(C.clNo, F.fileNo) AS Ref, 
	TU.usrFullName as AllocatedTo, 
	FU.usrFullName as fileHandler, 
	COALESCE(CL2.cdDesc, ''~'' + NULLIF(F.fileDepartment, '''') + ''~'') AS fileDeptDesc, 
	COALESCE(CL3.cdDesc, ''~'' + NULLIF(F.fileStatus, '''') + ''~'') AS fileStatusDesc, 
	COALESCE(CL4.cdDesc, ''~'' + NULLIF(T.tskType, '''') + ''~'') AS tskTypeDesc
FROM 
	dbo.dbClient C
INNER JOIN
	dbo.dbFile F ON C.clID = F.clID 
INNER JOIN 
	dbo.dbTasks T ON F.fileID = T.fileID 
INNER JOIN
	dbo.dbUser as TU ON T.feeusrID = TU.usrID 
INNER JOIN
	dbo.dbUser as FU ON F.fileprincipleID = FU.usrID
LEFT JOIN dbo.GetCodeLookupDescription ( ''FILETYPE'', @UI ) CL1 ON CL1.[cdCode] = F.fileType
LEFT JOIN dbo.GetCodeLookupDescription ( ''DEPT'', @UI ) CL2 ON CL2.[cdCode] = F.fileDepartment
LEFT JOIN dbo.GetCodeLookupDescription ( ''FILESTATUS'', @UI ) CL3 ON CL3.[cdCode] = F.fileStatus
LEFT JOIN dbo.GetCodeLookupDescription ( ''TASKTYPE'', @UI ) CL4 ON CL4.[cdCode] = T.tskType
WHERE 
	T.tskComplete = 0 AND
	T.tskType = COALESCE(@TASKTYPE, T.tskType) AND 
	T.feeusrid = COALESCE(@ALLOCATEDTO, T.feeusrid) AND 
	F.filestatus = COALESCE(@FILESTATUS, F.filestatus) AND 
	F.fileprincipleID = COALESCE(@FEEEARNER, F.fileprincipleID) AND 
	F.filedepartment = COALESCE(@DEPT, F.filedepartment) AND 
	F.filetype = COALESCE(@FILETYPE, F.filetype)
ORDER BY
	T.tskDue'

--- Debug Print
print @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @TASKTYPE uCodeLookup, @ALLOCATEDTO BigInt, @FILESTATUS uCodeLookup, @FEEEARNER BigInt, @DEPT uCodeLookup, @FILETYPE uCodeLookup, @DATERANGE uCodeLookup', @UI, @TASKTYPE, @ALLOCATEDTO, @FILESTATUS, @FEEEARNER, @DEPT, @FILETYPE, @DATERANGE

