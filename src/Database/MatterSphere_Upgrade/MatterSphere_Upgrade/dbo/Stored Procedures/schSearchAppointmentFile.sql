﻿CREATE PROCEDURE dbo.schSearchAppointmentFile 
(
	@UI uUICultureInfo = '{default}'
	, @FILEID BIGINT = 0
	, @MAX_RECORDS INT = 50
	, @TYPE NVARCHAR(20) = NULL
	, @ALLDAY NVARCHAR(20) = NULL
	, @STARTDATE DATETIME = NULL
	, @ENDDATE DATETIME = NULL
	, @PageNo INT = NULL
	, @ACTIVE BIT = NULL
	, @ORDERBY NVARCHAR(MAX) = NULL
)
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

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

SET @SELECT = 
	N'
DECLARE @OFFSET INT = 0,
	@TOTAL int  = 0, @TOP INT
	
IF @MAX_RECORDS > 0
	SET @TOP = @MAX_RECORDS
ELSE
	SET @TOP = 50

IF @PageNo IS NULL
	SET @OFFSET = 0
ELSE
	SET @OFFSET = @TOP * (@PageNo- 1)


DECLARE @Res TABLE(
	Id BIGINT PRIMARY KEY
	, appID BIGINT
);

WITH Res AS
(
 SELECT APP.*' + @NODUPCOLUMNS + N'
	, APP.appDate AS appDate2
	, CASE APP.appAllDay WHEN 0 THEN APP.appDate END AS appStartTime
	, CASE APP.appAllDay WHEN 0 THEN APP.appEndDate END AS appEndTime
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(APP.apptype, '''') + ''~'') AS apptypedesc
	, APP.applocation AS applocationdesc 
FROM dbo.dbappointments APP
	INNER JOIN dbo.dbuser U ON U.usrid = APP.feeusrid
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''APPTYPE'', @UI) CL1 ON CL1.cdCode = APP.apptype
WHERE APP.fileID = @FILEID 
	'
IF @TYPE IS NOT NULL
	SET @SELECT = @SELECT + N'AND APP.apptype = @TYPE
	'

IF @ALLDAY IS NOT NULL
	SET @SELECT = @SELECT + N'AND APP.appAllDay = @ALLDAY
	'

IF @ACTIVE IS NOT NULL
	SET @SELECT = @SELECT + N'AND APP.appActive = @ACTIVE
	'

IF @STARTDATE IS NOT NULL
	SET @SELECT = @SELECT + N'AND APP.appDate >= @STARTDATE
	'

IF @ENDDATE IS NOT NULL
	SET @SELECT = @SELECT + N'AND APP.appDate <= @ENDDATE
	'
SET @SELECT = @SELECT + N'
)

INSERT INTO @Res (appId, Id)
SELECT 
	appID
	'

IF @ORDERBY IS NULL
	SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY appID)'
ELSE 
	IF @ORDERBY NOT LIKE '%appID%'
		SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY ' + @ORDERBY  + N', appID)'
	ELSE 
		SET @SELECT = @SELECT + N', ROW_NUMBER() OVER(ORDER BY ' + @ORDERBY  + N')'

SET @SELECT = @SELECT + N'
FROM Res

SET @Total = @@ROWCOUNT

SELECT TOP(@TOP) APP.*' + @NODUPCOLUMNS + N'
	, APP.appDate AS appDate2
	, CASE APP.appAllDay WHEN 0 THEN APP.appDate END AS appStartTime
	, CASE APP.appAllDay WHEN 0 THEN APP.appEndDate END AS appEndTime
	, COALESCE(CL1.cdDesc, ''~'' + NULLIF(APP.apptype, '''') + ''~'') AS apptypedesc
	, APP.applocation AS applocationdesc 
	, @TOTAL as Total
FROM @RES res
	INNER JOIN dbo.dbappointments APP ON APP.appID = res.appID
	INNER JOIN dbo.dbuser U ON U.usrid = APP.feeusrid
	LEFT OUTER JOIN dbo.GetCodeLookupDescription (''APPTYPE'', @UI) CL1 ON CL1.cdCode = APP.apptype
WHERE res.Id > @OFFSET 
ORDER BY res.Id
'

PRINT @SELECT

EXEC sp_executesql @SELECT,  N'@UI uUICultureInfo, @FILEID BIGINT, @MAX_RECORDS INT, @TYPE NVARCHAR(20), @ALLDAY NVARCHAR(20), @STARTDATE DATETIME, @ENDDATE DATETIME, @PageNo INT, @ACTIVE BIT',
	@UI, @FILEID, @MAX_RECORDS, @TYPE, @ALLDAY, @STARTDATE, @ENDDATE, @PageNo, @ACTIVE 
GO
GRANT EXECUTE
	ON OBJECT::[dbo].[schSearchAppointmentFile] TO [OMSRole]
	AS [dbo];


GO
GRANT EXECUTE
	ON OBJECT::[dbo].[schSearchAppointmentFile] TO [OMSAdminRole]
	AS [dbo];
