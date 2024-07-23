
CREATE PROCEDURE [dbo].[sprClearDocumentArchiveTempList]
(
	@ArchiveAction nvarchar(1)
	,@Requestedby int
)
AS

DELETE FROM 
	[dbo].[dbDocumentArchiveTempList] 
WHERE
	ArchiveAction = ISNULL(@ArchiveAction, ArchiveAction)
	AND Requestedby = ISNULL(RequestedBy, @Requestedby)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprClearDocumentArchiveTempList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprClearDocumentArchiveTempList] TO [OMSAdminRole]
    AS [dbo];

