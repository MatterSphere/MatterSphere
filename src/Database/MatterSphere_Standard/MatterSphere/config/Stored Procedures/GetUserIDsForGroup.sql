


CREATE PROCEDURE [config].[GetUserIDsForGroup]

	@groupID uniqueidentifier ,
	@adminUserID int

AS
SET NOCOUNT ON

BEGIN TRY
	BEGIN TRANSACTION
		
		DECLARE @usrID nvarchar(max)
		SET @USRID = ''

		-- Build up list of MatterSphere User IDs
		SELECT 
			@usrID = @USRID + convert(nvarchar(max), isnull(usrID, '')) + convert(nvarchar(max),';') 
		FROM
			relationship.Group_User r
			INNER JOIN item.[User] i on i.ID = r.UserID
			INNER JOIN dbUser u on u.usrADID = i.NTLogin
		WHERE 
			GroupID = @groupID

		-- Trim ending separator
		IF LEN(@usrID) > 0
		BEGIN
			SET @usrID = ISNULL( LEFT(@usrID, LEN(@usrID) - 1), '')
		END

		SELECT @usrID as UserIDs
		
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	IF @@Trancount <> 0
		ROLLBACK TRANSACTION
	DECLARE @er nvarchar(max)
	SET @er = ERROR_MESSAGE()
	RAISERROR ( @er , 16 ,1 )
END CATCH


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetUserIDsForGroup] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetUserIDsForGroup] TO [OMSAdminRole]
    AS [dbo];

