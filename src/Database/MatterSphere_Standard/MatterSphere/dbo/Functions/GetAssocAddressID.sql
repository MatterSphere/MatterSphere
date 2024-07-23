

CREATE FUNCTION [dbo].[GetAssocAddressID] (@AssocID bigint)  
RETURNS bigint AS  
BEGIN 
declare @clid bigint
declare @contid bigint
declare @assoctype uCodeLookup
declare @addid bigint
declare @setting bit

select @clid = dbfile.clid, @contid = dbassociates.contid, @assoctype = dbassociates.assoctype, @addid = dbassociates.assocdefaultaddid, @setting = assocdefaddsetting
from dbassociates INNER JOIN dbfile on dbassociates.fileid = dbfile.fileid
where associd = @associd

if @addid IS NULL
begin
	if (select count(*)  from dbclientcontacts where clid = @clid and contid = @contid) > 0 and @setting = 0
	begin
		select @addid = cldefaultaddress from dbclient where clid = @clid
	end
	else
	begin
		select @addid = contdefaultaddress from dbcontact where contid = @contid
	end
end

return @addid			
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAssocAddressID] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAssocAddressID] TO [OMSAdminRole]
    AS [dbo];

