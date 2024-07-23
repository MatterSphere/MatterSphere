-- =============================================
-- Author:		Renato Nappo
-- Create date: 20.03.17
-- Description:	Assign a TreeView folder GUID to a document record in dbDocument
-- =============================================
CREATE PROCEDURE [dbo].[sprAutoAssignFolderGUID]
	
	@id bigint,
	@emailfolderGUID uniqueidentifier, 
	@correspondencefolderGUID uniqueidentifier

AS
BEGIN

	SET NOCOUNT ON;

	declare @schemaName nvarchar(10) = 'dbo'

	if (exists (select *
				from INFORMATION_SCHEMA.TABLES
				where TABLE_SCHEMA = 'config'
				and TABLE_NAME = 'dbDocument'))
	begin
		set @schemaName = 'config'
	end



	declare @autoAssignGuidSql nvarchar(max) = ''

	set @autoAssignGuidSql = 'UPDATE ' + @schemaName + '.dbDocument
							  SET docFolderGUID = 
								  CASE 
									 WHEN (docExtension like ''%msg'' and docFolderGUID is null) THEN ''' + cast(@emailfolderGUID as nvarchar(max)) + '''
									 WHEN (docExtension not like ''%msg'' and docFolderGUID is null) THEN ''' + cast(@correspondencefolderGUID as nvarchar(max)) + '''
								  END
							  WHERE fileID = ' + cast(@id as nvarchar(30))  + ' and docFolderGUID is null and docParent is null'

	exec(@autoAssignGuidSql)

	

	declare @assignEmailAttachmentsToEmailFolder nvarchar(max) = ''
	
	set @assignEmailAttachmentsToEmailFolder = 'UPDATE ' + @schemaName + '.DBDOCUMENT 
												SET docFolderGUID = ''' + cast(@emailfolderGUID as nvarchar(max)) + '''
												WHERE docParent IS NOT NULL
												AND fileID = ' + cast(@id as nvarchar(max))  + ' and docFolderGUID is null'

	exec(@assignEmailAttachmentsToEmailFolder)
END

GO
