CREATE PROCEDURE dbo.dshCalendar (@UI uUICultureInfo = '{default}', @StarDT DATETIME, @EndDT DATETIME)
AS
SET NOCOUNT ON
SET TRAN ISOLATION LEVEL READ UNCOMMITTED

DECLARE @USERID NVARCHAR(200) = (SELECT [config].[GetUserLogin]())
	, @usrID INT 

SET @usrID = (SELECT usrID FROM dbo.dbUser WHERE usrADID = @USERID)

SELECT 
	a.appID
	, a.clID
	, a.fileID
	, COALESCE(APPType.cdDesc, '~' + NULLIF(a.appType, '') + '~') AS AppTypeDesc
	, a.appDesc
	, a.appLocation
	, a.appDate
	, a.appEndDate
	, a.appAllDay
	, a.appTimeZone
FROM dbo.dbAppointments a 
	LEFT OUTER JOIN dbo.GetCodeLookupDescription('APPTYPE',@UI) APPType on APPType.cdCode = a.appType
WHERE ((a.appDate < @EndDT AND a.appEndDate > @StarDT) OR (a.appAllDay = 1 AND (a.appDate = @StarDT OR a.appDate = @EndDT)))
	AND a.feeusrID = @usrID
	AND a.appActive = 1

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dshCalendar] TO [OMSRole]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dshCalendar] TO [OMSAdminRole]
    AS [dbo];
