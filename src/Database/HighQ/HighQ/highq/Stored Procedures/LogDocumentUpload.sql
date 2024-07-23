CREATE PROCEDURE [highq].[LogDocumentUpload]
	@docID BIGINT,
	@usrID INT,
	@hqFileID INT
	AS
	MERGE INTO [highq].[dbDocumentUpload] AS Target
	USING(VALUES(@docID, @usrID, @hqFileID, GETUTCDATE()))
	AS Source (docID, usrID, hqFileID, UploadDate)
	ON Target.docID = Source.docID
	WHEN MATCHED THEN
	UPDATE SET usrID = Source.usrID
	, UploadDate = Source.UploadDate	
	WHEN NOT MATCHED BY Target THEN
	INSERT (docID, usrID, hqFileID, UploadDate)
	VALUES (docID, usrID, hqFileID, UploadDate);	