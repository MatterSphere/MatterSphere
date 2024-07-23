

CREATE PROCEDURE [dbo].[srepCliConv]
(
	@UI uUICultureInfo='{default}',
	@FIRMCONT int = NULL,
	@STARTDATE datetime = NULL,
	@ENDDATE datetime = NULL
)

AS 

DECLARE @SQL nvarchar(2500)
DECLARE @SELECT nvarchar(1500)
DECLARE @WHERE nvarchar(1000)
DECLARE @ORDERBY nvarchar(200)

--- Select Statement for the base Query
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT     
	dbo.GetFileRef(CL.clNo, F.fileNo) AS Ref, 
	CL.clPreClientConvDate, 
	CL.Created, 
	DATEDIFF(d, CL.created, CL.clPreClientConvDate) AS daysdiff, 
	CL.clName, 
	U.usrInits AS FirmContInits, 
	U.usrFullName AS FirmContName, 
	CL.clPreClient
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
LEFT OUTER JOIN
	dbo.dbUser U ON CL.feeusrID = U.usrID'

--- SET THE WHERE CLAUSE
SET @WHERE = ' WHERE CL.clPreClientConvDate IS NOT NULL AND CL.clPreClient = 0 '

--- FIRMCONT CLAUSE
IF(@FIRMCONT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND CL.feeUsrid = @FIRMCONT '
END

--- STARTDATE CLAUSE
IF(@STARTDATE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + 
	' AND (CL.clPreClientConvDate >= @STARTDATE AND CL.clPreClientConvDate < @ENDDATE) '
END

-- Set the Order By Clause
SET @ORDERBY = N' ORDER BY FirmContName, daysDiff '

--- CONCATENATE THE CLAUSES INTO THE SQL VARIABLE
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

-- Debug Print
-- PRINT @SQL

exec sp_executesql @sql,
 N'
	@UI nvarchar(10),
	@FIRMCONT int,
	@STARTDATE datetime,
	@ENDDATE datetime',
	@UI,
	@FIRMCONT,
	@STARTDATE,
	@ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliConv] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliConv] TO [OMSAdminRole]
    AS [dbo];

