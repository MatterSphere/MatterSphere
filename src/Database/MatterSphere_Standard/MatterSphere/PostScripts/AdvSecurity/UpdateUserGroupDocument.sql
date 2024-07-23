Print 'Starting AdvSecurity\UpdateUserGroupDocument.sql'

/**************  Populate new columns *****************************************/
update b set b.clid= a.clID,b.fileid= a.fileID from  config.dbDocument a join [relationship].[UserGroup_Document] b on a.docid = b.documentid
GO


