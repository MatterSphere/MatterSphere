

CREATE FUNCTION [dbo].[GetAssocAddressTypes] (@AddID Bigint, @ContID BIGINT, @UI uUICultureInfo = '{default}')

RETURNS NVARCHAR(MAX)

AS

BEGIN
		DECLARE @AddressList VARCHAR(8000)
		SELECT @AddressList = ''
		SELECT @AddressList = @AddressList + dbo.GetCodeLookupDesc('INFOADDTYPE', ContCode, @UI) +', '
		FROM dbContactAddresses
		WHERE contaddID = @AddID AND contID = @ContID
		ORDER BY ContCode --yes we can sort
RETURN LEFT(@AddressList,(LEN(@AddressList) -1))

END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAssocAddressTypes] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAssocAddressTypes] TO [OMSAdminRole]
    AS [dbo];

