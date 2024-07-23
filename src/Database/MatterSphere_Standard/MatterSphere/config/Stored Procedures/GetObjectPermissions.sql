

CREATE PROCEDURE [config].[GetObjectPermissions]
	@securableType uCodeLookup , 
	@objectID bigint , 
	@children bit,
	@status bit = 0

AS
SET NOCOUNT ON 
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @USER varchar(50)
SET @USER = config.GetUserLogin() 

IF @securableType = 'SYSTEM'
BEGIN
	exec config.GetSystemPermissions @children, @status,@User
	return
END
IF @securableType = 'CONTACT'
BEGIN
	exec config.GetContactPermissions @objectid, @children, @status,@User
	return
END
IF @securableType = 'CLIENT'
BEGIN
	exec config.GetClientPermissions @objectid, @children, @status,@User
	return
END
IF @securableType = 'FILE'
BEGIN
	exec config.GetFilePermissions @objectid, @children, @status,@User
	return
END
IF @securableType = 'DOCUMENT'
BEGIN
	exec config.GetDocumentPermissions @objectid, @children, @status,@User
	return
END



GO
GRANT EXECUTE
    ON OBJECT::[config].[GetObjectPermissions] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetObjectPermissions] TO [OMSAdminRole]
    AS [dbo];

