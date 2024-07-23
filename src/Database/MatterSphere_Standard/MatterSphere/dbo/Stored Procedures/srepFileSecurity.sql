

CREATE PROCEDURE [dbo].[srepFileSecurity]
(
	@UI uUICultureInfo = '{default}'
	, @CLNO nvarchar(20) = NULL
)

AS

DECLARE @SELECT nvarchar(2000)
DECLARE @WHERE nvarchar(1000)

--- SET THE SELECT STATEMENT
SET @SELECT = '
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT     
	CL.clNo
	, CL.clName
	, F.fileNo
	, F.fileDesc
	, U.usrFullName
	, FE.evdesc
	, FE.evWhen
FROM         
	dbo.dbFile F
INNER JOIN
	dbo.dbFileEvents FE ON F.fileID = FE.fileID
INNER JOIN
	dbo.dbUser U ON FE.evusrID = U.usrID 
INNER JOIN
	dbo.dbClient CL ON F.clID = CL.clID '

--- SET THE WHERE STATEMENT
SET @WHERE = ' WHERE FE.evtype = ''SECAUDIT'' '

--- ... CLAUSE
IF(@CLNO <> '')
BEGIN
	SET @WHERE = @WHERE + ' AND CL.clNo = @CLNO '
END

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = @SELECT + @WHERE

--- DEBUG PRINT
PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @CLNO nvarchar(20) ' 
	, @UI
	, @CLNO

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileSecurity] TO [OMSAdminRole]
    AS [dbo];

