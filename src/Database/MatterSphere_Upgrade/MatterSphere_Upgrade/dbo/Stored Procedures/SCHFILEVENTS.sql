CREATE PROCEDURE dbo.SCHFILEVENTS
(
	@UI uUICultureInfo = '{default}'
	, @FILEID BIGINT 
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
	FE.evID
	, FE.fileID
	, FE.evType
	, FE.evDesc
	, FE.evExtended
	, FE.evusrID
	, FE.evWhen
	, USR.*
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(FE.evType, '''') + ''~'') AS evTypeDesc
FROM dbo.dbfileevents FE
	LEFT JOIN dbo.dbuser USR ON evusrid = USR.usrid 
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''FILEEVENT'', @UI) CL1 ON CL1.cdCode = FE.evType
WHERE FE.fileid = @FILEID
)
SELECT *
FROM Res
'

IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY evwhen'
ELSE 
	IF @ORDERBY NOT LIKE '%evwhen%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', evwhen'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @SELECT, N'@UI uUICultureInfo, @FILEID BIGINT', @UI, @FILEID

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFILEVENTS] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFILEVENTS] TO [OMSAdminRole]
    AS [dbo];
