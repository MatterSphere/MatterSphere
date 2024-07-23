
CREATE Procedure [dbo].[sprDocArchiveGetList]
as
select 
	da.ArchID as ArchID
	, da.ArchDocID as ArchDocID
	, da.ArchDocFileID  as ArchDocFileID
	, da.ArchAction as ArchAction
	, da.ArchMessage as ArchMessage
	, da.ArchDirID  as ArchDirID
	, da.Created as Created
	, da.CreatedBy as CreatedBy
	, d1.dirPath as SourceDirectory
	, d.dirPath as DestinationDirectory
	, da.Archived as Archived
	, da.ArchStatus as ArchStatus
	, da.RunID as RunID
	, da.docdirID as docdirID
from 
	dbDocumentArchiveInfo da
	inner join dbdocument doc on da.ArchDocID=doc.docID
	inner join dbDirectory d on  da.ArchDirID=d.dirID
	inner join dbDirectory d1 on doc.docdirID =d1.dirID
where 
	da.ArchAction in ('A', 'D') -- Include Items for Archiving and Deletion only
	AND da.RunID =-1
	--and da.Archived is null
	and ArchStatus = 1	-- new ones only


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprDocArchiveGetList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprDocArchiveGetList] TO [OMSAdminRole]
    AS [dbo];

