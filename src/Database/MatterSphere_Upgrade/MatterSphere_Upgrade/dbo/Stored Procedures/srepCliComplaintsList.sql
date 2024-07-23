CREATE  PROCEDURE [dbo].[srepCliComplaintsList]
(
	@UI uUICultureInfo='{default}',
	@CLNO nvarchar(50) = null
)

AS 

DECLARE @SQL nvarchar(4000)

--- Select Statement for the base Query
SET @SQL = N'
SELECT     
	CL.clNo, 
	CL.clName, 
	C.compType, 
	C.compDesc, 
	C.compRef, 
        C.compEstCompDate, 
	C.CreatedBy, 
	C.Created, 
	C.compCompleted, 
	CASE
	WHEN C.compNote IS NOT NULL THEN ''Notes: '' + Convert(nvarchar(4000), C.compNote)
	END AS Notes,  
	U.usrInits, 
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(C.compType, '''') + ''~'') AS compType
FROM    
	dbo.dbComplaints C
INNER JOIN
        dbo.dbClient CL ON C.clID = CL.clID 
INNER JOIN
        dbo.dbUser U ON C.CreatedBy = U.usrID
LEFT JOIN dbo.GetCodeLookupDescription ( ''COMPLAINT'', @UI ) CL1 ON CL1.[cdCode] =  C.compType
WHERE
	C.compActive = 1 AND
	CL.clNo = COALESCE(@CLNO, CL.clNo)'

--- Debug Print
PRINT @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @CLNO nvarchar(50)', @UI, @CLNO
