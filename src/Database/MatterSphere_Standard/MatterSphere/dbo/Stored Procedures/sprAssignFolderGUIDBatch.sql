-- =============================================
-- Author:		Ruslan Abdraev
-- Create date: 18.09.18
-- Description:	Assign a TreeView folder GUID to a document record in dbDocument
-- =============================================
CREATE PROCEDURE [dbo].[sprAssignFolderGUIDBatch]
	
	@docIDs xml,
	@folderGUID uniqueidentifier,
	@includeRelatedDocuments bit,
	@updatedBy int = NULL,
	@result int OUTPUT

AS
BEGIN

	SET NOCOUNT ON;

	declare @schemaName nvarchar(10) = 'dbo', @setUpdatedFields nvarchar(100) = ''

	if (exists (select *
				from INFORMATION_SCHEMA.TABLES
				where TABLE_SCHEMA = 'config'
				and TABLE_NAME = 'dbDocument'))
	begin
		set @schemaName = 'config'
	end

	if @updatedBy IS NOT NULL
	begin
		set @setUpdatedFields = ', Updated = GETUTCDATE(), UpdatedBy = ' + convert(nvarchar(12), @updatedBy)
	end
	
	begin try

		-- update the selected document
		DECLARE @updateSelectedDoc nvarchar(max) = 'update ' + @schemaName + '.dbDocument SET docFolderGUID = @folderGUID' +
		@setUpdatedFields + ' where docID in (select ''docid'' = x.v.value(''./text()[1]'', ''bigint'') from @docIDs.nodes(''items/item'') x(v))';
		exec sp_executesql @updateSelectedDoc,  N'@folderGUID nvarchar(max), @docIDs xml', @folderguid, @docIDs

		if @includeRelatedDocuments = 1 
		begin
			-- update any children of the selected document
			DECLARE @updateChildDocs nvarchar(max) = 'update ' + @schemaName + '.dbDocument SET docFolderGUID = @folderGUID' + @setUpdatedFields + ' where docParent in (select ''docid'' = x.v.value(''./text()[1]'', ''bigint'') from @docIDs.nodes(''items/item'') x(v))';
			exec sp_executesql @updateChildDocs,  N'@folderGUID nvarchar(max), @docIDs xml', @folderguid, @docIDs


			-- update the parent of the selected document
			DECLARE @updateParentDocStart nvarchar(max) = 'update ' + @schemaName  + '.dbDocument set docFolderGUID = @folderGUID' + @setUpdatedFields + ' where docID = (select docParent from ' + @schemaName + '.dbDocument where docID ';
			DECLARE @updateParentDocFinish nvarchar(20) = ')';
			SET @updateParentDocStart = @updateParentDocStart + 'in (select ''docid'' = x.v.value(''./text()[1]'', ''bigint'') from @docIDs.nodes(''items/item'') x(v))' + @updateParentDocFinish
			exec sp_executesql @updateParentDocStart,  N'@folderGUID nvarchar(max), @docIDs xml', @folderguid, @docIDs

	
			-- update the siblings of the selected document
			DECLARE @updateSiblingDocumentsStart nvarchar(max) = 'update ' + @schemaName + '.dbDocument set docFolderGUID = @folderGUID' + @setUpdatedFields + ' where docParent is not null and docParent in (select docParent from ' + @schemaName + '.dbDocument where docID in ';
			DECLARE @updateSiblingDocumentsFinish nvarchar(max) = ') and docID not in ';
			SET @updateSiblingDocumentsStart = @updateSiblingDocumentsStart + '(select ''docid'' = x.v.value(''./text()[1]'', ''bigint'') from @docIDs.nodes(''items/item'') x(v))' + @updateSiblingDocumentsFinish + '(select ''docid'' = x.v.value(''./text()[1]'', ''bigint'') from @docIDs.nodes(''items/item'') x(v))'
			exec sp_executesql @updateSiblingDocumentsStart,  N'@folderGUID nvarchar(max), @docIDs xml', @folderguid, @docIDs
		end

		select @result = 1

	end try

	begin catch

		select @result = 0;

	end catch
end