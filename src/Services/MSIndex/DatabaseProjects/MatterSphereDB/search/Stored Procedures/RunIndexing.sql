CREATE PROCEDURE search.RunIndexing
AS
SET NOCOUNT ON
DECLARE @UI dbo.uUICultureInfo = '{default}'
	, @BatchSize BIGINT = (SELECT BatchSize FROM search.ChangeVersionControl)
	, @IndexingEnabled BIT 

EXEC search.GetNewWorkingVersion

EXEC search.GetMSUserAData @UI = @UI, @BatchSize = @BatchSize
EXEC search.GetMSAddressAData @UI = @UI, @BatchSize = @BatchSize
EXEC search.GetMSClientAData @UI = @UI, @BatchSize = @BatchSize
EXEC search.GetMSContactAData @UI = @UI, @BatchSize = @BatchSize
EXEC search.GetMSFileAData @UI = @UI, @BatchSize = @BatchSize
EXEC search.GetMSAssociateAData @UI = @UI, @BatchSize = @BatchSize
EXEC search.GetMSDocumentAData @UI = @UI, @BatchSize = @BatchSize
EXEC search.GetMSPrecedentAData @UI = @UI, @BatchSize = @BatchSize
EXEC search.GetMSAppointmentAData @UI = @UI, @BatchSize = @BatchSize
EXEC search.GetMSTaskAData @UI = @UI, @BatchSize = @BatchSize

EXEC search.UpdateChangeTrackingVersion