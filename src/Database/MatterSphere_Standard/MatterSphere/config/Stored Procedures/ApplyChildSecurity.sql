

CREATE PROCEDURE [config].[ApplyChildSecurity]
	@parentType uCodeLookup ,
	@adminUserID int ,
	@parentID bigint ,
	@childID bigint


AS
SET NOCOUNT ON

-- security level check
IF (SELECT [config].[GetSecurityLevel] () & 384 ) = 0 
	RETURN

DECLARE @securityTable table
	(
		PolicyID uniqueidentifier ,
		UserGroupID uniqueidentifier,
		Clid bigint
	)



IF @parentType = 'CLIENT' and ( SELECT [config].[GetSecurityLevel] () & 256 ) = 256
BEGIN
	IF NOT EXISTS ( SELECT PolicyID FROM [relationship].[UserGroup_Client] WHERE [ClientID] = @parentID )
		RETURN
	ELSE
	BEGIN
		IF EXISTS ( SELECT PolicyID FROM [relationship].[UserGroup_File] WHERE [FileID] = @childID )
			RETURN
		 --Get a list of the current policies inforce on the parent object
		INSERT @securityTable ( UserGroupID , PolicyID )
		SELECT UserGroupID , PolicyID FROM [relationship].[UserGroup_Client] WHERE [ClientID] = @parentID AND isnull(block_inheritance,0) = 0 
		 --Now Create the security for the child object
		INSERT [relationship].[UserGroup_File] ( [UserGroupID] , [FileID] , [PolicyID], [CLID], [Inherited] )
		SELECT UserGroupID , @childID , PolicyID , @parentID , 'C' FROM @securityTable
		 --Now Audit the event
		INSERT [audit].[UserGroup_File] ( [Created] , [CreatedBy] , [Event] ,  [UserGroupID] , [FileID] , [PolicyID] )
		SELECT GetUTCDate() , @adminUserID , 'NEWSECFILE' , UserGroupID , @childID , PolicyID FROM @securityTable 
		 --Exit
		RETURN
	END
END



IF @parentType = 'FILE' and (SELECT  top 1 [regBlockInheritence] FROM [dbo].[dbRegInfo]) = 0 AND ( SELECT [config].[GetSecurityLevel] () & 128 ) = 128
BEGIN
	IF NOT EXISTS ( SELECT PolicyID FROM [relationship].[UserGroup_File] WHERE [FileID] = @parentID )
		RETURN
	ELSE
	BEGIN
		IF EXISTS ( SELECT PolicyID FROM [relationship].[UserGroup_Document] WHERE [DocumentID] = @childID )
			RETURN
		 --Get a list of the current policies inforce on the parent object
		INSERT @securityTable ( UserGroupID , PolicyID, CLID )
		SELECT UserGroupID , PolicyID , CLID FROM [relationship].[UserGroup_File] WHERE [FileID] = @parentID 
		 --Now Create the security for the child object
		INSERT [relationship].[UserGroup_Document] ( [UserGroupID] , [DocumentID] , [PolicyID], [CLID] , [FileID] , [Inherited] )
		SELECT UserGroupID , @childID , PolicyID , clid , @parentID , 'F' FROM @securityTable
		 --Now Audit the event
		INSERT [audit].[UserGroup_Document] ( [Created] , [CreatedBy] , [Event] ,  [UserGroupID] , [DocumentID] , [PolicyID] )
		SELECT GetUTCDate() , @adminUserID , 'NEWSECDOC' , UserGroupID , @childID , PolicyID FROM @securityTable 
		 --Exit
		RETURN
	END
END



GO
GRANT EXECUTE
    ON OBJECT::[config].[ApplyChildSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ApplyChildSecurity] TO [OMSAdminRole]
    AS [dbo];

