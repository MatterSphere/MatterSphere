

CREATE PROCEDURE [dbo].[srepCliComplaint]
(
	@UI uUICultureInfo='{default}',
	@CLNO nvarchar(30) = null
)

AS 

DECLARE @SQL nvarchar(2500)
DECLARE @SELECT nvarchar(1500)
DECLARE @WHERE nvarchar(1000)

--- Select Statement for the base Query
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT     
	CL.clNo, 
	CL.clName, 
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
	X.cdDesc AS compType
FROM    
	dbo.dbComplaints C
INNER JOIN
    dbo.dbClient CL ON C.clID = CL.clID 
LEFT OUTER JOIN
    dbo.dbUser U ON C.CreatedBy = U.usrID
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''COMPLAINT'', @UI) AS X ON X.cdCode = C.compType '

--- SET THE WHERE CLAUSE
SET @WHERE = ' WHERE C.compActive = 1 '

--- CLNO CLAUSE
IF(@CLNO IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND cl.clNO = @CLNO '
END

--- CONCATENATE THE SELECT & WHERE CLAUSES INTO THE SQL VARIABLE
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE)

-- Debug Print
-- PRINT @SQL

exec sp_executesql @sql,
 N'
	@UI nvarchar(10),
	@CLNO nvarchar(30)',
	@UI,
	@CLNO

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliComplaint] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliComplaint] TO [OMSAdminRole]
    AS [dbo];

