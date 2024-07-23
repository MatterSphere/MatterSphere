CREATE TABLE search.ESIndexPrecedentLog
(
	precID BIGINT
	, ESIndexProcessID INT CONSTRAINT DF_ESIndexPrecedentLog_ESIndexProcessID DEFAULT 0
	, Sys_FileName NVARCHAR(MAX)
	, Sys_FileSize BIGINT
	, Sys_ProcessTime FLOAT
	, Sys_DocIndexingError NVARCHAR(MAX)
	, Sys_ErrorCode NVARCHAR(MAX)
	, ErrB BIT
	, docExtension NVARCHAR(15)
	, EmptyContent BIT
	, LastProcessDate DATETIME 
	, CONSTRAINT PK_ESIndexPrecedentLog PRIMARY KEY (ESIndexProcessID, precID)
)
GO
CREATE INDEX IX_ESIndexPrecedentLog_ErrB ON search.ESIndexPrecedentLog (ErrB) INCLUDE(precID)
GO

CREATE INDEX IX_ESIndexPrecedentLog_docExtension ON search.ESIndexPrecedentLog (docExtension) INCLUDE(ErrB, EmptyContent, ESIndexProcessID)
GO

CREATE INDEX IX_ESIndexPrecedentLog_precID ON search.ESIndexPrecedentLog (precID, ESIndexProcessID)
GO