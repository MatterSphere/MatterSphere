

CREATE PROCEDURE [dbo].[sprStoreStorageItem] (@token uniqueidentifier, @data image, @docid bigint = null, @precid bigint = null, @latest bit = 1)
as

	begin transaction

	if exists (select token from dbstorage where token = @token)
		update dbstorage set data = @data where token = @token
	else
		insert into dbstorage (token, data) values (@token, @data)


	if @latest = 1
	begin
		if not @docid is null
			execute sprSaveDocumentBLOB @docid, @data
		else if not @precid is null
			execute sprSavePRecedentBLOB @precid, @data
		
	end

	commit transaction

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprStoreStorageItem] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprStoreStorageItem] TO [OMSAdminRole]
    AS [dbo];

