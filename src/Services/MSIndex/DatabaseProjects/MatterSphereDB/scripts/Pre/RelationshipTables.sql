IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[relationship].[Usergroup_File_Delete]') AND type in (N'U'))
BEGIN
	CREATE TABLE [relationship].[Usergroup_Client_Delete]
	(
		[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
		[clid] BIGINT NOT NULL
	)

	EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [relationship].[TR_UserGroup_Client_Delete]
   ON  [relationship].[UserGroup_Client]
   FOR DELETE NOT FOR REPLICATION
AS 
BEGIN
	SET NOCOUNT ON;

    INSERT into  [relationship].[UserGroup_Client_Delete]
	SELECT [RelationshipID],[ClientID] from deleted;

END
' 
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[relationship].[Usergroup_Contact_Delete]') AND type in (N'U'))
BEGIN
	CREATE TABLE [relationship].[Usergroup_Contact_Delete]
	(
		[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
		[ContactID] BIGINT NOT NULL
	)

	EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [relationship].[TR_UserGroup_Contact_Delete]
   ON  [relationship].[UserGroup_Contact]
   FOR DELETE NOT FOR REPLICATION
AS 
BEGIN
	SET NOCOUNT ON;

    INSERT into  [relationship].[UserGroup_Contact_Delete]
	SELECT [RelationshipID],[ContactID] from deleted;

END
' 
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[relationship].[Usergroup_Document_Delete]') AND type in (N'U'))
BEGIN
	CREATE TABLE [relationship].[Usergroup_Document_Delete]
	(
		[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
		[DocumentID] BIGINT NOT NULL
	)

	EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [relationship].[TR_UserGroup_Document_Delete]
   ON  [relationship].[UserGroup_Document]
   FOR DELETE NOT FOR REPLICATION
AS 
BEGIN
	SET NOCOUNT ON;

    INSERT into  [relationship].[UserGroup_Document_Delete]
	SELECT [RelationshipID],[DocumentID] from deleted;

END
' 
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[relationship].[Usergroup_File_Delete]') AND type in (N'U'))
BEGIN
	CREATE TABLE [relationship].[Usergroup_File_Delete]
	(
		[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
		[fileid] BIGINT NOT NULL
	)

	EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [relationship].[TR_UserGroup_File_Delete]
   ON  [relationship].[UserGroup_File]
   FOR DELETE NOT FOR REPLICATION
AS 
BEGIN
	SET NOCOUNT ON;

    INSERT into  [relationship].[UserGroup_File_Delete]
	SELECT [RelationshipID],[FileID] from deleted;

END
' 
END
