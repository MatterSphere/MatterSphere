

CREATE  PROCEDURE [dbo].[sprDocumentRecord]  (@DOCID bigint, @UI uUICultureInfo = '{default}',@DUPLICATECHECK BIT = 0) 
AS

if @DUPLICATECHECK = 1
begin
	select DOC.*, SP.sptype, DIR.dirpath as [docLivePath], ARCH.dirpath as [docArchivePath]from config.dbdocument DOC
	left join dbstorageprovider SP on DOC.doclocation = SP.spid
	left join dbdirectory DIR on DOC.docDirID = DIR.dirid
	left join dbdirectory ARCH on DOC.docDirID = ARCH.dirid
	where exists  (select documentid from [config].[searchDocumentAccess] ('DOCUMENT',@DOCID) r where r.[documentid] = DOC.docid) and DOC.docid = @DOCID 
	or (exists  (select documentid from [config].[searchDocumentAccess] ('DOCUMENT',@DOCID) r where r.[documentid] = DOC.docid) and DOC.docidold = @DOCID and DOC.docid <> DOC.docidold)
end 
else
begin
	select DOC.*, SP.sptype, DIR.dirpath as [docLivePath], ARCH.dirpath as [docArchivePath]from config.dbdocument DOC
	left join dbstorageprovider SP on DOC.doclocation = SP.spid
	left join dbdirectory DIR on DOC.docDirID = DIR.dirid
	left join dbdirectory ARCH on DOC.docDirID = ARCH.dirid
	where exists  (select documentid from [config].[searchDocumentAccess] ('DOCUMENT',@DOCID) r where r.[documentid] = DOC.docid) and DOC.docid = @DOCID 
end


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprDocumentRecord] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprDocumentRecord] TO [OMSAdminRole]
    AS [dbo];

