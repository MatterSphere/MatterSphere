

CREATE PROCEDURE [dbo].[sprContactDefaultAddress] (@ContID bigint, @Active smallint =-1, @UI uUICultureInfo = '{default}', @addid bigint)  AS
--Get Addresses order by ContOrder where ContType=Active
	if @Active <> -1 
Begin
	SELECT dbContactAddresses.contaddID, dbContactAddresses.contID, dbContactAddresses.contActive, dbContactAddresses.contOrder,  dbo.GetCodeLookupDesc('INFOADDTYPE', dbContactAddresses.contCode, @UI) as ContTypeDesc,  dbContactAddresses.contCode, dbAddress.*
		FROM  dbContactAddresses INNER JOIN
		               dbAddress ON dbContactAddresses.contaddID = dbAddress.addID
		WHERE dbContactAddresses.ContID = @ContID and dbContactAddresses.contActive = @Active and dbAddress.addid = coalesce(@addID, (select contdefaultaddress from dbcontact where contid = @contid))
		ORDER BY dbContactAddresses.contOrder
End
Else
Begin
	SELECT dbContactAddresses.contaddID, dbContactAddresses.contID, dbContactAddresses.contActive, dbContactAddresses.contOrder,  dbo.GetCodeLookupDesc('INFOADDTYPE', dbContactAddresses.contCode, @UI) as ContTypeDesc,  dbContactAddresses.contCode, dbAddress.*
		FROM  dbContactAddresses INNER JOIN
		               dbAddress ON dbContactAddresses.contaddID = dbAddress.addID
		WHERE dbContactAddresses.ContID = @ContID and dbAddress.addID  = coalesce(@addID, (select contdefaultaddress from dbcontact where contid = @contid))
		ORDER BY dbContactAddresses.contOrder
End

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprContactDefaultAddress] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprContactDefaultAddress] TO [OMSAdminRole]
    AS [dbo];

