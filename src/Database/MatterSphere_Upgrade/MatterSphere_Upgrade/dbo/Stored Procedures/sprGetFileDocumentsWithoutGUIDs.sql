-- =============================================
-- Author:	Nappo & McLaughlin
-- Create date: 04/10/17
-- Description:	Return the documents for a Matter which do not have a folderGUID assigned to them
-- =============================================
CREATE PROCEDURE sprGetFileDocumentsWithoutGUIDs
	@fileID bigint = 0
	
AS
BEGIN
	SET NOCOUNT ON;
	select 
		docID, 
		docWallet,
		docExtension
	from dbDocument 
	where fileID = @fileID 
	and docFolderGUID is null
END
GO