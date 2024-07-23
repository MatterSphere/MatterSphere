

CREATE PROCEDURE [config].[ResetDocumentSecurity]
	@securableID bigint

AS
SET NOCOUNT ON 
--DECLARE @documentID bigint
--SET @documentID = ( SELECT TOP 1 DocumentID FROM [relationship].[UserGroup_Document] WHERE UserGroupID = @userGroup )
DELETE [relationship].[UserGroup_Document] WHERE DocumentID = @securableID



GO
GRANT EXECUTE
    ON OBJECT::[config].[ResetDocumentSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ResetDocumentSecurity] TO [OMSAdminRole]
    AS [dbo];

