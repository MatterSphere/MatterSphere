
CREATE Procedure [dbo].[sprDocumentArchiveDocVersionInfo]
(@docID bigint)
as

select distinct
dv.docID 
,dv.verNumber 
,dv.verLabel 
,dv.verToken 
from dbDocumentVersion  dv
where dv.docID = @docID
order by dv.docID,dv.verNumber,verLabel


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprDocumentArchiveDocVersionInfo] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprDocumentArchiveDocVersionInfo] TO [OMSAdminRole]
    AS [dbo];

