-- =============================================
-- Author:		Renato Nappo
-- Create date: 12.04.17
-- Description:	Delete a folder tree template from the system
-- =============================================
CREATE PROCEDURE [dbo].[sprDeleteFileFolderTreeTemplate]

	@id bigint = 0

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    Delete from dbFileFolderTreeTemplates where id = @id
END

GO