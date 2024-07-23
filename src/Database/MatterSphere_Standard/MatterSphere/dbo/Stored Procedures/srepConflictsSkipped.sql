

CREATE PROCEDURE [dbo].[srepConflictsSkipped] 
(
	@STARTDATE datetime = null
	, @ENDDATE datetime = null
)

AS 

DECLARE @SELECT nvarchar(2000)
DECLARE @WHERE nvarchar(1000)
DECLARE @SQL nvarchar(3000)

--- BUILD THE SELECT CLAUSE
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT 
	C.clNo + ''/'' + F.fileNo AS [Ref]
	, C.clName
	, F.fileDesc
	, U.usrFullName
	, CON.evWhen 
FROM 
(
	SELECT
		E.fileID,
		E.evUsrID,
		Max(E.evWhen) as evWhen
	FROM
		dbFileEvents E
	WHERE
		E.evType = ''CONFLICTSKIPPED''
	GROUP BY
		E.fileID,
		E.evUsrID
) CON
LEFT OUTER JOIN
(
	SELECT
		E.fileID,
		E.evUsrID,
		Count(E.fileID) as ConflictDone
	FROM
		dbFileEvents E
	WHERE
		E.evType = ''CONFLICTDONE''
	GROUP BY
		E.fileID,
		E.evUsrID
) DONE ON DONE.fileID = CON.fileID AND DONE.evUsrID = CON.evUsrID
INNER JOIN 
	dbFile F on F.fileId = CON.fileID
INNER JOIN 
	dbClient C on C.ClID = F.ClID
INNER JOIN 
	dbUser U on U.usrID = CON.evusrID'

-- Build the where clause
SET @WHERE = N' WHERE DONE.ConflictDone IS NULL' 

-- Start date clause
IF (@STARTDATE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND (CON.evWhen >= @STARTDATE AND CON.evWhen < @ENDDATE)' 
END

--- ADD THE CLAUSES TOGETHER
SET @SQL = @SELECT + @WHERE

--- DEBUG PRINT
-- PRINT @SQL


--- EXECUTE THE PROCEDURE
EXEC sp_executesql @SQL, 
N'
	@STARTDATE datetime
	, @ENDDATE datetime'
	, @STARTDATE
	, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepConflictsSkipped] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepConflictsSkipped] TO [OMSAdminRole]
    AS [dbo];

