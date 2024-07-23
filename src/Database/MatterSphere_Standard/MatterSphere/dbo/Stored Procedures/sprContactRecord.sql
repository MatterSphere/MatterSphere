

CREATE PROCEDURE [dbo].[sprContactRecord] (@ContID bigint, @UI uUICultureInfo = '{default}')  AS
	declare @conttypecode uCodeLookup
	declare @DefaultAddress int
	select @conttypecode = conttypecode, @DefaultAddress = contDefaultAddress from dbcontact where contid = @ContID
	-- CONTACT
	--Contact Header Information
	select * from dbcontact  where contid = @ContID
	-- ADDRESSES
	--Get Contact Addresses
	--exec sprContactAddresses @ContID, -1, @UI


		SELECT     dbContactAddresses.contaddID, dbContactAddresses.contID, dbContactAddresses.contActive, dbContactAddresses.contOrder, 
		                      dbo.GetCodeLookupDesc('INFOADDTYPE', dbContactAddresses.contCode, @UI) AS ContTypeDesc, dbContactAddresses.contCode, dbContactAddresses.rowguid, dbAddress.addID, 
		                      dbAddress.addLine1, dbAddress.addLine2, dbAddress.addLine3, dbAddress.addLine4, dbAddress.addLine5, dbAddress.addPostcode, 
		                      dbAddress.addCountry, dbAddress.addDXCode, dbAddress.Created, dbAddress.CreatedBy, dbAddress.Updated, dbAddress.UpdatedBy
		                     -- dbAddress.addCountryOld
		FROM         dbContactAddresses INNER JOIN
		                      dbAddress ON dbContactAddresses.contaddID = dbAddress.addID
		WHERE     (dbContactAddresses.contID = @ContID) -- [SEE NOTE 1 AT BOTTOM] and dbContactAddresses.contActive = 1
		UNION
		SELECT 0, @ContID, 1, -1,  dbo.GetCodeLookupDesc('RESOURCE', 'DEFAULT', @UI) , '', null as rowguid, dbAddress.addID, 
		                      dbAddress.addLine1, dbAddress.addLine2, dbAddress.addLine3, dbAddress.addLine4, dbAddress.addLine5, dbAddress.addPostcode, 
		                      dbAddress.addCountry, dbAddress.addDXCode, dbAddress.Created, dbAddress.CreatedBy, dbAddress.Updated, dbAddress.UpdatedBy
		                     -- dbAddress.addCountryOld
		from dbaddress where addid = @DefaultAddress
		ORDER BY dbContactAddresses.contOrder


	-- NUMBERS
	--Get Number Records
	SELECT *,dbo.GetCodeLookupDesc('NUMBERTYPE', dbContactNumbers.contCode, @UI) as NumberTypeDesc, dbo.GetCodeLookupDesc('INFOTYPE', dbContactNumbers.contExtraCode, @UI) + ': ' + contNumber as DisplayNumber
		FROM  [dbContactNumbers]  
		where contid = @ContID
		ORDER BY ContOrder
	-- EMAILS
	--Get Email Addresses
	SELECT *,dbo.GetCodeLookupDesc('INFOTYPE', dbContactEmails.contCode, @UI) as EmailTypeDesc, dbo.GetCodeLookupDesc('INFOTYPE', dbContactEmails.contCode, @UI) + ': ' + contEmail as DisplayEmail
		FROM  [dbContactEmails]  
		where contid = @ContID
		ORDER BY ContOrder


SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprContactRecord] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprContactRecord] TO [OMSAdminRole]
    AS [dbo];

