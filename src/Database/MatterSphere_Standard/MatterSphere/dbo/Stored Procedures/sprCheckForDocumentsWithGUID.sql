-- =============================================
-- Description:	Check for Documents with a TreeView folder GUID
-- =============================================
CREATE PROCEDURE [dbo].[sprCheckForDocumentsWithGUID]
(
	@folderGUID uniqueidentifier,
	@includeDeleted bit = 1
)
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

	declare @sql nvarchar(max) = ''
	
	set @sql = 'select docID from ' + @schemaName + '.dbDocument
				where docFolderGUID = ''' + cast(@folderGUID as nchar(36)) + ''''

	if (@includeDeleted = 0)
		set @sql = @sql + ' and docDeleted = 0'

	exec(@sql)
END

GO