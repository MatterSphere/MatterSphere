

CREATE PROCEDURE [dbo].[sprContactAddresses] (@ContID bigint, @Active smallint =-1, @UI uUICultureInfo = '{default}')  AS
--Get Addresses order by ContOrder where ContType=Active
	if @Active <> -1 
Begin
	SELECT dbContactAddresses.contaddID, dbContactAddresses.contID, dbContactAddresses.contActive, dbContactAddresses.contOrder,  dbo.GetCodeLookupDesc('INFOADDTYPE', dbContactAddresses.contCode, @UI) as ContTypeDesc,  dbContactAddresses.contCode, dbAddress.*
		FROM  dbContactAddresses INNER JOIN
		               dbAddress ON dbContactAddresses.contaddID = dbAddress.addID
		WHERE dbContactAddresses.ContID = @ContID and dbContactAddresses.contActive = @Active
		ORDER BY dbContactAddresses.contOrder
End
Else
Begin
	SELECT dbContactAddresses.contaddID, dbContactAddresses.contID, dbContactAddresses.contActive, dbContactAddresses.contOrder,  dbo.GetCodeLookupDesc('INFOADDTYPE', dbContactAddresses.contCode, @UI) as ContTypeDesc,  dbContactAddresses.contCode, dbAddress.*
		FROM  dbContactAddresses INNER JOIN
		               dbAddress ON dbContactAddresses.contaddID = dbAddress.addID
		WHERE dbContactAddresses.ContID = @ContID 
		ORDER BY dbContactAddresses.contOrder
End

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprContactAddresses] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprContactAddresses] TO [OMSAdminRole]
    AS [dbo];

