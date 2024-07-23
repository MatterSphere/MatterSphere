-- =============================================
-- Author:		Renato Nappo
-- Create date: 20.04.17
-- Description:	Manage retrieval of fileFolderTemplateXML for MatterSphere Configurable Type
-- =============================================
CREATE PROCEDURE [dbo].[sprRetrieveFileFolderTemplate] 
	
	@code nvarchar(15),
	@tablename nvarchar(60) = null

as 
begin   
    declare @Sql nvarchar(MAX)   
    declare @params nvarchar(MAX)
    declare @treeXML nvarchar(MAX)
    
	set @sql=N'SELECT treeXML FROM ' + @tablename + ' WHERE templatedesc =' + @code
	set @params=N'@xml nvarchar(MAX) OUTPUT'

    exec sp_executesql @sql, @params, @xml = @treeXML OUTPUT
end
GO

