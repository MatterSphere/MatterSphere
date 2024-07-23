

CREATE PROCEDURE [dbo].[sprUpdateDocumentPreview] (@docid bigint = null, @text ntext, @version uniqueidentifier= null) AS

	begin transaction
	if not @docid is null
	begin 

		if (select count(docid) from config.dbdocumentpreview where docid = @docid) > 0
			update config.dbdocumentpreview set docpreview = @text where docid = @docid
		else
			insert into config.dbdocumentpreview (docid, docpreview) values (@docid, @text)
	end

	if not @version is null
	begin
		if (select count(*) from dbdocumentversionpreview where verid = @version) > 0
			update dbdocumentversionpreview set verpreview = @text where verid = @version
		else
			insert into dbdocumentversionpreview (verid, verpreview) values (@version, @text)
	end
	commit transaction

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprUpdateDocumentPreview] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprUpdateDocumentPreview] TO [OMSAdminRole]
    AS [dbo];

