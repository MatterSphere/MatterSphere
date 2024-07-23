

CREATE PROCEDURE [dbo].[sprFetchStorageItem] (@token uniqueidentifier = null, @docid bigint = null, @precid bigint = null)
AS

	if not @token is null
	begin
		select data from dbstorage where token = @token
	end
	else
	begin
		if not @docid is null
		begin
			select docblob as [data] from dbdocumentstorage where docid = @docid
		end
		else if not @precid is null
		begin
			select precblob as [data] from dbprecedentstorage where precid = @precid
		end
		else
			select null as [data]
	
	end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFetchStorageItem] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFetchStorageItem] TO [OMSAdminRole]
    AS [dbo];

