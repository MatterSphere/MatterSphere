

CREATE PROCEDURE [dbo].[sprSavePrecedentBlob] (@PRECID bigint, @PRECEDENT image) AS
if exists (select precID from dbprecedentstorage where precid = @PRECID)
	update dbprecedentstorage set precblob = @PRECEDENT where precid = @PRECID
else
	insert into dbprecedentstorage (precid, precblob) values (@PRECID, @PRECEDENT)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprSavePrecedentBlob] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprSavePrecedentBlob] TO [OMSAdminRole]
    AS [dbo];

