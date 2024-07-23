CREATE PROCEDURE search.RunIndexing
AS
SET NOCOUNT ON
DECLARE @Rel search.ESRelationship
DECLARE @UI dbo.uUICultureInfo = '{default}'
	, @BatchSize BIGINT = (SELECT BatchSize FROM search.ChangeVersionControl)
	, @IndexingEnabled BIT 

EXEC search.GetNewWorkingVersion

EXEC search.GetMSUserAData @UI = @UI, @BatchSize = @BatchSize, @Rel = @Rel
EXEC search.GetMSAddressAData @UI = @UI, @BatchSize = @BatchSize, @Rel = @Rel
EXEC search.GetMSClientAData @UI = @UI, @BatchSize = @BatchSize, @Rel = @Rel
EXEC search.GetMSContactAData @UI = @UI, @BatchSize = @BatchSize, @Rel = @Rel
EXEC search.GetMSCCLinkAData @UI = @UI, @BatchSize = @BatchSize, @Rel = @Rel
EXEC search.GetMSFileAData @UI = @UI, @BatchSize = @BatchSize, @Rel = @Rel
EXEC search.GetMSAssociateAData @UI = @UI, @BatchSize = @BatchSize, @Rel = @Rel
EXEC search.GetMSDocumentAData @UI = @UI, @BatchSize = @BatchSize, @Rel = @Rel
EXEC search.GetMSPrecedentAData @UI = @UI, @BatchSize = @BatchSize, @Rel = @Rel

SELECT	@IndexingEnabled  = IndexingEnabled
FROM search.ESIndexTable CROSS APPLY search.ChangeVersionControl 
WHERE ObjectType = 'APPOINTMENT'

IF @IndexingEnabled = 1
	EXEC search.GetMSAppointmentAData @UI = @UI, @BatchSize = @BatchSize, @Rel = @Rel

SET @IndexingEnabled = 0

SELECT	@IndexingEnabled  = IndexingEnabled
FROM search.ESIndexTable CROSS APPLY search.ChangeVersionControl 
WHERE ObjectType = 'EMAIL'

IF @IndexingEnabled = 1
	EXEC search.GetMSEmailAData @UI = @UI, @BatchSize = @BatchSize, @Rel = @Rel

SET @IndexingEnabled = 0

SELECT	@IndexingEnabled  = IndexingEnabled
FROM search.ESIndexTable CROSS APPLY search.ChangeVersionControl 
WHERE ObjectType = 'TASK'

IF @IndexingEnabled = 1
	EXEC search.GetMSTaskAData @UI = @UI, @BatchSize = @BatchSize, @Rel = @Rel

SET @IndexingEnabled = 0

SELECT	@IndexingEnabled  = IndexingEnabled
FROM search.ESIndexTable CROSS APPLY search.ChangeVersionControl 
WHERE ObjectType = 'NOTE'

IF @IndexingEnabled = 1
	EXEC search.GetMSNoteAData @UI = @UI, @BatchSize = @BatchSize, @Rel = @Rel

EXEC search.UpdateChangeTrackingVersion