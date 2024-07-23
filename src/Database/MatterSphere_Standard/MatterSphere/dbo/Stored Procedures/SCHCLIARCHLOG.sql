CREATE PROCEDURE dbo.SCHCLIARCHLOG
(
	@UI uUICultureInfo = '{default}'
	, @MAX_RECORDS INT = 50
	, @LOGID INT
	, @LOGTYPE NVARCHAR(15) = 'ARCH'
	, @ORDERBY NVARCHAR(MAX) = NULL
)  
AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @Select NVARCHAR(MAX)

SET @Select = N'
WITH Arch AS (
SELECT 
	T.* 
	, CONT.contName
	, COALESCE(CL2.cdDesc, ''~'' + NULLIF(T.trackLocReference, '''') + ''~'') AS StorageLocation
	, UOUT.usrFullName AS CheckedOutByUser
	, UIN.usrFullName AS CheckedInByUser
FROM dbo.dbTracking T
	LEFT OUTER JOIN dbo.dbContact CONT ON CONT.contID = T.trackIssuedTo
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''LOCTYPE'', @UI) CL2 ON CL2.cdCode = T.trackLocReference
	LEFT OUTER JOIN dbo.dbUser UOUT ON UOUT.usrID = T.trackCheckedOutBy
	LEFT OUTER JOIN dbo.dbUser UIN ON UIN.usrID = T.trackCheckedInBy
WHERE T.logID = @LOGID 
	AND T.logType = @LOGTYPE
)
'
IF @MAX_RECORDS > 0
	SET @Select =  @Select + N'
SELECT TOP (@MAX_RECORDS) *
FROM Arch
'
ELSE
	SET @Select =  @Select + N'
SELECT *
FROM Arch
'

IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY trackID DESC'
ELSE 
	IF @ORDERBY NOT LIKE '%trackID%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', trackID DESC'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @Select,  N'@UI uUICultureInfo, @MAX_RECORDS INT, @LOGID INT, @LOGTYPE NVARCHAR(15)', @UI, @MAX_RECORDS, @LOGID, @LOGTYPE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCLIARCHLOG] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCLIARCHLOG] TO [OMSAdminRole]
    AS [dbo];