CREATE PROCEDURE [dbo].[ADSyncGroupUsers]
	@userXML xml ,
	@defaultPolicyID nvarchar(50) ,
	@groupID nvarchar(50)
	
	
AS
SET NOCOUNT ON
DECLARE @userTable table ( userName nvarchar(50) , userNTName nvarchar(200) , active bit , policyID nvarchar(50) , userID nvarchar(50) )
DECLARE @docHandle int

EXEC sp_xml_preparedocument @docHandle OUTPUT, @userXML
INSERT @userTable ( userName , userNTName, active , policyID , userID )

-- Get a temporary store of users
SELECT userName , userNTName , CASE WHEN active & 2 = 2 THEN 0 ELSE 1 END as UserActive , @defaultPolicyID , Convert(nvarchar(50) , newid()) FROM OPENXML(@docHandle, 'DocumentElement/users/userName')
WITH (userName nvarchar(50) '../userName' , active nvarchar(20) '../active' , userNTName nvarchar(200) '../userNTName')
EXEC sp_xml_removedocument @docHandle 


UPDATE @userTable SET userNTName = REPLACE (userNTName , '\\' , '\' )
-- Now add the new users
BEGIN TRY
BEGIN TRANSACTION
	---- Create new users
	--INSERT [item].[User] ( [ID] , [NTLogin] , [Name] , [Active] , [PolicyID] )
	--SELECT UT.userID , UT.userNTName , UT.userName , UT.Active , @defaultPolicyID 
	--FROM ( SELECT userNTName FROM @userTable EXCEPT SELECT NTLogin FROM [item].[User] ) X JOIN @userTable UT ON UT.userNTName = X.userNTName
	-- Add new users to the Group
	INSERT [relationship].[Group_User] ( GroupID , UserID )
	SELECT @groupID , Y.ID
	FROM
	(SELECT UserNTName  COLLATE DATABASE_DEFAULT as UserNTName  FROM @userTable EXCEPT SELECT NTLogin FROM [item].[User] U JOIN [relationship].[Group_User] R ON R.UserID = U.ID WHERE GroupID = @groupID ) Z
	JOIN item.[User] Y ON Z.userNTName = Y.NTLogin
	--( SELECT userID FROM @userTable EXCEPT	SELECT UserID FROM [relationship].[Group_User] WHERE GroupID = @groupID ) Z JOIN @userTable Y ON Z.userID = Y.userID
	-- Remove any users that are no longer in the group
	DELETE GU FROM [relationship].[Group_User] GU
	JOIN
	( 
		SELECT Y.ID FROM
		(
			SELECT NTLogin FROM [item].[User] U JOIN [relationship].[Group_User] R ON R.UserID = U.ID 
			JOIN dbo.dbUser dbU on dbu.usrADID = U.NTLogin  
			WHERE GroupID = @groupID 
			AND (dbU.AccessType) <> 'EXTERNAL'  -- To not remove Remote users during resync
			EXCEPT SELECT UserNTName  COLLATE DATABASE_DEFAULT as UserNTName  FROM @userTable 
		) Z JOIN [item].[User] Y ON Z.NTLogin = Y.NTLogin 
	) X ON X.ID = GU.UserID WHERE GU.GroupID = @groupID
--	DELETE [relationship].[Group_User]	FROM [relationship].[Group_User] GU JOIN ( SELECT UserID FROM [relationship].[Group_User] WHERE GroupID = @groupID EXCEPT SELECT userID FROM @userTable ) G ON G.userID = GU.userID
COMMIT TRANSACTION
END TRY

BEGIN CATCH
	IF @@TranCount <> 0
		ROLLBACK TRANSACTION
	DECLARE @err nvarchar(Max)
	SET @err = ERROR_MESSAGE()
	RAISERROR ( @err , 16 , 1)	
END CATCH