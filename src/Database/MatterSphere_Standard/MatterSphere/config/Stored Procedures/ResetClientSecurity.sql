

CREATE PROCEDURE [config].[ResetClientSecurity]
	@securableID bigint

AS
SET NOCOUNT ON 
-- delete Client records
DELETE [relationship].[UserGroup_Client] WHERE ClientID = @securableID

-- delete contact records
DELETE [relationship].[UserGroup_Contact] WHERE ContactID IN ( SELECT C.contID FROM  [dbo].[dbContact] C JOIN [dbo].[dbClientContacts] CC ON C.[ContID] = CC.[ContID] WHERE CC.[ClID] = @securableID )

-- delete file records
DELETE [relationship].[UserGroup_File] WHERE fileID IN ( SELECT fileID FROM [dbo].[dbFile] WHERE [ClID] = @securableID )

-- delete document records
DELETE [relationship].[UserGroup_Document] WHERE documentID IN ( SELECT docID FROM [dbo].[dbDocument] WHERE [ClID] = @securableID )



GO
GRANT EXECUTE
    ON OBJECT::[config].[ResetClientSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ResetClientSecurity] TO [OMSAdminRole]
    AS [dbo];

