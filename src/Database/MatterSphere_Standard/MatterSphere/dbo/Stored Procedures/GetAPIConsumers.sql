

CREATE PROCEDURE [dbo].[GetAPIConsumers]
AS
SET NOCOUNT ON

SELECT 
	apiGUID as [ID],
	apiAuthor as [CompanyName],
	apiCompanyID as [CompanyID],
	apiCode as [Name],
	apiDesc	as [Description],	
	apiPublicKeyToken as [PublicKeyToken],
	apiUIType as [ConsumerTarget],
	apiConsumerType as [ConsumerType],
	apiDefaultPriority as [DefaultPriority],
	apiMaximumPriority as [MaximumPriority],
	apiValidFrom as [ValidFrom],
	apiExpires as [Expires],
	apiRegistered as [Enabled]
FROM dbapi

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAPIConsumers] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAPIConsumers] TO [OMSAdminRole]
    AS [dbo];

