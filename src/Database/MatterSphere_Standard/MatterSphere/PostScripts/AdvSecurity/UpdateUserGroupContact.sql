Print 'Starting AdvSecurity\UpdateUserGroupContact.sql'

/**************  Populate new columns *****************************************/
update b set b.clid= a.clID FROM [dbo].[dbClientContacts] a JOIN [relationship].[UserGroup_Contact] b ON a.[ContID] = b.ContactID
GO
