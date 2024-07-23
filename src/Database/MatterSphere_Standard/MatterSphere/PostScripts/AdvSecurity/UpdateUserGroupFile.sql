Print 'Starting AdvSecurity\UpdateUserGroupFile.sql'

/**************  Populate new columns *****************************************/
update b set b.clid= a.clID from  config.dbFile a join [relationship].[UserGroup_File] b on a.Fileid = b.Fileid
GO

