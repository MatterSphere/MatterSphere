

CREATE PROCEDURE [config].[RemoveConfigurabeTypePolicy]
	@securableID varchar(15) ,
	@securableType uCodeLookup

AS
SET NOCOUNT ON

DELETE [config].[ConfigurableTypePolicy_UserGroup] WHERE SecurableTypeCode = @securableID AND SecurableType = @securableType



GO
GRANT EXECUTE
    ON OBJECT::[config].[RemoveConfigurabeTypePolicy] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[RemoveConfigurabeTypePolicy] TO [OMSAdminRole]
    AS [dbo];

