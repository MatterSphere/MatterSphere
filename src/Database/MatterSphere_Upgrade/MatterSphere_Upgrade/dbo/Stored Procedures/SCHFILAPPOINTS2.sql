CREATE PROCEDURE dbo.SCHFILAPPOINTS2
(
	@UI uUICultureInfo = '{default}'
	, @FILEID BIGINT 
	, @type uCodeLookup = NULL
	, @AllDay BIT = NULL
	, @StartDate DATETIME = NULL
	, @EndDate DATETIME = NULL
	, @ORDERBY NVARCHAR(MAX) = NULL
)  
AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @SELECT NVARCHAR(MAX)
	, @NODUPCOLUMNS NVARCHAR(MAX) = (
SELECT '
	, U.' + COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'dbuser'
	AND TABLE_SCHEMA = 'dbo'
AND COLUMN_NAME NOT IN (
SELECT COLUMN_NAME 
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME IN ('dbuser', 'dbappointments')
	AND TABLE_SCHEMA = 'dbo'
GROUP BY COLUMN_NAME
HAVING COUNT(*) > 1
)
FOR XML PATH(''), TYPE).value('.','NVARCHAR(MAX)')


SET @SELECT = N'
WITH Res AS
(
SELECT 
	A.*' + @NODUPCOLUMNS + N'
	, CASE A.appAllDay WHEN 0 THEN A.appDate END AS appTime
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(A.apptype, '''') + ''~'') AS apptypedesc
	, A.applocation AS applocationdesc
FROM dbo.dbappointments A
	INNER JOIN 	dbo.dbuser U ON U.usrid = A.feeusrid
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''APPTYPE'', @UI) CL1 ON CL1.cdCode = A.apptype
WHERE A.fileID = @FILEID
	'
IF @type IS NOT NULL
	SET @SELECT = @SELECT + N'AND A.apptype = @type
	'
IF @AllDay IS NOT NULL
	SET @SELECT = @SELECT + N'AND A.appAllDay = @AllDay
	'

IF @StartDate IS NOT NULL
	SET @SELECT = @SELECT + N'AND A.appDate >= @StartDate
	'

IF @EndDate IS NOT NULL
	SET @SELECT = @SELECT + N'AND A.appDate <= @EndDate
	'

SET @SELECT = @SELECT + N'
)
SELECT *
FROM Res
'

IF @ORDERBY IS NULL
	SET  @SELECT =  @SELECT + N'ORDER BY appDate'
ELSE 
	IF @ORDERBY NOT LIKE '%appDate%'
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY  + N', appDate'
	ELSE 
		SET  @SELECT =  @SELECT + N'ORDER BY ' + @ORDERBY

EXEC sp_executesql @SELECT, N'@UI uUICultureInfo, @FILEID BIGINT, @type uCodeLookup, @AllDay BIT, @StartDate DATETIME, @EndDate DATETIME', 
	@UI, @FILEID, @type, @AllDay, @StartDate, @EndDate

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFILAPPOINTS2] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHFILAPPOINTS2] TO [OMSAdminRole]
    AS [dbo];