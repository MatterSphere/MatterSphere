CREATE PROCEDURE dbo.SCHCOMAWAITACK
(
	@ORDERBY NVARCHAR(MAX) = NULL
)

AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @SELECT NVARCHAR(MAX)

--- SET THE SELECT CLAUSE
SET @SELECT = N'WITH Res AS
(
SELECT
	F.clid
	, F.fileid
	, dbo.getfilerefbyid(F.fileid) AS OurRef
	, C.clname
	, F.filedesc
	, U.usrfullname
	, F.created
	, C.clautosource
	, F.filestatus
	, F.fileextlinktxtid
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') AS FileTypeDesc
FROM dbClient C
	INNER JOIN dbFile F ON F.clid = C.clid 
	LEFT OUTER JOIN dbo.dbuser U ON U.usrid = F.fileprincipleid
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''FILETYPE'', ''en-gb'') CL1 ON CL1.cdCode = F.filetype
WHERE F.filestatus = ''LIVEAWAITACK''
)
SELECT *
FROM Res
'
IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY clid'
ELSE 
	IF @ORDERBY NOT LIKE '%clid%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', clid'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @SELECT

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCOMAWAITACK] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCOMAWAITACK] TO [OMSAdminRole]
    AS [dbo];