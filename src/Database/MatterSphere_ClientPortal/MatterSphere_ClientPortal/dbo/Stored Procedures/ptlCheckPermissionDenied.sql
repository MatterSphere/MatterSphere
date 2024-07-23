CREATE PROCEDURE [dbo].[ptlCheckPermissionDenied]
	@permType nvarchar(30),
	@objectId bigint
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @user nvarchar(128);
	SET @user = config.GetUserLogin()

	IF @permType = 'CONTACT_UPDATE'
		SELECT @permType AS Permission, CAST(CASE WHEN EXISTS (SELECT 1 FROM config.CheckContactAccess(@user, @objectId) WHERE UDeny = 1) THEN 1 ELSE 0 END AS bit) AS Denied
	ELSE IF @permType = 'CLIENT_UPDATE'
		SELECT @permType AS Permission, CAST(CASE WHEN EXISTS (SELECT 1 FROM config.CheckClientAccess(@user, @objectId) WHERE UDeny = 1) THEN 1 ELSE 0 END AS bit) AS Denied
	IF @permType = 'FILE_UPDATE'
		SELECT @permType AS Permission, CAST(CASE WHEN EXISTS (SELECT 1 FROM config.CheckFileAccess(@user, @objectId) WHERE UDeny = 1) THEN 1 ELSE 0 END AS bit) AS Denied
	ELSE IF @permType = 'FILE_SAVEDOC'
		SELECT @permType AS Permission, CAST(CASE WHEN EXISTS (SELECT 1 FROM config.CheckFileSaveDocAccess(@user, @objectId) WHERE UDeny = 1) THEN 1 ELSE 0 END AS bit) AS Denied
	ELSE IF @permType = 'DOCUMENT_UPDATE'
		SELECT @permType AS Permission, CAST(CASE WHEN EXISTS (SELECT 1 FROM config.CheckDocumentAccess(@user, @objectId) WHERE UDeny = 1) THEN 1 ELSE 0 END AS bit) AS Denied
	ELSE
		SELECT CAST(NULL AS nvarchar(30)) AS Permission, CAST(NULL AS bit) AS Denied
END
