

CREATE PROCEDURE [dbo].[ApplySecurity]    
@secproc_entityId bigint,
@secproc_entityDefinition nvarchar(50),
@secproc_userId bigint = null,
@secproc_userSecurityId UniqueIdentifier = null
AS
BEGIN  

       SET NOCOUNT ON;

       DECLARE @securityTable table
       (
              PolicyID uniqueidentifier ,
              UserGroupID uniqueidentifier,
              Clid bigint,
              inherited char(1)
       )

       DECLARE @parentID bigint

       IF @secproc_entityDefinition = 'dbDocument' and ( SELECT [config].[GetSecurityLevel] () & 128 ) = 128
       BEGIN
			Update config.dbDocument set SecurityOptions = 2 where [DocID] = @secproc_entityId;
 	        SET @parentID = (SELECT TOP 1 fileid from config.dbDocument where docID = @secproc_entityId)
			IF NOT EXISTS ( SELECT PolicyID FROM [relationship].[UserGroup_File] WHERE [FileID] = @parentID )
				RETURN
			ELSE
				BEGIN
					IF EXISTS ( SELECT PolicyID FROM [relationship].[UserGroup_Document] WHERE [DocumentID] = @secproc_entityId )
						RETURN
					IF EXISTS ( SELECT 1 FROM [dbo].[dbRegInfo] where regBlockInheritence = 1)
						RETURN
							 --Get a list of the current policies inforce on the parent object
					INSERT @securityTable ( UserGroupID , PolicyID, clid ,inherited )
					SELECT UserGroupID , PolicyID, clid, case when inherited is null then 'F' else inherited end FROM [relationship].[UserGroup_File] WHERE [FileID] = @parentID 
							  --Now Create the security for the child object
					INSERT [relationship].[UserGroup_Document] ( [UserGroupID] , [DocumentID] , [PolicyID],fileid,inherited )
					SELECT UserGroupID , @secproc_entityId , PolicyID, @parentID,inherited FROM @securityTable
							 --Now Audit the event
					INSERT [audit].[UserGroup_Document] ( [Created] , [CreatedBy] , [Event] ,  [UserGroupID] , [DocumentID] , [PolicyID] )
					SELECT GetUTCDate() , @secproc_userId , 'NEWSECDOC' , UserGroupID , @secproc_entityId , PolicyID FROM @securityTable 
							  --Exit
					RETURN
              END
       END
END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ApplySecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ApplySecurity] TO [OMSAdminRole]
    AS [dbo];

