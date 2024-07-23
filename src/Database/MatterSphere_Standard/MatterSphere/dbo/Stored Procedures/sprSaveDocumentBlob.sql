

CREATE PROCEDURE [dbo].[sprSaveDocumentBlob] (@DOCID bigint, @DOCUMENT image) AS
if exists (select docid from dbdocumentstorage where docid = @DOCID)
	update dbdocumentstorage set docblob = @DOCUMENT where docid = @DOCID
else
	insert into dbdocumentstorage (docid, docblob) values (@DOCID, @DOCUMENT)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprSaveDocumentBlob] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprSaveDocumentBlob] TO [OMSAdminRole]
    AS [dbo];

