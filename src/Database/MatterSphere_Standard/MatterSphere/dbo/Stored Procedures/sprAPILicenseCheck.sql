

CREATE PROCEDURE [dbo].[sprAPILicenseCheck] (@ApplicationKey uniqueidentifier , @Code nvarchar(100))  AS
	select apiuitype, apidesigner, apiservice from dbAPI where apiguid = @ApplicationKey and apicode = @Code and apiregistered = 1

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAPILicenseCheck] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAPILicenseCheck] TO [OMSAdminRole]
    AS [dbo];

