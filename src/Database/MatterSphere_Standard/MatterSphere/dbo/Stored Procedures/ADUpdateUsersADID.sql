CREATE PROCEDURE [dbo].[ADUpdateUsersADID]
	@sourceUserName NVARCHAR(48),
	@targetUserName NVARCHAR(48),
	@exceededTopUsersReturnCount INT = 10,
	@rowsAffected INT = 0 OUTPUT,
	@isExceededUserMaxLength BIT = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @tempUserTable TABLE(
		userADID NVARCHAR(50),
		userADIDLen INT,
		newUserADID NVARCHAR(100),
		newUserADIDLen INT
	);

	DECLARE @strFROM NVARCHAR(50), @strTO NVARCHAR(50);
	DECLARE @newUserADIDLenCount INT;

	SET @strFROM = CONCAT(@sourceUserName, N'\');
	SET @strTO = CONCAT(@targetUserName, N'\');

	IF (@strFROM = @strTO)
	BEGIN
		SET @isExceededUserMaxLength = 0;
		SET @rowsAffected = 0;
		RETURN;
	END



	INSERT INTO @tempUserTable
	SELECT usrADID,
		LEN(usrADID) AS userADIDLen,
		@strTO + SUBSTRING(usrADID, LEN(@strFROM) + 1, 50) AS newUserADID,
		LEN(@strTO + SUBSTRING(usrADID, LEN(@strFROM) + 1, 50)) AS newUserADIDLen
	FROM dbUser
	WHERE AccessType = 'INTERNAL' AND LEFT(usrADID, LEN(@strFROM)) = @strFROM;



	SET @newUserADIDLenCount = (SELECT COUNT(newUserADIDLen) FROM @tempUserTable);

	IF (@newUserADIDLenCount = 0)
	BEGIN
		SET @isExceededUserMaxLength = 0;
		SET @rowsAffected = 0;
		RETURN;
	END



	IF (SELECT COUNT(newUserADIDLen) FROM @tempUserTable WHERE newUserADIDLen > 50) > 0
	BEGIN
		SET @isExceededUserMaxLength = 1;
		SET @rowsAffected = 0;

		SELECT TOP (@exceededTopUsersReturnCount) * FROM @tempUserTable WHERE newUserADIDLen > 50;
	END
	ELSE BEGIN
		UPDATE dbo.dbUser
		SET usrADID = @strTO + SUBSTRING(usrADID, LEN(@strFROM) + 1, 50)
		WHERE AccessType = 'INTERNAL' AND LEFT(usrADID, LEN(@strFROM)) = @strFROM;

		SET @rowsAffected = @newUserADIDLenCount;
		SET @isExceededUserMaxLength = 0;
	END
END
GO




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ADUpdateUsersADID] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ADUpdateUsersADID] TO [OMSAdminRole]
    AS [dbo];