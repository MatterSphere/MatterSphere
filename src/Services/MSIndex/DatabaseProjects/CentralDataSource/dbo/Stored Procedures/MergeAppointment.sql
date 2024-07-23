CREATE PROCEDURE dbo.MergeAppointment (@Source AS dbo.AppointmentMSType READONLY)
AS
SET NOCOUNT ON;

MERGE dbo.Appointment AS target
USING @Source AS source ON (target.mattersphereid = source.mattersphereid)
WHEN NOT MATCHED BY TARGET THEN
	INSERT (mattersphereid, appDesc, appLocation, modifieddate, [file-id], [client-id], [associate-id], [document-id], [appointmentType])
	VALUES(source.mattersphereid, source.appDesc, source.appLocation, source.modifieddate, source.[file-id], source.[client-id], source.[associate-id], source.[document-id], source.[appointmentType])
WHEN MATCHED AND source.op = 'D'
    THEN DELETE  
WHEN MATCHED
	THEN UPDATE SET target.appDesc = source.appDesc
		, target.appLocation = source.appLocation
		, target.modifieddate = source.modifieddate
		, target.[file-id] = source.[file-id]
		, target.[client-id] = source.[client-id]
		, target.[associate-id] = source.[associate-id]
		, target.[document-id] = source.[document-id]
		, target.appointmentType = source.appointmentType
;
