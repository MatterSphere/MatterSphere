-- =============================================
-- Author:		Renato Nappo
-- Create date: 13.03.17
-- Description:	Manage retrieval of fileFolderTreeXML for MatterSphere DM
-- =============================================
CREATE PROCEDURE [dbo].[sprRetrieveFileFolderTree] 
	
	@id bigint = 0,
	@tablename nvarchar(60) = null

as 
begin   
    declare @Sql Nvarchar(MAX)   
    declare @params nvarchar(MAX)
    declare @treeXML NVARCHAR(MAX)
    
	set @sql=N'SELECT treeXML FROM ' + @tablename + ' WHERE id =' + Convert(nvarchar,@id)
	set @params=N'@xml nvarchar(MAX) OUTPUT'

    exec sp_executesql @sql, @params, @xml = @treeXML OUTPUT
end
GO
