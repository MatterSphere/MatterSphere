-- =============================================
-- Author:		Nappo & Ince
-- Create date: 10/11/2017
-- Description:	Assign documents without docFolderGUID values to folders created from docWallet values
-- =============================================
CREATE PROCEDURE [dbo].[MapDocumentsToFoldersCreatedFromWallets]
(
	@foldercodetable dbo.tblWalletFolders readonly,
	@fileID bigint = null	
)
AS
BEGIN
		declare @i int
		declare @numrows int

		set @i = 1
		set @numrows = (select count(*) from @foldercodetable)

		if @numrows > 0
			   while (@i <= @numrows)
			   begin
					
					update dbDocument
					set docFolderGUID =	(select FolderGUID from @foldercodetable where id = @i)
					where fileID = @fileID
					and docWallet = (select FolderCode from @foldercodetable where id = @i)
					and docFolderGUID is null

					set @i = @i + 1
			   end
END
