-- =============================================
-- Author:		Renato Nappo
-- Create date: 07/11/17
-- Description:	Return Code Lookup data for document wallets for a particular Matter
-- =============================================
CREATE PROCEDURE [dbo].[GetMatterDocumentWalletCodes]
	@fileid bigint = null,
	@UI nvarchar(15) = null
AS
BEGIN
	SET NOCOUNT ON;

	declare @tblFolderCodes table (
		id int primary key identity,
		FolderCode nvarchar(15),
		FolderGUID uniqueidentifier
	)

	insert into @tblFolderCodes
		select distinct c.cdcode, null from dbDocument d 
		inner join dbCodeLookup c on c.cdcode = d.docWallet
		where d.fileID = @fileid
		and c.cdType = 'DFLDR_MATTER'
		and c.cdUICultureInfo = @UI

	select * from @tblFolderCodes
END


