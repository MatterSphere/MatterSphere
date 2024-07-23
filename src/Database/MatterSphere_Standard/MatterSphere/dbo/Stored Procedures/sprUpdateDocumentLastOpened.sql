CREATE PROCEDURE [dbo].[sprUpdateDocumentLastOpened] (@docid bigint, @opened datetime) AS
	update dbDocument set Opened = @opened where docid = @docid		