

CREATE PROCEDURE [config].[ApplyClientSecurity]
	@policyID uniqueidentifier ,
	@userGroupID uniqueidentifier ,
	@securableTypeID bigint ,
	@adminUserID int


AS
SET NOCOUNT ON
SET ANSI_WARNINGS OFF

-- security level check
IF (SELECT [config].[GetSecurityLevel] () & 896 ) = 0 
	RETURN

DECLARE @fileTable table (fileID bigint)
DECLARE @contTable table (contID bigint)
DECLARE @docTable table (docID bigint)
DECLARE @SubPermission uniqueidentifier,@Group tinyint

BEGIN TRY
	BEGIN TRANSACTION
	IF NOT EXISTS ( SELECT 1 from [relationship].[UserGroup_Client] where [ClientID] = @securableTypeID) 
	BEGIN
		DECLARE GROUP_CURSOR CURSOR FOR 
		SELECT distinct [UserGroupID],case when g.id is not null then 0 else 1 end as [Group] from [relationship].[UserGroup_File] UGF 
					LEFT JOIN [ITEM].[Group] G ON G.ID = UGF.UserGroupID
					LEFT JOIN [item].[User] U ON U.ID = UGF.UserGroupID
			 where clID = @securableTypeID
			 and ugf.UserGroupID <> @userGroupID
			 order by [group]
		OPEN GROUP_CURSOR;
		FETCH NEXT FROM GROUP_CURSOR into @SubPermission,@Group;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT [relationship].[UserGroup_Client] ( [UserGroupID] , [ClientID] , [PolicyID], [block_inheritance] )
			VALUES ( @SubPermission , @securableTypeID , @policyID, 1 )

			-- Audit the security event
			INSERT [audit].[UserGroup_Client] ( [Created] , [CreatedBy] , [Event] ,  [UserGroupID] , [ClientID] , [PolicyID] )
			VALUES ( GetUTCDate() , @adminUserID , 'NEWSECFILE' , @SubPermission , @securableTypeID , @policyID )

			INSERT [relationship].[UserGroup_Contact] ( [UserGroupID] , [ContactID] , [PolicyID], [clid], inherited)
			SELECT @SubPermission, contid , @policyID, @securableTypeID, 'C' from dbo.dbClientContacts where clid = @securableTypeID

			-- Audit the security event
			INSERT [audit].[UserGroup_Contact] ( [Created] , [CreatedBy] , [Event] , [UserGroupID] , [ContactID] , [PolicyID])
			SELECT GetUTCDate() , @adminUserID , 'NEWSECFILE' ,@SubPermission, contid, @policyID from dbo.dbClientContacts where clid = @securableTypeID

			FETCH NEXT FROM GROUP_CURSOR into @SubPermission,@Group;
		END;
		CLOSE GROUP_CURSOR;
		DEALLOCATE GROUP_CURSOR;
		
	END	

	If exists (select 1 from [relationship].[UserGroup_Client] where ClientID = @securableTypeID and UserGroupID = (select ID from [item].[Group] where name = 'ALLINTERNAL'))
	begin
		delete from [relationship].[UserGroup_Client] where ClientID = @securableTypeID and UserGroupID = (select ID from [item].[Group] where name = 'ALLINTERNAL')
		delete from [relationship].[UserGroup_File] where ClID = @securableTypeID and UserGroupID = (select ID from [item].[Group] where name = 'ALLINTERNAL')
		delete from [relationship].[UserGroup_Document] where ClID = @securableTypeID and UserGroupID = (select ID from [item].[Group] where name = 'ALLINTERNAL')
	end


	-- Create the client security record
	IF (SELECT [config].[GetSecurityLevel] () & 512 ) = 512
	BEGIN 
		INSERT [relationship].[UserGroup_Client] ( [UserGroupID] , [ClientID] , [PolicyID] )
		VALUES ( @userGroupID , @securableTypeID , @policyID )

		-- Audit the security event
		INSERT [audit].[UserGroup_Client] ( [Created] , [CreatedBy] , [Event] , [UserGroupID] , [ClientID] , [PolicyID] )
		VALUES ( GetUTCDate() , @adminUserID , 'NEWSECCL' , @userGroupID , @securableTypeID , @policyID )
	END
	
	-- Create the contact security record if they do not have an existing policy assigned to that user
	IF ( SELECT [config].[GetSecurityLevel] () & 1024 ) = 1024
	BEGIN
		INSERT [relationship].[UserGroup_Contact] ( [UserGroupID] , [ContactID] , [PolicyID], [CLID] , [Inherited]  )
		OUTPUT Inserted.ContactID INTO @contTable
		SELECT @userGroupID , C.[ContID] , @policyID, CC.Clid, 'C' FROM [config].[dbContact] C 
		JOIN [dbo].[dbClientContacts] CC ON C.[ContID] = CC.[ContID] 
		LEFT JOIN [relationship].[UserGroup_Contact] UGC ON UGC.[ContactID] = C.[ContID] AND UGC.[UserGroupID] = @userGroupID
		WHERE CC.[ClID] = @securableTypeID AND UGC.[ContactID] IS NULL
				/********************* New Section to prevent All Internal to lower contacts where not required ***********************/
		AND NOT EXISTS (SELECT 1  FROM [relationship].[UserGroup_contact] ugc
						LEFT JOIN [item].[User] u 
							LEFT JOIN [dbo].[dbuser] dbu on dbu.usrADID = u.NTLogin
						on ugc.UserGroupID = u.ID
						LEFT JOIN [item].[Group] g on g.ID = ugc.UserGroupID
						WHERE ugc.ContactID = c.contID
						AND (g.ID is not null or dbu.AccessType = 'INTERNAL')
						AND @UserGroupID = (select ID from [item].[Group] where name = 'ALLINTERNAL'))


		-- Audit the security event
		INSERT [audit].[UserGroup_Contact] ( [Created] , [CreatedBy] , [Event] , [UserGroupID] , [ContactID] , [PolicyID] )
		SELECT GetUTCDate() , @adminUserID , 'NEWSECCONT' ,  @userGroupID , contID , @policyID FROM @contTable
	END
	
	-- Now secure all files under the client if they do not have an existing policy assigned to that user
	IF ( SELECT [config].[GetSecurityLevel] () & 256 ) = 256 
	BEGIN
		INSERT [relationship].[UserGroup_File] ( [UserGroupID] , [FileID] , [PolicyID], [CLID] , [Inherited] )
		OUTPUT Inserted.FileID INTO @fileTable
		SELECT  @userGroupID , F.[FileID] , @policyID, F.CLID , 'C' 
		FROM [config].[dbFile] F 
		LEFT JOIN [relationship].[UserGroup_File] UGF ON UGF.[FileID] = F.[FileID] 
		AND UGF.[UserGroupID] = @userGroupID
		WHERE F.[ClID] = @securableTypeID AND UGF.[FileID] IS NULL
		/********************* New Section to prevent All Internal to lower files where not required ***********************/
		AND NOT EXISTS (SELECT 1  FROM [relationship].[UserGroup_File] ugf
						LEFT JOIN [item].[User] u 
							LEFT JOIN [dbo].[dbuser] dbu on dbu.usrADID = u.NTLogin
						on ugf.UserGroupID = u.ID
						LEFT JOIN [item].[Group] g on g.ID = ugf.UserGroupID
						WHERE fileID = f.fileid
						AND (g.ID is not null or dbu.AccessType = 'INTERNAL')
						AND @UserGroupID = (select ID from [item].[Group] where name = 'ALLINTERNAL'))


		-- Audit the security event
		INSERT [audit].[UserGroup_File] (  [Created] , [CreatedBy] , [Event] , [UserGroupID] , [FileID] , [PolicyID] )
		SELECT GetUTCDate() , @adminUserID , 'NEWSECFILE' ,  @userGroupID , [FileID] , @policyID FROM @fileTable
	END
	
	-- Now secure all documents under the file if they do not have an existing policy assigned to that user
	IF exists (SELECT  1 FROM [dbo].[dbRegInfo] where [regBlockInheritence] = 0) AND exists (select 1 where (SELECT [config].[GetSecurityLevel] () & 128) = 128)
	BEGIN
		INSERT [relationship].[UserGroup_Document] ( [UserGroupID] , [DocumentID] , [PolicyID], [CLID], [FILEID], [Inherited] )
		OUTPUT Inserted.DocumentID INTO @docTable
		SELECT @userGroupID , D.DocID , @policyID, D.CLID, D.FILEID , 'C' FROM [config].[dbDocument] D LEFT JOIN [relationship].[UserGroup_Document] UGD ON UGD.[DocumentID] = D.[DocID] AND UGD.[UserGroupID] = @userGroupID
		WHERE D.[ClID] = @securableTypeID AND UGD.[DocumentID] IS NULL

		-- Audit the security event
		INSERT [audit].[UserGroup_Document] ( [Created] , [CreatedBy] , [Event] ,  [UserGroupID] , [DocumentID] , [PolicyID] )
		SELECT GetUTCDate() , @adminUserID , 'NEWSECDOC' , @userGroupID , [DocID] , @policyID FROM @docTable
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



GO
GRANT EXECUTE
    ON OBJECT::[config].[ApplyClientSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ApplyClientSecurity] TO [OMSAdminRole]
    AS [dbo];

