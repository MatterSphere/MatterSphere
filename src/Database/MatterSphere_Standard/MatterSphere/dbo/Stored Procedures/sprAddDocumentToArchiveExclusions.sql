
CREATE PROCEDURE [dbo].[sprAddDocumentToArchiveExclusions]
(	@Docidlist varchar(max),
	@CreatedBy bigint,
	@Created Datetime,
	@ArchiveAction nvarchar(1)
)

AS

INSERT INTO dbDocumentArchiveExclusion
select items, d.fileID, @CreatedBy, @Created, @ArchiveAction, NEWID()
from dbo.SplitStringToTable(@Docidlist,',') as DocList
inner join dbo.dbDocument d on DocList.items = d.docID


SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddDocumentToArchiveExclusions] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddDocumentToArchiveExclusions] TO [OMSAdminRole]
    AS [dbo];

