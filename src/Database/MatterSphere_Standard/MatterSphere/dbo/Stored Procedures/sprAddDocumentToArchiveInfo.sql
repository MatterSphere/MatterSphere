
CREATE PROCEDURE [dbo].[sprAddDocumentToArchiveInfo]
(	
	@Action nvarchar(1),
	@ArchiveDirID smallint,
	@RequestedBy int,
    @Created datetime
)  
as



INSERT INTO dbDocumentArchiveInfo
select 
	ArchTempList.DocID, 
	ArchTempList.FileID,
	@Action,
	' ',
	CASE @Action WHEN 'D' THEN docdirID ELSE @ArchiveDirID END,
	@RequestedBy,
	@Created,
	-1,	
	null,
	docdirID,
	1,
	NEWID()
from 
	dbDocumentArchiveTempList  AS ArchTempList
where 
	ISNULL(ArchTempList.ImageColumn,-1)<>6
	and ArchTempList.Requestedby=@RequestedBy
	and ArchiveAction = @Action





GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddDocumentToArchiveInfo] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddDocumentToArchiveInfo] TO [OMSAdminRole]
    AS [dbo];

