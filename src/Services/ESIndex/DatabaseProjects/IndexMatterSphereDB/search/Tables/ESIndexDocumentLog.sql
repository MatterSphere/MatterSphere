CREATE TABLE search.ESIndexDocumentLog
(
	docID BIGINT
	, ESIndexProcessID INT CONSTRAINT DF_ESIndexDocumentLog_ESIndexProcessID DEFAULT 0
	, Sys_FileName NVARCHAR(MAX)
	, Sys_FileSize BIGINT
	, Sys_ProcessTime FLOAT
	, Sys_DocIndexingError NVARCHAR(MAX)
	, Sys_ErrorCode NVARCHAR(MAX)
	, ErrB BIT
	, docExtension NVARCHAR(15)
	, EmptyContent BIT
	, LastProcessDate DATETIME 
	, CONSTRAINT PK_ESIndexDocumentLog PRIMARY KEY (ESIndexProcessID, docID)
)

GO
CREATE INDEX IX_ESIndexDocumentLog_ErrB ON search.ESIndexDocumentLog (ErrB) INCLUDE(docID)
GO

CREATE INDEX IX_ESIndexDocumentLog_docExtension ON search.ESIndexDocumentLog (docExtension) INCLUDE(ErrB, EmptyContent, ESIndexProcessID)
GO

CREATE INDEX IX_ESIndexDocumentLog_docID ON search.ESIndexDocumentLog (docID, ESIndexProcessID)
GO