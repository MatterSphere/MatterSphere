

CREATE PROCEDURE [dbo].[ADSyncUsers] 
	@userXML xml

AS
SET NOCOUNT ON

DECLARE @userTable table ( userName nvarchar(50) , userNTName nvarchar(200) , active bit )
DECLARE @docHandle int

EXEC sp_xml_preparedocument @docHandle OUTPUT, @userXML
INSERT @userTable ( userName , userNTName, active )

-- Get a temporary store of users
SELECT userName , userNTName , CASE WHEN active & 2 = 2 THEN 0 ELSE 1 END as UserActive FROM OPENXML(@docHandle, 'DocumentElement/users/userName')
WITH (userName nvarchar(50) '../userName' , active nvarchar(20) '../active' , userNTName nvarchar(200) '../userNTName')
EXEC sp_xml_removedocument @docHandle 

-- Now add the new users
BEGIN TRY
BEGIN TRANSACTION
	-- Now synchronise the user accounts
	UPDATE U
	SET U.active = I.active
	FROM 
	[item].[User] U JOIN ( SELECT userNTName , Active FROM @userTable EXCEPT SELECT NTLogin , Active FROM [item].[User] ) I  ON I.userNTName = U.NTLogin	
COMMIT TRANSACTION
END TRY

BEGIN CATCH
	IF @@TranCount <> 0
		ROLLBACK TRANSACTION
	DECLARE @err nvarchar(Max)
	SET @err = ERROR_MESSAGE()
	RAISERROR ( @err , 16 , 1)	
END CATCH



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ADSyncUsers] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ADSyncUsers] TO [OMSAdminRole]
    AS [dbo];

