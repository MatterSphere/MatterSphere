

CREATE FUNCTION [config].[CheckObjectIsSecurityEnbled] (@secObject uCodeLookup , @secID bigint )
RETURNS bit
AS
BEGIN
	DECLARE @isSecurityEnabled bit

	-- Document security
	IF @secObject = 'DOCUMENT'
	BEGIN
	IF EXISTS  ( SELECT RelationshipID FROM [relationship].[UserGroup_Document] WHERE DocumentID = @secID )
		BEGIN
			SET @isSecurityEnabled = 1
		END
		ELSE
		BEGIN
				SET @isSecurityEnabled = 0
		END
	END

	-- Matter Security
	IF @secObject = 'FILE'
	BEGIN
	IF EXISTS  ( SELECT RelationshipID FROM [relationship].[UserGroup_File] WHERE FileID = @secID )
		BEGIN
			SET @isSecurityEnabled = 1
		END
		ELSE
		BEGIN
				SET @isSecurityEnabled = 0
		END
	END

	IF @secObject = 'CLIENT'
	BEGIN
	IF EXISTS  ( SELECT RelationshipID FROM [relationship].[UserGroup_Client] WHERE ClientID = @secID )
		BEGIN
			SET @isSecurityEnabled = 1
		END
		ELSE
		BEGIN
				SET @isSecurityEnabled = 0
		END
	END

	IF @secObject = 'CONTACT'
	BEGIN
		IF EXISTS  ( SELECT RelationshipID FROM [relationship].[UserGroup_Contact] WHERE ContactID = @secID )
		BEGIN
			SET @isSecurityEnabled = 1
		END
		ELSE
		BEGIN
				SET @isSecurityEnabled = 0
		END
	END

	RETURN @isSecurityEnabled
END


GO
GRANT EXECUTE
    ON OBJECT::[config].[CheckObjectIsSecurityEnbled] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[CheckObjectIsSecurityEnbled] TO [OMSAdminRole]
    AS [dbo];

