CREATE PROCEDURE [dbo].[sprDocumentRecordExternal] (@DOCID nvarchar(50), @UI uUICultureInfo = '{default}',@DUPLICATECHECK BIT = 0) 
AS
SET NOCOUNT ON

IF @DUPLICATECHECK = 1
BEGIN
	select DOC.*, SP.sptype, DIR.dirpath as [docLivePath], ARCH.dirpath as [docArchivePath]from dbdocument DOC
	left join dbstorageprovider SP on DOC.doclocation = SP.spid
	left join dbdirectory DIR on DOC.docDirID = DIR.dirid
	left join dbdirectory ARCH on DOC.docDirID = ARCH.dirid
	where cast(DOC.docid as nvarchar) = @DOCID or (cast(DOC.docidold as nvarchar) = @DOCID and DOC.docid <> DOC.docidold) or docidext = @DOCID 
END
ELSE
BEGIN
	select DOC.*, SP.sptype, DIR.dirpath as [docLivePath], ARCH.dirpath as [docArchivePath]from dbdocument DOC
	left join dbstorageprovider SP on DOC.doclocation = SP.spid
	left join dbdirectory DIR on DOC.docDirID = DIR.dirid
	left join dbdirectory ARCH on DOC.docDirID = ARCH.dirid
	where docidext = @DOCID
END
