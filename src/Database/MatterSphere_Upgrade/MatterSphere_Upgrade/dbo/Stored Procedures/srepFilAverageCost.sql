CREATE PROCEDURE [dbo].[srepFilAverageCost]
(
	@UI uUICultureInfo = '{default}'
	, @DEPARTMENT nvarchar(30) = NULL
	, @FILETYPE nvarchar(30) = NULL
	, @STARTDATE DateTime = NULL
	, @ENDDATE DateTime = NULL
)

AS 

DECLARE @SELECT nvarchar(MAX)
DECLARE @WHERE nvarchar(1500)

--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT
	CL.clName,
	U.usrFullName,
	F.fileNo,
	F.fileDesc,
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileDepartment, '''') + ''~'') AS fileDepartment,  
	COALESCE(CL2.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') AS fileType, 
	TL.timeCost, 
	TL.timeCharge
FROM 
	dbTimeLedger TL 
INNER JOIN 
	dbFile F on TL.fileID = F.fileID
INNER JOIN
	dbClient CL ON CL.clID = F.clID
INNER JOIN
	dbUser U ON U.usrID = F.filePrincipleID
LEFT JOIN dbo.GetCodeLookupDescription ( ''DEPT'', @UI ) CL1 ON CL1.[cdCode] = F.fileDepartment
LEFT JOIN dbo.GetCodeLookupDescription ( ''FILETYPE'', @UI ) CL2 ON CL2.[cdCode] = F.fileType'

---- SET THE WHERE STATEMENT
SET @WHERE = ' WHERE F.fileStatus = ''DEAD'' '

--- DEPARTMENT CLAUSE
IF(@DEPARTMENT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.fileDepartment = @DEPARTMENT '
END

--- FILE TYPE CLAUSE
IF(@FILETYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.fileType = @FILETYPE '
END

--- FILECLOSED START DATE AND END DATE
IF(@STARTDATE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND (F.fileClosed >= @STARTDATE AND F.fileClosed < @ENDDATE) '
END


DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE)

--- DEBUG PRINT
-- PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI uUICultureInfo
	, @DEPARTMENT nvarchar(30)
	, @FILETYPE nvarchar(30)
	, @STARTDATE DateTime
	, @ENDDATE DateTime'
	, @UI
	, @DEPARTMENT
	, @FILETYPE
	, @STARTDATE
	, @ENDDATE
