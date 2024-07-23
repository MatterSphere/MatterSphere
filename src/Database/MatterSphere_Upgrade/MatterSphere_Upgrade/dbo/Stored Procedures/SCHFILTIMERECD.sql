CREATE PROCEDURE dbo.SCHFILTIMERECD
(
	@UI uUICultureInfo = '{default}'
	, @ID BIGINT 
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
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(T.timeactivitycode, '''') + ''~'') AS TimeActivityDesc
	, dbo.getuser(feeusrid,''USRINITS'') as TimeInits
FROM dbo.dbtimeledger T
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''TIMEACTCODE'', @UI) CL1 ON CL1.cdCode = T.timeactivitycode
WHERE t.fileid = @ID 
)
SELECT *
FROM Res
'

IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY timerecorded'
ELSE 
	IF @ORDERBY NOT LIKE '%timerecorded%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', timerecorded'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @SELECT, N'@UI uUICultureInfo, @ID BIGINT', @UI, @ID

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFILTIMERECD] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFILTIMERECD] TO [OMSAdminRole]
    AS [dbo];
