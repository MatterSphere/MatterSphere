

CREATE PROCEDURE [config].[ResetContactSecurity]
	@securableID bigint

AS
SET NOCOUNT ON 
--DECLARE @contactID bigint
--SET @contactID = ( SELECT TOP 1ContactID FROM [relationship].[UserGroup_Contact] WHERE UserGroupID = @userGroup )
DELETE [relationship].[UserGroup_Contact] WHERE ContactID = @securableID



GO
GRANT EXECUTE
    ON OBJECT::[config].[ResetContactSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ResetContactSecurity] TO [OMSAdminRole]
    AS [dbo];

