
CREATE PROCEDURE [dbo].[sprEditDocumentArchiveDeletionPeriods]
(
	@listOfIDs nvarchar(max)
	,@deletionPeriod smallint
	,@UpdatedBy int
	,@Updated datetime
)
AS

declare @delimiter nvarchar(1) 
SET @delimiter = ','

update 
	dbDocumentArchiveDeletionPeriod
set 
	deletionPeriod = @deletionPeriod
	,updatedBy = @UpdatedBy
	,updated = @Updated
where 
	ID in (select items from dbo.SplitStringToTable(@listOfIDs,@delimiter))

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprEditDocumentArchiveDeletionPeriods] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprEditDocumentArchiveDeletionPeriods] TO [OMSAdminRole]
    AS [dbo];

