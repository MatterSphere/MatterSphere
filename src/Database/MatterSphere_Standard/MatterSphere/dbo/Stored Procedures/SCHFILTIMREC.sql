CREATE PROCEDURE dbo.SCHFILTIMREC
(
	@UI uUICultureInfo = '{default}'
	, @docid BIGINT 
	, @ORDERBY NVARCHAR(MAX) = NULL
)  
AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @SELECT NVARCHAR(MAX)


SET @SELECT = N'
WITH Res AS
(
SELECT 
	T.*
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(T.timeactivitycode, '''') + ''~'') AS TimeActivityCodeDesc
	, U.usrInits
FROM dbo.dbtimeledger T
	LEFT OUTER JOIN dbo.dbUser U ON U.usrID = t.feeusrID
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''TIMEACTCODE'', @UI) CL1 ON CL1.cdCode = T.timeactivitycode
WHERE T.docid = @docid
)
SELECT *
FROM Res
'

IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY Created'
ELSE 
	IF @ORDERBY NOT LIKE '%Created%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', Created'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @SELECT, N'@UI uUICultureInfo, @docid BIGINT', @UI, @docid

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFILTIMREC] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFILTIMREC] TO [OMSAdminRole]
    AS [dbo];
