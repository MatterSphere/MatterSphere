

CREATE PROCEDURE [dbo].[srepCliDOB]
(
	@UI uUICultureInfo='{default}',
	@CLTYPE ucodeLookup = NULL,
	@FIRMCONT int = NULL,
	@CLNAME nvarchar(128) = NULL,
	@STARTDATE datetime = NULL,
	@ENDDATE datetime = NULL
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
	CI.contDOB, 
	A.addLine1, 
	C.contName, 
	FLOOR(DATEDIFF(dd, CI.contDOB, GETDATE())/365.25) AS Age, 
	DATEPART(d, CI.contDOB) AS Day, 
	CL.feeusrID, 
    U.usrInits AS FirmContInits, 
	U.usrFullName AS FirmContName
FROM    
	dbo.dbContact C
INNER JOIN
    dbo.dbClientContacts CC ON C.contID = CC.contID 
INNER JOIN
    dbo.dbClient CL ON CC.clID = CL.clID 
INNER JOIN
    dbo.dbUser U ON CL.feeusrID = U.usrID 
LEFT OUTER JOIN
    dbo.dbContactIndividual CI ON C.contID = CI.contID 
LEFT OUTER JOIN
    dbo.dbAddress A ON C.contDefaultAddress = A.addID'

--- SET THE WHERE CLAUSE
SET @WHERE = ' WHERE CC.clActive = 1 '

--- CLTYPE CLAUSE
IF(@CLTYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND CL.clTypeCode = @CLTYPE '
END

--- FIRMCONT CLAUSE
IF(@FIRMCONT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND CL.feeUsrID = @FIRMCONT '
END

--- CLNAME CLAUSE
IF(@CLNAME IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND CL.clName LIKE ''%'' + @CLNAME + ''%'' '
END

--- STARTDATE CLAUSE
IF(@STARTDATE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND (CI.contDOB >= @STARTDATE AND CI.contDOB < @ENDDATE) '
END


--- CONCATENATE THE CLAUSES INTO THE SQL VARIABLE
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE)

-- Debug Print
-- PRINT @SQL

exec sp_executesql @sql,
 N'
	@UI nvarchar(10),
	@CLTYPE nvarchar,
	@FIRMCONT int,
	@CLNAME nvarchar(128),
	@STARTDATE datetime,
	@ENDDATE datetime',
	@UI,
	@CLTYPE,
	@FIRMCONT,
	@CLNAME,
	@STARTDATE,
	@ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliDOB] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliDOB] TO [OMSAdminRole]
    AS [dbo];

