CREATE TABLE [highq].[dbDocumentUpload] (	
	[docID] BIGINT NOT NULL,
	[usrID] INT NOT NULL,
	[hqFileID] INT NOT NULL,
	[UploadDate] DATETIME NOT NULL,
	CONSTRAINT PK_dbDocumentUpload_docID PRIMARY KEY ([docID]))	
