

CREATE PROCEDURE [config].[ResetSecurity]
	@securableID bigint ,
	@securableType uCodeLookup

AS
SET NOCOUNT ON

IF @securableType = 'CLIENT' 
BEGIN	
	EXECUTE [config].[ResetClientSecurity] @securableID = @securableID
	RETURN
END
	
IF @securableType = 'CONTACT' 
BEGIN	
	EXECUTE [config].[ResetContactSecurity] @securableID = @securableID
	RETURN
END

IF @securableType = 'FILE' 
BEGIN	
	EXECUTE [config].[ResetFileSecurity]  @securableID = @securableID
END

IF @securableType = 'DOCUMENT' 
BEGIN	
	EXECUTE [config].[ResetDocumentSecurity]  @securableID = @securableID
	RETURN
END



GO
GRANT EXECUTE
    ON OBJECT::[config].[ResetSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ResetSecurity] TO [OMSAdminRole]
    AS [dbo];

