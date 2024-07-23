-- =============================================
-- Author:		Renato Nappo
-- Create date: 12.04.17
-- Description:	Manage storage of a new file folder tree template for MatterSphere DM
-- =============================================
CREATE PROCEDURE [dbo].[sprStoreNewFileFolderTreeTemplate] 
	
	@id bigint = 0,
	@description nvarchar(15),
	@treeXML nvarchar(max) = null,
	@tablename nvarchar(max) = null,
	@userID int = 0

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @SQL NVARCHAR(MAX)

	SET @SQL = 'IF ' + @description + ' is not null
					BEGIN
						IF EXISTS (SELECT * FROM ' + @tablename + ' WHERE templatedesc = ' + @description + ')
							UPDATE ' + @tablename + ' 
							SET treeXML = ''' + @treeXML + ''',
							updated = ''' + CONVERT(NVARCHAR(MAX),GETUTCDATE()) + ''',  
							updatedby = ' + CONVERT(NVARCHAR(MAX), @userID) + ' 
							WHERE templatedesc = ' + @description + '
						ELSE
							INSERT INTO ' + @tablename + ' (templateDesc, treeXML, created, createdby) VALUES (''' + replace(@description,'''','') + ''',''' + @treeXML + ''',''' + CONVERT(NVARCHAR(MAX),GETUTCDATE()) + ''',' + CONVERT(NVARCHAR(MAX), @userID) + ')
					END'
	PRINT @SQL
	EXEC (@SQL)

END
GO

