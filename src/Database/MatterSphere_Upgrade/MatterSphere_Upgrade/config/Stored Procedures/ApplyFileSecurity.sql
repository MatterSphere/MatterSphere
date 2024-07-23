CREATE PROCEDURE [config].[ApplyFileSecurity]
	@policyID uniqueidentifier ,
	@userGroupID uniqueidentifier ,
	@securableTypeID bigint ,
	@adminUserID int


AS
SET NOCOUNT ON
SET ANSI_WARNINGS OFF

-- security level check
IF (SELECT [config].[GetSecurityLevel] () & 256 ) = 0 
	RETURN

DECLARE @docTable table ( DocID bigint )
DECLARE @Inherited char(1)
SET @Inherited = null
DECLARE @CLID bigint
select @clid = clid from config.dbFile where fileid = @securableTypeID

DECLARE @user nvarchar(200)
select @user = NTLogin from item.[user] where id = @userGroupID


BEGIN TRY
	BEGIN TRANSACTION
	-- Create the file security record
	IF ( SELECT [config].[GetSecurityLevel] () & 256 ) = 256
	BEGIN
		IF exists (select 1 from relationship.UserGroup_Client where ClientID = @CLID)
		and not exists (select 1 from relationship.UserGroup_Client where ClientID = @CLID and (usergroupid = (select id from [item].[group] where name = 'ALLINTERNAL') or UserGroupID = @userGroupID))
			begin
			
			DECLARE @Special UNIQUEIDENTIFIER = (SELECT o.ID FROM config.ObjectPolicy o WHERE o.Type = 'LIMITEDFILEDEF');
			IF @Special IS NULL SET @Special = @policyID;

			INSERT [relationship].[UserGroup_Client] ( [UserGroupID] , [ClientID] , [PolicyID], [block_inheritance] )
			VALUES ( @userGroupID , @CLID , @Special, 1 )

			-- Audit the security event
			INSERT [audit].[UserGroup_Client] ( [Created] , [CreatedBy] , [Event] ,  [UserGroupID] , [ClientID] , [PolicyID] )
			VALUES ( GetUTCDate() , @adminUserID , 'NEWSECFILE' , @userGroupID , @clID , @Special )

			INSERT [relationship].[UserGroup_Contact] ( [UserGroupID] , [ContactID] , [PolicyID], [clid], inherited)
			SELECT @userGroupID, contid , @Special, @CLID, 'C' from dbo.dbClientContacts where clid = @CLID

			-- Audit the security event
			INSERT [audit].[UserGroup_Contact] ( [Created] , [CreatedBy] , [Event] , [UserGroupID] , [ContactID] , [PolicyID])
			SELECT GetUTCDate() , @adminUserID , 'NEWSECFILE' ,@userGroupID, contid, @Special from dbo.dbClientContacts where clid = @CLID
		   end		   

		If exists (select 1 from [relationship].[UserGroup_File] where FileID = @securableTypeID and UserGroupID = (select ID from [item].[Group] where name = 'ALLINTERNAL'))
		begin
			delete from [relationship].[UserGroup_File] where FileID = @securableTypeID and UserGroupID = (select ID from [item].[Group] where name = 'ALLINTERNAL')
			delete from [relationship].[UserGroup_Document] where FileID = @securableTypeID and UserGroupID = (select ID from [item].[Group] where name = 'ALLINTERNAL')
		end

		IF exists (select 1 from relationship.UserGroup_Client where ClientID = @CLID and UserGroupID = @userGroupID and block_inheritance is null) 
		begin

			set @Inherited = 'C'

		end

		INSERT [relationship].[UserGroup_File] ( [UserGroupID] , [FileID] , [PolicyID], [Clid] ,[inherited])
		VALUES ( @userGroupID , @securableTypeID , @policyID, @Clid, @Inherited )


		-- Audit the security event
		INSERT [audit].[UserGroup_File] ( [Created] , [CreatedBy] , [Event] ,  [UserGroupID] , [FileID] , [PolicyID] )
		VALUES ( GetUTCDate() , @adminUserID , 'NEWSECFILE' , @userGroupID , @securableTypeID , @policyID )

	END
	

	-- Now secure all documents under the file if they do not have an existing policy assigned to that user
	IF exists (SELECT  1 FROM [dbo].[dbRegInfo] where [regBlockInheritence] = 0) 
		AND exists (select 1 where (SELECT [config].[GetSecurityLevel] () & 128) = 128)
	BEGIN
		INSERT [relationship].[UserGroup_Document] ( [UserGroupID] , [DocumentID] , [PolicyID], [CLID], [FileID] , [Inherited] )
		OUTPUT Inserted.DocumentID INTO @docTable
		SELECT @userGroupID , D.DocID , @policyID, d.CLID, d.FileID , CASE WHEN @Inherited  = 'C' then 'C' else 'F' end FROM [config].[dbDocument] D LEFT JOIN [relationship].[UserGroup_Document] UGD ON UGD.[DocumentID] = D.[DocID] AND UGD.[UserGroupID] = @userGroupID
		WHERE D.[fileID] = @securableTypeID AND UGD.[DocumentID] IS NULL

		-- Audit the security event
		INSERT [audit].[UserGroup_Document] ( [Created] , [CreatedBy] , [Event] ,  [UserGroupID] , [DocumentID] , [PolicyID] )
		SELECT GetUTCDate() , @adminUserID , 'NEWSECDOC' , @userGroupID , DocID , @policyID FROM [dbo].[dbDocument] WHERE [fileID] = @securableTypeID
	END
		
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	IF @@Trancount <> 0
		ROLLBACK TRANSACTION
	DECLARE @er nvarchar(max)
	SET @er = ERROR_MESSAGE()
	RAISERROR ( @er , 16 ,1 )
END CATCH



