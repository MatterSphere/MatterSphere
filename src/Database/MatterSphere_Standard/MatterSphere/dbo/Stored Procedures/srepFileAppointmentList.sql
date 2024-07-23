

CREATE PROCEDURE [dbo].[srepFileAppointmentList] 
(
	@UI uUICultureInfo='{default}',
	@ClNo nvarchar(12) ,
	@fileno nvarchar(20) 
)

AS 
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	AP.appDate,
	CL.clname,
	replace(F.fileDesc, char(13) + char(10), ', ') as fileDesc,
	AP.appLocation,
	AP.appDesc,
	AP.appType, 
    AP.appLocation,
	X.cdDesc AS AppTypeDesc,
	CL.clNo,
	F.fileNo,
	U.usrInits,
	U.usrFullName, 
    AP.feeusrID,
	dbUser_1.usrInits AS InitsWith,
	dbUser_1.usrFullName AS FullNameWith
		, AP.appAllDay
FROM
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID
INNER JOIN
    dbo.dbUser U ON F.filePrincipleID = U.usrID
INNER JOIN
    dbo.dbAppointments AP ON F.fileID = AP.fileID AND AP.appactive = 1
INNER JOIN
    dbUser AS dbuser_1 ON AP.feeusrID = dbUser_1.usrID
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription('APPTYPE', @UI) AS X ON X.cdCode = AP.appType 
WHERE
	CL.clNo = @clNo 
AND
	F.fileNo = @fileNo
ORDER BY
	AP.appdate

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileAppointmentList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileAppointmentList] TO [OMSAdminRole]
    AS [dbo];

