
CREATE PROCEDURE [dbo].[sprRemoveDocumentfromArchiveExclusions]
(	@Docidlist varchar(max)
	,@ArchiveAction nvarchar(1)
)  

AS

 Delete dbDocumentArchiveExclusion 
 from dbDocumentArchiveExclusion AE
 inner join (select items from dbo.SplitStringToTable(@Docidlist,',') ) B
 ON AE.DocID=B.items
 where AE.ArchiveAction=@ArchiveAction



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprRemoveDocumentfromArchiveExclusions] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprRemoveDocumentfromArchiveExclusions] TO [OMSAdminRole]
    AS [dbo];

