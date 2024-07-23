

CREATE VIEW[dbo].[vwDBAppointments]
AS
SELECT     dbo.dbAppointments.appID AS AppointmentID, dbo.dbAppointments.fileID AS MatterID, dbo.dbAppointments.clID, dbo.dbAppointments.appDesc, 
                      dbo.dbUser.usrInits AS AppFeeEarner, dbo.dbAppointments.appDate, '' AS AppInOutlook, dbo.dbAppointments.appType, dbo.dbAppointments.appNotes, 
                      dbo.dbAppointments.assocID AS AppAssocRef, dbo.dbAppointments.appLocation, dbo.dbAppointments.appAtAssociate, 0 AS DocRef
FROM         dbo.dbAppointments INNER JOIN
                      dbo.dbUser ON dbo.dbAppointments.feeusrID = dbo.dbUser.usrID

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBAppointments] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBAppointments] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBAppointments] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBAppointments] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBAppointments] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBAppointments] TO [OMSApplicationRole]
    AS [dbo];

