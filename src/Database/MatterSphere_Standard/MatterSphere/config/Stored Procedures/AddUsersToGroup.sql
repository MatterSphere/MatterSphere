

CREATE PROCEDURE [config].[AddUsersToGroup]
	@userIDs nvarchar(max) ,
	@groupID uniqueidentifier ,
	@adminUserID int

AS
SET NOCOUNT ON

BEGIN TRY
	BEGIN TRANSACTION
		
		-- Remove users from group
		DELETE FROM relationship.Group_User WHERE GroupID = @groupID		
		
		-- Add new users to group
		INSERT INTO relationship.Group_User (UserID, GroupID)
		(
			SELECT 
				IU.ID
				,@groupID
			FROM 
				dbo.SplitStringToTable(@userIDs, ';') S
				INNER JOIN dbUser U on U.usrID = S.items
				INNER JOIN item.[User] IU on IU.NTLogin = U.usrADID
		)

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
    ON OBJECT::[config].[AddUsersToGroup] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[AddUsersToGroup] TO [OMSAdminRole]
    AS [dbo];

