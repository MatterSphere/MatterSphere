
CREATE PROCEDURE [dbo].[sprAddDocumentArchiveDeletionPeriods]
(
	@deletionPeriod smallint
	,@CreatedBy int
	,@Created datetime
)

AS

insert into dbDocumentArchiveDeletionPeriod (fileType, brID, deletionPeriod, created, createdBy)
select 
	typeCode 
	, brid
	, @deletionPeriod
	, @Created
	, @CreatedBy
from
	dbFileType f
	cross join dbbranch b
where
	typeCode + convert(nvarchar(max), brid) not in (select filetype + convert(nvarchar(max), brid) from dbDocumentArchiveDeletionPeriod)
order by
	typeCode, brID




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddDocumentArchiveDeletionPeriods] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddDocumentArchiveDeletionPeriods] TO [OMSAdminRole]
    AS [dbo];

