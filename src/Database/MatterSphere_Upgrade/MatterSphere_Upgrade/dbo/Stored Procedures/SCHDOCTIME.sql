﻿CREATE PROCEDURE dbo.SCHDOCTIME
(
	@DOCID BIGINT
	, @UI uUICultureInfo = '{default}'
	, @ORDERBY NVARCHAR(MAX) = NULL
)

AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @SELECT NVARCHAR(MAX)

--- SET THE SELECT CLAUSE
SET @SELECT = N' WITH Res AS
(
SELECT T.*
	,  COALESCE(CL1.cdDesc, ''~'' + NULLIF(T.timeActivityCode, '''') + ''~'') AS TimeActivityCodeDesc
FROM dbo.dbTimeLedger T
	LEFT OUTER JOIN dbo.GetCodeLookupDescription(''TIMEACTCODE'', @UI) CL1 ON CL1.cdCode = T.timeActivityCode
WHERE T.docid = @DOCID
)
SELECT *
FROM Res
'

IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY Created DESC'
ELSE 
	IF @ORDERBY NOT LIKE '%Created%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', Created DESC'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @SELECT, N'@UI uUICultureInfo, @DOCID BIGINT', @UI, @DOCID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHDOCTIME] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHDOCTIME] TO [OMSAdminRole]
    AS [dbo];
