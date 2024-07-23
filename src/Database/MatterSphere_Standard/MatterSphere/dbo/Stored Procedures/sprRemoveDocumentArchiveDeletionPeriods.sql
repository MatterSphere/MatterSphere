
CREATE PROCEDURE [dbo].[sprRemoveDocumentArchiveDeletionPeriods]

AS

delete from dbDocumentArchiveDeletionPeriod


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprRemoveDocumentArchiveDeletionPeriods] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprRemoveDocumentArchiveDeletionPeriods] TO [OMSAdminRole]
    AS [dbo];

