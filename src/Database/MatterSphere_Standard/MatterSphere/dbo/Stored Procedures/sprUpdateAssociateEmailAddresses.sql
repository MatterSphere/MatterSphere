

CREATE PROCEDURE [dbo].[sprUpdateAssociateEmailAddresses]
(
	@emailAddress varchar(100), 
	@assocIDs varchar(1000)
)
AS

update dbAssociates set assocEmail = @emailAddress where assocID IN (select ID from dbo.SplitIDs(@assocIDs, ','));

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprUpdateAssociateEmailAddresses] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprUpdateAssociateEmailAddresses] TO [OMSAdminRole]
    AS [dbo];

