-- =============================================
-- Author:		Renato Nappo
-- Create date: 19.05.17
-- Description:	Delete folder tree XML for a specific Matter from the system
-- =============================================
CREATE PROCEDURE [dbo].[sprDeleteFileFolderTreeXML]

	@id bigint = 0

AS
BEGIN
	SET NOCOUNT ON;

    Delete from dbFileFolderTreeData where id = @id
END