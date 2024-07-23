

CREATE PROCEDURE [config].[ApplyObjectPolicyToAllExistingMatterTypes]  
	@fileID bigint = NULL ,
	@acknowledge bit = 0

AS
SET NOCOUNT ON

-- security level check
IF ( SELECT [config].[GetSecurityLevel]() & 256 ) = 0
	RETURN
	
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
DECLARE @adminUserID int , @count bigint

IF @fileID IS NULL
BEGIN
	RAISERROR('No fileID was supplied' , 10 , 1)
	RETURN
END

IF @acknowledge = 0
BEGIN
	RAISERROR('This procedure may result in a long running transaction and as such should be executed with this in mind. To acknowledge your understanding of this please execute the procedure with @acknowledge = 1' , 10 , 1)
	RETURN
END

SET @adminUserID = (SELECT usrID FROM dbo.dbUser WHERE usrADID = Suser_Sname())
IF @adminUserID IS NULL
BEGIN
	RAISERROR('This proceure needs to be executed by a user that is mapped in Matter Centre' , 10 , 1)
	RETURN
END


DECLARE @table table (policyID uniqueidentifier , userGroupID uniqueidentifier) 
DECLARE @matterType uCodeLookup , @policyID uniqueidentifier , @userGroupID uniqueidentifier

SET @matterType = (SELECT fileType FROM dbo.dbFile WHERE fileID = @fileID)

INSERT @table 
SELECT UGF.PolicyID , UGF.UserGroupID FROM config.dbFile F JOIN relationship.UserGroup_File UGF ON F.fileID = UGF.fileID
WHERE F.fileID = @fileID GROUP BY UGF.PolicyID , UGF.UserGroupID
DECLARE @tempMatters table (fileID bigint , policyID uniqueidentifier , userGroupID uniqueidentifier) 
INSERT @tempMatters ( fileID , policyID , userGroupID )

SELECT X.fileID , T.policyID , X.userGroupID FROM
(
	SELECT F.fileID , K.UserGroupID FROM config.dbfile F CROSS JOIN ( SELECT UserGroupID FROM @table GROUP BY UserGroupID ) K WHERE F.filetype = @matterType
	EXCEPT
	SELECT FileID , UserGroupID FROM relationship.UserGroup_File 
) X
JOIN
	@table T ON T.UserGroupID = X.UserGroupID

SET @count = ( SELECT Count(*) FROM @tempMatters)
IF @count = 0
BEGIN
	RAISERROR('No security policies will be applied using this fileID' , 10 , 1)
	RETURN
END

SET @count = @count/(SELECT Count(*) FROM @table)
BEGIN TRY
	BEGIN TRANSACTION
	WHILE (SELECT Count(*) FROM @tempMatters) > 0
	BEGIN
		SELECT TOP 1 @fileID = FileID , @userGroupID = UserGroupID , @policyID = PolicyID FROM @tempMatters
		EXECUTE [config].[ApplyFileSecurity]@policyID , @userGroupID , @fileID , @adminUserID 
		DELETE @tempMatters where fileID = @fileID
	END
	COMMIT TRANSACTION
	PRINT Convert(varchar(20), @count) + ' matter records successfully secured.'
END TRY

BEGIN CATCH
	IF @@trancount <> 0
		ROLLBACK TRANSACTION
	DECLARE @errMessage varchar(max)
	SET @errMessage = (SELECT ERROR_MESSAGE())
	RAISERROR(@errMessage , 16, 1 )
END CATCH



GO
GRANT EXECUTE
    ON OBJECT::[config].[ApplyObjectPolicyToAllExistingMatterTypes] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ApplyObjectPolicyToAllExistingMatterTypes] TO [OMSAdminRole]
    AS [dbo];

