

CREATE FUNCTION[config].[GetUserGroupMembership] ()
RETURNS @table table ( userID uniqueidentifier, userType nvarchar(15) )
AS
BEGIN
	DECLARE @userID uniqueidentifier
	SET @userID = (SELECT Convert(nvarchar(128),context_info) FROM master.config.sysprocesses WHERE spid = @@SPID)

	INSERT @table (userID , userType)
	SELECT GroupID, NULL FROM GroupMembership WHERE UserID = @userID
	GROUP BY GroupID
	UNION
	SELECT rowguid , usrType FROM dbo.dbUser WHERE rowguid = @userID

	RETURN
END
