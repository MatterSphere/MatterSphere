-- =============================================
-- Author:		Renato Nappo
-- Create date: 13.03.17
-- Description:	Manage storage of fileFolderTreeXML for MatterSphere DM
-- =============================================
CREATE PROCEDURE [dbo].[sprStoreFileFolderTree] 
	
	@id bigint = 0,
	@treeXML nvarchar(max) = null,
	@tablename nvarchar(max)

AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @SQL NVARCHAR(MAX)

	SET @SQL = 'IF EXISTS (SELECT * FROM ' + @tablename + ' WHERE id = ' + CONVERT(NVARCHAR(MAX),@id) + ')
					UPDATE ' + @tablename + ' SET treeXML = ''' + @treeXML + ''' WHERE id = ' + CONVERT(NVARCHAR(MAX),@id) + '
				ELSE
					INSERT INTO ' + @tablename + ' (id, treeXML) VALUES (' + CONVERT(NVARCHAR(MAX),@id) + ',''' + @treeXML + ''')'
	PRINT @SQL
	EXEC (@SQL)
END

GO
