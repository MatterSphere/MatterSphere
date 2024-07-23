

CREATE PROCEDURE [dbo].[srepSMSCounter]
(
	@UI uUICultureInfo = '{default}'
	, @DEPARTMENT nvarchar(15) = NULL
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
)

AS 

DECLARE @SELECT nvarchar(2500)
DECLARE @WHERE nvarchar(1250)
DECLARE @GROUPBY nvarchar(250)

--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT
	CL.clNo + ''/'' + F.fileNo AS OurRef
	, CL.clName
	, F.fileDesc
	, D.Created AS Created
	, COUNT(D.docID) AS TotalSMS
	, U.usrInits AS SentBy
FROM
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON F.clID = CL.clID
INNER JOIN
	dbo.dbDocument D ON F.fileID = D.fileID
INNER JOIN
	dbo.dbUser U ON U.usrID = D.CreatedBy '

--- SET THE WHERE CLAUSE
SET @WHERE = ' WHERE D.docType = ''SMS'' '

--- DEPARTMENT CLAUSE
IF(@DEPARTMENT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.fileDepartment = @DEPARTMENT '
END

--- DOCUMENT CREATED START DATE AND END DATE
IF(@STARTDATE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND (D.Created >= @STARTDATE AND D.Created < @ENDDATE) '
END

--- SET THE GROUP BY STATEMENT
SET @GROUPBY = N'
GROUP BY
	CL.clNo
	, F.fileNo
	, CL.clName
	, F.fileDesc
	, D.Created
	, U.usrInits '

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@GROUPBY)

-- DEBUG PRINT
-- PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @DEPARTMENT nvarchar(15)
	, @STARTDATE datetime
	, @ENDDATE datetime '
	, @UI
	, @DEPARTMENT
	, @STARTDATE
	, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepSMSCounter] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepSMSCounter] TO [OMSAdminRole]
    AS [dbo];

