



CREATE PROCEDURE [config].[DeleteUserGroupFromObject]
	@userGroupID uniqueidentifier,
	@securableType uCodelookup,
	@securableID nvarchar(15),
	@parentSecurableID bigint = null,
	@adminUserID bigint,
	@BlockInheritence bit = 1
	

AS
SET NOCOUNT ON
SET ANSI_WARNINGS OFF
	DECLARE @err nvarchar(2000) , @policyID uniqueidentifier
	
	DECLARE @policyTable table ( newpolicyID uniqueidentifier, oldpolicyID uniqueidentifier )
	DECLARE @PermsRestored bit
	DECLARE @UserGroup uniqueidentifier
	DECLARE @User nvarchar(200)
	DECLARE @T1 Table (Fileid bigint)

SELECT @user = NTLogin from [item].[User] where id = @userGroupID

IF @securableType IN ( 'clientType' , 'FileType' , 'DocumentType' , 'ContactType' )
BEGIN
	DELETE [config].[ConfigurableTypePolicy_UserGroup]
	WHERE UserGroupID = @userGroupID AND SecurableType = @securableType AND SecurableTypeCode = @securableID
	RETURN
END

IF @securableType = 'CLIENT'
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
		
	
			DELETE UGCO 
			FROM relationship.UserGroup_Contact UGCO
			JOIN dbo.dbClientContacts CC ON CC.ContID = UGCO.ContactID
			JOIN relationship.UserGroup_Client UGC ON UGC.ClientID = CC.ClID
			WHERE UGCO.UserGroupID = @userGroupID AND UGC.ClientID = @securableID;

			DELETE [relationship].[UserGroup_Client] WHERE UserGroupID = @userGroupID AND ClientID = @securableID;
			
			DELETE UGF 
			FROM relationship.UserGroup_File UGF
			JOIN dbo.dbFile F ON F.fileID = UGF.fileID
			WHERE UGF.UserGroupID = @userGroupID AND F.ClID = @securableID ;

			IF EXISTS (SELECT 1 FROM relationship.UserGroup_Client UGC
							LEFT JOIN [item].[User] IU 
								JOIN [dbo].[dbUser] DU on IU.NTLogin = DU.usrADID AND DU.AccessType = 'INTERNAL'
							ON IU.ID = UGC.UserGroupID  
							LEFT JOIN [item].[Group] IG 
							ON IG.ID = UGC.UserGroupID
						WHERE UGC.ClientID = @securableID
						AND (DU.usrID is not null or IG.ID is not null))
			BEGIN
				INSERT INTO @T1
				SELECT DISTINCT F.FileID FROM relationship.UserGroup_File UGF
							JOIN dbo.dbFile F ON F.fileID = UGF.fileID
							WHERE UGF.UserGroupID <> @userGroupID 
							AND F.ClID = @securableID

				DELETE T
						 FROM relationship.UserGroup_File UGF1
											JOIN [item].[User] IU 
												JOIN [dbo].[dbUser] DU on IU.NTLogin = DU.usrADID and DU.AccessType = 'INTERNAL'
											ON IU.ID = UGF1.UserGroupID
											JOIN @T1 T on T.Fileid = UGF1.FileID

				DELETE T FROM relationship.UserGroup_File UGF1
											JOIN [item].[Group] IG 
												ON IG.ID = UGF1.UserGroupID
											JOIN @T1 T on T.Fileid = UGF1.FileID

				INSERT into [relationship].[UserGroup_File]	
				SELECT newid()
						,[UserGroupID]
						,T.Fileid
						,[PolicyID]
						,[ClientID]
						,'C'
					FROM [relationship].[UserGroup_Client] UGC
					JOIN [config].[dbFile] dbf on dbf.clID = UGC.ClientID
					JOIN @T1 T ON T.Fileid = dbf.fileID
					WHERE ClientID = @securableID 
					AND UserGroupID = (SELECT ID FROM [item].[group] where [name] = 'ALLINTERNAL')

			END
			
			DELETE UGD 
			FROM relationship.UserGroup_Document UGD
			JOIN dbo.dbDocument D ON D.DocID = UGD.DocumentID
			WHERE UGD.UserGroupID = @userGroupID AND D.ClID = @securableID ;

			/* Create any group on matters not defined at client - Group first to ensure any user in group not created seperately */
				DECLARE GROUP_CURSOR CURSOR FOR 
					select distinct UserGroupID,op.ID
					from relationship.UserGroup_File F
					JOIN [item].[group] g on g.ID = f.UserGroupID
					join config.ObjectPolicy op on [op].[type] = 'GLOBALOBJDEF'
					where clid = @securableID
					and inherited is null
					and not exists (
					select 1 
					from relationship.UserGroup_client C
					join [item].[group] g on g.ID = C.UserGroupID
					join [relationship].[Group_User] gu on gu.GroupID = g.ID
					where clientid = f.clid and gu.UserID = f.UserGroupID)
					and not exists (
					select 1 
					from relationship.UserGroup_client C
					where clientid = f.clid and c.UserGroupID = f.UserGroupID)
				OPEN GROUP_CURSOR;
				FETCH NEXT FROM GROUP_CURSOR into @UserGroup,@policyid;
				WHILE @@FETCH_STATUS = 0
				BEGIN
					INSERT [relationship].[UserGroup_Client] ( [UserGroupID] , [ClientID] , [PolicyID], [block_inheritance] )
					VALUES ( @UserGroup , @securableID , @policyID, 1 )

					-- Audit the security event
					INSERT [audit].[UserGroup_Client] ( [Created] , [CreatedBy] , [Event] ,  [UserGroupID] , [ClientID] , [PolicyID] )
					VALUES ( GetUTCDate() , @adminUserID , 'NEWSECFILE' , @UserGroup , @securableID , @policyID )

					INSERT [relationship].[UserGroup_Contact] ( [UserGroupID] , [ContactID] , [PolicyID], [clid], inherited)
					SELECT @UserGroup, contid , @policyID, @securableID, 'C' from dbo.dbClientContacts where clid = @securableID

					-- Audit the security event
					INSERT [audit].[UserGroup_Contact] ( [Created] , [CreatedBy] , [Event] , [UserGroupID] , [ContactID] , [PolicyID])
					SELECT GetUTCDate() , @adminUserID , 'NEWSECFILE' ,@UserGroup, contid, @policyID from dbo.dbClientContacts where clid = @securableID

					FETCH NEXT FROM GROUP_CURSOR into @UserGroup,@PolicyID;
				END;
				CLOSE GROUP_CURSOR;
				DEALLOCATE GROUP_CURSOR;

			/* Create any group on matters not defined at client - Group first to ensure any user in group not created seperately */

				DECLARE USER_CURSOR CURSOR FOR 
					select distinct UserGroupID,op.ID
					from relationship.UserGroup_File F
					JOIN [item].[user] u on u.ID = f.UserGroupID
					join config.ObjectPolicy op on [op].[type] = 'GLOBALOBJDEF'
					where clid = @securableID
					and inherited is null
					--and not exists (
					--select 1 
					--from relationship.UserGroup_client C
					--join [item].[group] g on g.ID = C.UserGroupID
					--join [relationship].[Group_User] gu on gu.GroupID = g.ID
					--where clientid = f.clid and gu.UserID = f.UserGroupID)
					and not exists (
					select 1 
					from relationship.UserGroup_client C
					where clientid = f.clid and c.UserGroupID = f.UserGroupID)
				OPEN USER_CURSOR;
				FETCH NEXT FROM USER_CURSOR into @UserGroup,@policyid;
				WHILE @@FETCH_STATUS = 0
				BEGIN
					INSERT [relationship].[UserGroup_Client] ( [UserGroupID] , [ClientID] , [PolicyID], [block_inheritance] )
					VALUES ( @UserGroup , @securableID , @policyID, 1 )

					-- Audit the security event
					INSERT [audit].[UserGroup_Client] ( [Created] , [CreatedBy] , [Event] ,  [UserGroupID] , [ClientID] , [PolicyID] )
					VALUES ( GetUTCDate() , @adminUserID , 'NEWSECFILE' , @UserGroup , @securableID , @policyID )

					INSERT [relationship].[UserGroup_Contact] ( [UserGroupID] , [ContactID] , [PolicyID], [clid], inherited)
					SELECT @UserGroup, contid , @policyID, @securableID, 'C' from dbo.dbClientContacts where clid = @securableID

					-- Audit the security event
					INSERT [audit].[UserGroup_Contact] ( [Created] , [CreatedBy] , [Event] , [UserGroupID] , [ContactID] , [PolicyID])
					SELECT GetUTCDate() , @adminUserID , 'NEWSECFILE' ,@UserGroup, contid, @policyID from dbo.dbClientContacts where clid = @securableID

					FETCH NEXT FROM USER_CURSOR into @UserGroup,@PolicyID;
				END;
				CLOSE USER_CURSOR;
				DEALLOCATE USER_CURSOR;
			
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TranCount <> 0
			ROLLBACK TRANSACTION
		SET @err = ( SELECT ERROR_MESSAGE() )
		RAISERROR (@err , 16 , 1)
	END CATCH
--RAISERROR ( 'PERMRESTORE' , 16 , 1)
RETURN
END

IF @securableType = 'CONTACT'
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			DELETE [relationship].[UserGroup_Contact] WHERE UserGroupID = @userGroupID AND ContactID = @securableID
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TranCount <> 0
			ROLLBACK TRANSACTION
		SET @err = ( SELECT ERROR_MESSAGE() )
		RAISERROR (@err , 16 , 1)
	END CATCH
RETURN
END


IF @securableType = 'FILE'
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		
		IF EXISTS (SELECT 1 FROM relationship.UserGroup_Client C WHERE C.ClientID = ( SELECT clid from config.dbFile where FileID = @securableID) AND UserGroupID = @userGroupID AND isnull(block_inheritance,0) = 0)
		BEGIN
		UPDATE F SET PolicyID = C.PolicyID,F.Inherited = 'C'
		FROM relationship.UserGroup_File F 
		JOIN relationship.UserGroup_Client C ON C.ClientID = F.ClID AND C.UserGroupID = F.UserGroupID 
		WHERE F.UserGroupID = @userGroupID AND F.FileID = @securableID

		UPDATE D SET PolicyID = D.PolicyID,D.Inherited = 'C'
		FROM relationship.UserGroup_Document D 
		JOIN relationship.UserGroup_Client C ON C.ClientID = D.ClID AND C.UserGroupID = D.UserGroupID 
		WHERE D.UserGroupID = @userGroupID AND D.DocumentID = @securableID AND D.Inherited = 'F'

			--SET @PermsRestored = 1
		END
		ELSE 
		BEGIN
			DELETE [relationship].[UserGroup_File] WHERE UserGroupID = @userGroupID AND FileID = @securableID  AND Inherited is NULL
				
			DELETE UGD 
			FROM relationship.UserGroup_Document UGD
			JOIN dbo.dbDocument D ON D.DocID = UGD.DocumentID
			WHERE UGD.UserGroupID = @userGroupID AND D.FileID = @securableID  AND Inherited ='F'

			IF NOT EXISTS (SELECT 1 FROM relationship.UserGroup_File F WHERE UserGroupID = @userGroupID AND ClID = ( SELECT clid from config.dbFile where FileID = @securableID))
			BEGIN
				DELETE
				FROM relationship.UserGroup_Client 
				WHERE UserGroupID = @userGroupID AND ClientID = ( SELECT clid from config.dbFile where FileID = @securableID) AND block_inheritance = 1
				DELETE
				FROM relationship.UserGroup_Contact 
				WHERE UserGroupID = @userGroupID AND ClID = ( SELECT clid from config.dbFile where FileID = @securableID) 
			END

		END

		IF NOT EXISTS (SELECT 1 FROM relationship.UserGroup_File F where FileID = @securableID AND UserGroupID not in 
										(SELECT [ID] FROM [item].[User] U JOIN [dbo].[dbUser] DU on U.NTLogin = DU.usrADID AND DU.AccessType = 'External'))
		BEGIN

			INSERT into [relationship].[UserGroup_File]	
			SELECT newid()
					,[UserGroupID]
					,@securableID
					,[PolicyID]
					,[ClientID]
					,'C'
				FROM [relationship].[UserGroup_Client]
				WHERE ClientID = ( SELECT clid from [config].[dbFile] where [FileID] = @securableID) 
				AND UserGroupID = (SELECT ID FROM [item].[group] where [name] = 'ALLINTERNAL')

		END

		COMMIT TRANSACTION
		
	END TRY
	BEGIN CATCH
		IF @@TranCount <> 0
			ROLLBACK TRANSACTION
		SET @err = ( SELECT ERROR_MESSAGE() )
		RAISERROR (@err , 16 , 1)
	END CATCH
IF @PermsRestored = 1 RAISERROR ( 'PERMRESTORE' , 16 , 1)
RETURN
END


IF @securableType = 'DOCUMENT'
	GOTO Document



Document:
BEGIN TRY
	BEGIN TRANSACTION
		IF @BlockInheritence = 1 or EXISTS (SELECT D.DocumentID 
		FROM relationship.UserGroup_Document D 
		WHERE D.DocumentID = @securableID AND INHERITED is NULL
		GROUP BY D.DocumentID)
		BEGIN
			IF EXISTS (SELECT D.DocumentID
						FROM relationship.UserGroup_Document D 
						JOIN relationship.UserGroup_File F ON D.FileID = F.FileID 
									AND D.UserGroupID = F.UserGroupID 
						WHERE D.UserGroupID = @userGroupID 
						AND D.DocumentID = @securableID GROUP BY D.DocumentID)

			BEGIN
		--RA 20/05/2011 Don't want to change the Files permissions when alter the document especially since the securableID would be a docID not a FileID
			--UPDATE relationship.UserGroup_File SET PolicyID = 
			--(SELECT  C.PolicyID FROM relationship.UserGroup_File F JOIN config.dbFile FI ON FI.fileID = F.FileID JOIN relationship.UserGroup_Client C ON C.ClientID = FI.clID 
			--AND C.UserGroupID = F.UserGroupID WHERE C.UserGroupID = @userGroupID AND F.FileID = @securableID GROUP BY C.PolicyID) WHERE UserGroupID = @userGroupID AND FileID = @securableID
			
			UPDATE D SET PolicyID = F.PolicyID,Inherited = isnull(F.Inherited,'F')
			FROM relationship.UserGroup_Document D 
			JOIN relationship.UserGroup_File F ON F.FileID = D.fileID 	AND F.UserGroupID = D.UserGroupID 
			WHERE D.UserGroupID = @userGroupID AND D.DocumentID = @securableID 
			--SET @PermsRestored = 1

			END
			ELSE 
			BEGIN
				DELETE [relationship].[UserGroup_Document] WHERE UserGroupID = @userGroupID AND DocumentID = @securableID
			END
		END
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	IF @@TranCount <> 0
		ROLLBACK TRANSACTION
	SET @err = ( SELECT ERROR_MESSAGE() )
	RAISERROR (@err , 16 , 1)
END CATCH
IF @PermsRestored = 1 RAISERROR ( 'PERMRESTORE' , 16 , 1)
RETURN



GO
GRANT EXECUTE
    ON OBJECT::[config].[DeleteUserGroupFromObject] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[DeleteUserGroupFromObject] TO [OMSAdminRole]
    AS [dbo];

