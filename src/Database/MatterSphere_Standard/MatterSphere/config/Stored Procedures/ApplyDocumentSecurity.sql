


CREATE PROCEDURE [config].[ApplyDocumentSecurity]
	@policyID uniqueidentifier ,
	@userGroupID uniqueidentifier ,
	@securableTypeID bigint ,
	@adminUserID int
	

AS
SET NOCOUNT ON
SET ANSI_WARNINGS OFF

-- security level check
IF (SELECT [config].[GetSecurityLevel] () & 128 ) = 0 
	RETURN

DECLARE @CLID bigint,@FILEID bigint
select @clid = clid,@fileid = fileid from config.dbDocument where docid = @securableTypeID

BEGIN TRY
	BEGIN TRANSACTION
	-- Create the document security record
	INSERT [relationship].[UserGroup_Document] ( [UserGroupID] , [DocumentID] , [PolicyID], [Clid],[FileID] )
	VALUES ( @userGroupID , @securableTypeID , @policyID, @CLID, @FILEID )

	If exists (select 1 from [relationship].[UserGroup_Document] where DocumentID = @securableTypeID and UserGroupID = (select ID from [item].[Group] where name = 'ALLINTERNAL'))
	begin
			delete from [relationship].[UserGroup_Document] where DocumentID = @securableTypeID and UserGroupID = (select ID from [item].[Group] where name = 'ALLINTERNAL')
	end
	
	-- Audit the security event
	INSERT [audit].[UserGroup_Document] ( [Created] , [CreatedBy] , [Event] ,  [UserGroupID] , [DocumentID] , [PolicyID] )
	VALUES ( GetUTCDate() , @adminUserID , 'NEWSECDOC' , @userGroupID , @securableTypeID , @policyID )
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
    ON OBJECT::[config].[ApplyDocumentSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ApplyDocumentSecurity] TO [OMSAdminRole]
    AS [dbo];

